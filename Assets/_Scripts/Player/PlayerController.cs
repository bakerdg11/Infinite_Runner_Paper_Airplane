using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Rigidbody rb;


    public GameManager gameManager;
    public StatsManager statsManager;
    public UpgradesManager upgradesManager;
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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;


        gameManager = GameManager.Instance;
        if (gameManager == null)
            Debug.LogError("GameManager not found!");

        statsManager = StatsManager.Instance;
        if (statsManager == null)
            Debug.LogError("StatsManager not found!");

        upgradesManager = UpgradesManager.Instance;
        if (upgradesManager == null)
            Debug.LogError("UpgradesManager not found!");

        TryAttachEnergySlider();

        currentSpeed = baseSpeed;
        // Start in center lane
        _targetX = 0f;
    }



    private void Update()
    {
        // Idle at spawn until the player taps/clicks
        if (!launched)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
                launched = true;
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed) // editor fallback
                launched = true;

            if (!launched) return; // don�t move until launched
        }

        // 1) Read steering
        var steer = TouchInputManager.Instance != null
            ? TouchInputManager.Instance.CurrentSteer
            : SteerDirection.None;

        switch (steer)
        {
            case SteerDirection.Left: _targetX = -laneOffset; break;
            case SteerDirection.Right: _targetX = laneOffset; break;
            default: _targetX = 0f; break; // center
        }

        // 2) Move forward constantly (use currentSpeed so future boosts work)
        Vector3 pos = transform.position;
        pos += Vector3.forward * (currentSpeed * Time.deltaTime);

        // 3) Slide toward target lane on X
        float newX = Mathf.MoveTowards(pos.x, _targetX, lateralMoveSpeed * Time.deltaTime);
        pos.x = newX;
        transform.position = pos;

        // 4) Bank (roll) while laterally moving; level when centered
        float deltaX = _targetX - pos.x;                     // < 0 = moving left, > 0 = moving right
        bool movingHorizontally = Mathf.Abs(deltaX) > 0.01f;

        float targetBank = 0f;
        if (movingHorizontally)
        {
            targetBank = (deltaX < 0f) ? maxBankAngle : -maxBankAngle;
            float t = Mathf.Clamp01(Mathf.Abs(deltaX) / laneOffset);
            targetBank *= t; // scale bank by how far you are from lane target
        }
        _bank = Mathf.Lerp(_bank, targetBank, 1f - Mathf.Exp(-bankLerpSpeed * Time.deltaTime));

        var e = transform.rotation.eulerAngles;
        e.z = _bank;
        transform.rotation = Quaternion.Euler(e);

        // 5) Custom gravity (use rb.velocity, not linearVelocity)
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




    public void EnableGravity() { gravityActive = true; }
    public void DisableGravity() { gravityActive = false; }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && gravityActive)
        {
            CrashConditions();
        }

        if (other.CompareTag("Obstacle"))
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