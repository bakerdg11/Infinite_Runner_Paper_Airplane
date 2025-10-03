using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Rigidbody rb;


    GameManager gameManager;
    UpgradesManager upgradesManager;
    StatsManager statsManager;

    public Slider energySlider;




    [Header("Physics")]
    public bool launched = false;
    public bool gravityActive = false;
    public float gravityStrength = 1f;

    [Header("Speeds")]
    public float currentSpeed;
    public float baseSpeed = 12f;
    public float boostSpeed = 25f;
    public float dashSpeed = 50f;
    public bool isBoosting = false;
    public bool isDashing = false;

    [Header("Energy")]

    public float energyDepletionRate = 0.5f;
    public bool isSteering = false;

    [Header("Missiles")]
    public Transform missileSpawnPoint;
    public GameObject missilePrefab;


    [Header("Lanes and Banking")]
    public float laneOffset = 5.5f;
    public float lateralMoveSpeed = 7.5f;
    public float maxBankAngle = 25f;
    public float bankLerpSpeed = 7f;
    // Internals
    private float _targetX;   // desired lane x
    private float _bank;      // current roll angle in degrees


    private void Awake()
    {
        Instance = this;
    }


    // Called by PlayerManager after the player exists in the scene
    public void Inject(GameManager gm, UpgradesManager um, StatsManager sm)
    {
        gameManager = gm;
        upgradesManager = um;
        statsManager = sm;
    }


    void Start()
    {
        // Failsafe: if someone dragged a Player into a test scene without the PlayerManager,
        // we can still try to resolve singletons to stay functional in-editor.
        if (gameManager == null || upgradesManager == null || statsManager == null)
        {
            TryResolveServices();
        }


        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        TryAttachEnergySlider();

        currentSpeed = baseSpeed;
        _targetX = 0f;
    }



    private void Update()
    {
        if (energySlider == null) TryAttachEnergySlider();

        // Idle at spawn until the player taps/clicks
        if (!launched)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
                launched = true;
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed) // editor fallback
                launched = true;

            if (!launched) return; // don't move until launched
        }

        // 1) Read steering
        var steer = (TouchInputManager.Instance != null)
            ? TouchInputManager.Instance.CurrentSteer
            : SteerDirection.None;

        // IMPORTANT: drive energy from input
        isSteering = (steer != SteerDirection.None);

        switch (steer)
        {
            case SteerDirection.Left: _targetX = -laneOffset; break;
            case SteerDirection.Right: _targetX = laneOffset; break;
            default: _targetX = 0f; break; // center
        }

        // 2) Move forward constantly
        Vector3 pos = transform.position;
        pos += Vector3.forward * (currentSpeed * Time.deltaTime);

        // 3) Slide toward target lane on X
        float newX = Mathf.MoveTowards(pos.x, _targetX, lateralMoveSpeed * Time.deltaTime);
        pos.x = newX;
        transform.position = pos;

        // 4) Bank (roll)
        float deltaX = _targetX - pos.x;
        bool movingHorizontally = Mathf.Abs(deltaX) > 0.01f;

        float targetBank = 0f;
        if (movingHorizontally)
        {
            targetBank = (deltaX < 0f) ? maxBankAngle : -maxBankAngle;
            float t = Mathf.Clamp01(Mathf.Abs(deltaX) / laneOffset);
            targetBank *= t;
        }
        _bank = Mathf.Lerp(_bank, targetBank, 1f - Mathf.Exp(-bankLerpSpeed * Time.deltaTime));

        var e = transform.rotation.eulerAngles;
        e.z = _bank;
        transform.rotation = Quaternion.Euler(e);

        // 5) Custom gravity (use rb.velocity)
        if (gravityActive && rb != null)
        {
            rb.linearVelocity += Vector3.down * gravityStrength * Time.deltaTime;
        }

        // 6) Optional energy depletion while steering
        if (isSteering && energySlider != null)
        {
            bool pauseDepletion = (upgradesManager != null && upgradesManager.energyDepletionPaused);
            if (!pauseDepletion)
            {
                energySlider.value = Mathf.Max(0, energySlider.value - energyDepletionRate * Time.deltaTime);
            }

            if (energySlider.value <= 0) EnableGravity();
        }
    }


    void TryResolveServices()
    {
        gameManager = gameManager ?? GameManager.Instance;
        upgradesManager = upgradesManager ?? UpgradesManager.Instance;
        statsManager = statsManager ?? StatsManager.Instance;

        if (gameManager == null || upgradesManager == null || statsManager == null)
        {
            Debug.LogWarning("[PlayerController] One or more services not found. " +
                             "Ensure Bootstrap and managers are loaded before the Player.");
        }
    }


    public void SetEnergySlider(Slider slider)
    {
        energySlider = slider;
    }

    public void ResetPlayerForLevel()
    {
        Debug.Log("ResetPlayerForLevel() called");

        launched = false;
        gravityActive = false;
        currentSpeed = baseSpeed;

        if (rb)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        _targetX = 0f;
        _bank = 0f;
        var rot = transform.rotation.eulerAngles;
        rot.z = 0f;
        transform.rotation = Quaternion.Euler(rot);

        if (energySlider != null)
        {
            Debug.Log("Resetting Slider");
            energySlider.value = energySlider.minValue;
        }

    }






    private bool TryAttachEnergySlider()
    {
        if (energySlider != null) return true;

        var hud = HUD.Instance;
        if (hud != null && hud.energySlider != null)
        {
            energySlider = hud.energySlider;
            return true;
        }

        Debug.LogWarning("HUD or energySlider not ready. Ensure HUD persists and has the Slider assigned.");
        return false;
    }


    public void DepleteEnergy()
    {
        isSteering = true;
    }

    public void PauseDepleteEnergy()
    {
        isSteering = false;
    }




    public void EnableGravity() 
    {
        gravityActive = true;
    }
    public void DisableGravity() 
    {
        gravityActive = false;
    }

    public void Launch()
    {
        launched = true;
    }
    public void NotLaunched() { launched = false; }




    public void ResetForNewRun()
    {
        launched = false;
        gravityActive = false;

        if (rb)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // center lane & zero roll
        _targetX = 0f;
        _bank = 0f;
        var rot = transform.rotation.eulerAngles;
        rot.z = 0f;
        transform.rotation = Quaternion.Euler(rot);

        // reset forward speed baseline
        currentSpeed = baseSpeed;

        // reset energy if present
        if (energySlider != null)
            energySlider.value = energySlider.maxValue;
    }

    public void ResetForLevel()
    {
        launched = false;

        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        _targetX = 0f;
        _bank = 0f;
        var rot = transform.rotation.eulerAngles;
        rot.z = 0f;
        transform.rotation = Quaternion.Euler(rot);
    }









    public void FireMissile()
    {
        if (missilePrefab != null && missileSpawnPoint != null)
        {
            Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        }
    }




    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            CrashConditions();
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!upgradesManager.invincibleEnabled && !upgradesManager.dashEnabled)
            {
                CrashConditions();
            }
        }
    }


    public void CrashConditions()
    {
        Debug.Log("Plane Crashed");
        Time.timeScale = 0f;
        rb.linearVelocity = Vector3.zero;

        DisableGravity();
        NotLaunched();

        statsManager.DetermineDistanceTravelled();
        statsManager.UpdateTotalCredits();
        statsManager.UpdateCrashedMenuStats();

        upgradesManager.GameEndAmmoAmmounts();
        PersistentMenuManager.Instance.OpenCrashMenu();
    }



    // ----------------- BOOST ABILITY ----------------
    public void AirplaneBoost() { isBoosting = true; UpdateCurrentSpeed(); }
    public void AirplaneBoostEnd() { isBoosting = false; UpdateCurrentSpeed(); }

    // ------------------ DASH ABILITY -----------------
    public void AirplaneDash() { isDashing = true; UpdateCurrentSpeed(); }
    public void AirplaneDashEnd() { isDashing = false; UpdateCurrentSpeed(); }

    private void UpdateCurrentSpeed()
    {
        if (isDashing)
        {
            currentSpeed = dashSpeed;
            isBoosting = false; // dash overrides boost
        }
        else if (isBoosting)
        {
            currentSpeed = boostSpeed;
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        Debug.Log($"[Speed Update] Dashing: {isDashing}, Boosting: {isBoosting}, Speed: {currentSpeed}");
    }



}