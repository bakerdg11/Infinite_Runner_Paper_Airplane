using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // NEW input system

public class PaperAirplaneController : MonoBehaviour
{
    public static PaperAirplaneController Instance;
    private Rigidbody rb;

    public GameManager gameManager;
    public UpgradesManager upgradesManager;

    [Header("Plane Physics")]
    public bool launched = false;
    public bool gravityActive = false;
    public float gravityStrength = 1f;

    [Header("Speeds")]
    public float baseSpeed = 10f;
    public float currentSpeed;
    public float boostSpeed = 50f;
    public float dashSpeed = 75f;
    public bool isBoosting = false;
    public bool isDashing = false;

    [Header("Lane Movement")]
    public float laneOffset = 5.5f;
    public float lateralMoveSpeed = 3.0f;
    private float targetX = 0f;

    public float maxBankAngle = 30f;
    public float bankSpeed = 5f;
    private float targetBankAngle = 0f;

    // Altitude motion
    public float swipeUpHeight = 8f;
    public float swipeUpDuration = 1f;
    private bool isSwipingUp = false;
    private float swipeUpTimer = 0f;
    private float originalY = 0f;

    [Header("Energy")]
    public Slider energySlider;
    public float energyDepletionRate = 0.5f;
    public bool isHoldingLeft = false;
    public bool isHoldingRight = false;

    [Header("Missiles")]
    public Transform missileSpawnPoint;
    public GameObject missilePrefab;

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

        HUD hud = FindFirstObjectByType<HUD>();
        if (hud != null)
            energySlider = hud.energySlider;
        else
            Debug.LogWarning("HUD not found in scene. Make sure it's marked as DontDestroyOnLoad.");

        upgradesManager = FindFirstObjectByType<UpgradesManager>();
        if (upgradesManager == null)
            Debug.LogWarning("AbilitiesManager not found. Make sure it persists across scenes.");

        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // --- Launching Plane (NEW INPUT SYSTEM) ---
        // Launch on first mouse click (Editor/PC) OR first touch press (mobile)
        if (!launched)
        {
            bool mouseLaunch = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            bool touchLaunch = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;

            if (mouseLaunch || touchLaunch)
            {
                IsLaunched();
                rb.linearVelocity = transform.forward * currentSpeed;
            }
        }

        if (!launched) return;

        // Apply gravity acceleration (frame-based; FixedUpdate also applies per-physics-step gravity to velocity)
        if (gravityActive)
        {
            rb.linearVelocity += Vector3.down * gravityStrength * Time.deltaTime;
        }

        // Smooth lane shifting
        Vector3 position = transform.position;
        float prevX = position.x;
        position.x = Mathf.Lerp(position.x, targetX, lateralMoveSpeed * Time.deltaTime);
        transform.position = position;

        // Determine banking target based on x movement
        float xMovement = position.x - prevX;
        const float movementThreshold = 0.01f;
        if (xMovement > movementThreshold) targetBankAngle = -maxBankAngle; // moving right => bank right (negative Z)
        else if (xMovement < -movementThreshold) targetBankAngle = maxBankAngle; // moving left  => bank left  (positive Z)
        else targetBankAngle = 0f;

        // Smoothly rotate to target bank angle (Z-axis)
        float currentZ = transform.rotation.eulerAngles.z;
        if (currentZ > 180f) currentZ -= 360f;
        float newZ = Mathf.Lerp(currentZ, targetBankAngle, bankSpeed * Time.deltaTime);
        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, newZ);
        transform.rotation = targetRotation;

        // Gaining Altitude (swipe up arc)
        if (isSwipingUp)
        {
            swipeUpTimer += Time.deltaTime;
            float t = swipeUpTimer / swipeUpDuration;
            float heightOffset = Mathf.Sin(Mathf.PI * t) * swipeUpHeight;

            Vector3 pos = transform.position;
            pos.y = originalY + heightOffset;
            transform.position = pos;

            if (swipeUpTimer >= swipeUpDuration)
                isSwipingUp = false;
        }

        // Energy Bar Depletion (only while holding a lane button)
        if (!upgradesManager.energyDepletionPaused && (isHoldingLeft || isHoldingRight))
        {
            energySlider.value -= energyDepletionRate * Time.deltaTime;
            energySlider.value = Mathf.Max(energySlider.value, 0);
        }

        // Energy Depleted ? Enable Gravity
        if (energySlider.value <= 0)
        {
            EnableGravity();
        }
    }

    void FixedUpdate()
    {
        if (!launched) return;

        Vector3 velocity = transform.forward * currentSpeed;

        // Add gravity only if it's active (per physics step)
        if (gravityActive)
        {
            velocity += Vector3.down * gravityStrength;
        }

        rb.linearVelocity = velocity;
    }

    public void EnableGravity() { gravityActive = true; }
    public void DisableGravity() { gravityActive = false; }

    public void IsLaunched() { launched = true; }
    public void NotLaunched() { launched = false; }

    // ----- Lane movement API (called by TouchInputManager via EventTriggers) -----
    public void MoveToLeftLane()
    {
        targetX = -laneOffset;
        targetBankAngle = maxBankAngle;
        isHoldingLeft = true;
    }

    public void MoveToRightLane()
    {
        targetX = laneOffset;
        targetBankAngle = -maxBankAngle;
        isHoldingRight = true;
    }

    public void MoveToCenterLane()
    {
        targetX = 0f;
        targetBankAngle = 0f;
        isHoldingLeft = false;
        isHoldingRight = false;
    }

    // ----- Gesture hooks from TouchInputManager -----
    public void OnDoubleTap() { /* e.g., Boost or Dash trigger if you want */ }
    public void OnSwipeLeft() { /* optional: lane, roll, or dash logic */ }
    public void OnSwipeRight() { /* optional: lane, roll, or dash logic */ }
    public void OnSwipeUp()
    {
        if (!isSwipingUp && launched)
        {
            isSwipingUp = true;
            swipeUpTimer = 0f;
            originalY = transform.position.y;
        }
    }
    public void OnSwipeDown() { /* optional */ }
    public void OnTouchRelease() { /* optional */ }

    // ----- Missile -----
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
            gameManager.DetermineDistanceTravelled();
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

        gameManager.DetermineDistanceTravelled();
        gameManager.UpdateTotalCredits();
        gameManager.UpdateCrashedMenuStats();

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