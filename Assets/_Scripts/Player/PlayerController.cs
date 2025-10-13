using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Rigidbody rb;

    GameManager gameManager;
    UpgradesManager upgradesManager;
    StatsManager statsManager;

    [Header("Physics")]
    public bool launched = false;
    public bool gravityEnabled = false;
    public float gravityStrength = 1f;

    [Header("Speeds")]
    public float currentSpeed;
    public float baseSpeed = 12f;
    public float boostSpeed = 25f;
    public float dashSpeed = 50f;
    public bool isBoosting = false;
    public bool isDashing = false;

    [Header("Energy")]
    public float maxEnergy = 100f;
    public float energy = 100f;
    public float energyDepletionRate = 50f;
    public bool isSteering = false;

    [Header("Missiles")]
    public Transform missileSpawnPoint;
    public GameObject missilePrefab;

    [Header("Lanes and Banking")]
    public float laneOffset = 5.5f;
    public float lateralMoveSpeed = 7.5f;
    public float maxBankAngle = 25f;
    public float bankLerpSpeed = 7f;

    private float _targetX;
    private float _bank;

    void Awake()
    {
        Instance = this;
    }

    public void InjectManagers(GameManager gm, StatsManager sm, UpgradesManager um)
    {
        gameManager = gm;
        statsManager = sm;
        upgradesManager = um;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // init energy + push to HUD
        energy = maxEnergy;
        HUD.Instance?.SetEnergyRange(0f, maxEnergy);
        HUD.Instance?.UpdateEnergy(energy);

        currentSpeed = baseSpeed;
        _targetX = 0f;
    }

    void Update()
    {
        // launch gate
        if (!launched)
        {
            if (Touchscreen.current?.primaryTouch.press.isPressed == true ||
                Mouse.current?.leftButton.isPressed == true)
                launched = true;

            if (!launched) return;
        }

        // 1) Read steering
        var steer = (TouchInputManager.Instance != null)
            ? TouchInputManager.Instance.CurrentSteer
            : SteerDirection.None;

        isSteering = (steer != SteerDirection.None);

        switch (steer)
        {
            case SteerDirection.Left:  _targetX = -laneOffset; break;
            case SteerDirection.Right: _targetX =  laneOffset; break;
            default:                   _targetX =  0f;         break;
        }

        // 2) Move forward
        Vector3 pos = transform.position;
        pos += Vector3.forward * (currentSpeed * Time.deltaTime);

        // 3) Lateral slide
        pos.x = Mathf.MoveTowards(pos.x, _targetX, lateralMoveSpeed * Time.deltaTime);
        transform.position = pos;

        // 4) Bank
        float deltaX = _targetX - pos.x;
        float targetBank = 0f;
        if (Mathf.Abs(deltaX) > 0.01f)
        {
            targetBank = (deltaX < 0f) ? maxBankAngle : -maxBankAngle;
            targetBank *= Mathf.Clamp01(Mathf.Abs(deltaX) / laneOffset);
        }
        _bank = Mathf.Lerp(_bank, targetBank, 1f - Mathf.Exp(-bankLerpSpeed * Time.deltaTime));
        var e = transform.rotation.eulerAngles; e.z = _bank;
        transform.rotation = Quaternion.Euler(e);

        // 5) Custom gravity
        if (gravityEnabled && rb != null)
            rb.linearVelocity += Vector3.down * gravityStrength * Time.deltaTime;

        // 6) Energy depletion while steering
        if (isSteering)
        {
            bool pauseDepletion = (upgradesManager != null && upgradesManager.energyDepletionPaused);
            if (!pauseDepletion)
            {
                energy = Mathf.Max(0f, energy - energyDepletionRate * Time.deltaTime);
                HUD.Instance?.UpdateEnergy(energy); // HUD renders it
            }

            if (energy <= 0f) EnableGravity();
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
            CrashConditions();
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
        statsManager.FinalizeRun();

        upgradesManager.GameEndAmmoAmmounts();
        PersistentMenuManager.Instance.OpenCrashMenu();
    }



    public void OnRespawned()
    {
        if (rb)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        NotLaunched();
        DisableGravity();

        currentSpeed = baseSpeed;
        _targetX = 0f;
        _bank = 0f;
        var rot = transform.rotation.eulerAngles; rot.z = 0f;
        transform.rotation = Quaternion.Euler(rot);

        // refill energy for the new run and update HUD immediately
        energy = maxEnergy;
        HUD.Instance?.SetEnergyRange(0f, maxEnergy);
        HUD.Instance?.UpdateEnergy(energy);
    }



    public void EnableGravity() 
    {
        gravityEnabled = true;
    }
    public void DisableGravity() 
    {
        gravityEnabled = false;
    }

    public void Launch()
    {
        launched = true;
    }
    public void NotLaunched() 
    { 
        launched = false; 
    }















    public void FireMissile()
    {
        if (missilePrefab != null && missileSpawnPoint != null)
        {
            Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        }
    }



    // ----------------- BOOST ABILITY ----------------
    public void AirplaneBoost()
    { 
        isBoosting = true; 
        UpdateCurrentSpeed(); 
    }
    public void AirplaneBoostEnd()
    { 
        isBoosting = false; 
        UpdateCurrentSpeed(); 
    }

    // ------------------ DASH ABILITY -----------------
    public void AirplaneDash() 
    { 
        isDashing = true; 
        UpdateCurrentSpeed(); 
    }
    public void AirplaneDashEnd() 
    { 
        isDashing = false; 
        UpdateCurrentSpeed();
    }

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