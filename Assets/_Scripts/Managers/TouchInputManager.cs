using UnityEngine;
using UnityEngine.InputSystem;                      // New Input System
using ETouch = UnityEngine.InputSystem.EnhancedTouch; // For future gesture work

public enum SteerDirection { None, Left, Right }

public class TouchInputManager : MonoBehaviour
{
    public static TouchInputManager Instance;

    [Header("HUD Button Hold Components")]
    [Tooltip("Assign the HoldButton on your LEFT UI button.")]
    public HoldButton leftButton;
    [Tooltip("Assign the HoldButton on your RIGHT UI button.")]
    public HoldButton rightButton;

    [Header("Debug / Desktop Testing (optional)")]
    [SerializeField] private bool allowKeyboardTest = true; // A/D or arrow keys

    public SteerDirection CurrentSteer { get; private set; } = SteerDirection.None;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Enable EnhancedTouch (not strictly required for buttons; future-friendly)
        ETouch.EnhancedTouchSupport.Enable();
    }

    private void Start()
    {
        AutoWireButtonsIfMissing();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        ETouch.EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        bool leftHeld = (leftButton != null && leftButton.IsHeld);
        bool rightHeld = (rightButton != null && rightButton.IsHeld);

        // Editor/desktop fallback
        if (allowKeyboardTest && Keyboard.current != null)
        {
            leftHeld = leftHeld || Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
            rightHeld = rightHeld || Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
        }

        // Resolve steer: if both (or neither) pressed → None
        if (leftHeld && !rightHeld) CurrentSteer = SteerDirection.Left;
        else if (rightHeld && !leftHeld) CurrentSteer = SteerDirection.Right;
        else CurrentSteer = SteerDirection.None;
    }

    void AutoWireButtonsIfMissing()
    {
        if (leftButton == null || rightButton == null)
        {
            // Find the first two HoldButtons under any active Canvas
            var canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var c in canvases)
            {
                var holds = c.GetComponentsInChildren<HoldButton>(true);
                foreach (var h in holds)
                {
                    var n = h.gameObject.name.ToLowerInvariant();
                    if (leftButton == null && n.Contains("left")) leftButton = h;
                    if (rightButton == null && n.Contains("right")) rightButton = h;
                }
            }
        }
    }



}