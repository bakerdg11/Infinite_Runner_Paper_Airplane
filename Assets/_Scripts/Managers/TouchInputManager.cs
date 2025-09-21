using UnityEngine;
using UnityEngine.InputSystem; // New Input System (future-proof; also handy for desktop testing)
using ETouch = UnityEngine.InputSystem.EnhancedTouch; // optional: for future gesture work

public enum SteerDirection { None, Left, Right }

public class TouchInputManager : MonoBehaviour
{
    public static TouchInputManager Instance;

    [Header("HUD Button Hold Components")]
    [Tooltip("Add HoldButton to your Left UI Button and drag it here.")]
    public HoldButton leftButton;
    [Tooltip("Add HoldButton to your Right UI Button and drag it here.")]
    public HoldButton rightButton;

    [Header("Debug / Desktop Testing (optional)")]
    [SerializeField] private bool allowKeyboardTest = true; // A/D or Left/Right arrows

    public SteerDirection CurrentSteer { get; private set; } = SteerDirection.None;

    private bool leftHeld;
    private bool rightHeld;

    void Awake()
    {
        // Singleton for easy access
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Wire up the UI button events
        if (leftButton != null)
        {
            leftButton.onHoldStart.AddListener(() => leftHeld = true);
            leftButton.onHoldEnd.AddListener(() => leftHeld = false);
        }
        if (rightButton != null)
        {
            rightButton.onHoldStart.AddListener(() => rightHeld = true);
            rightButton.onHoldEnd.AddListener(() => rightHeld = false);
        }

        // Enable EnhancedTouch for future swipe/tap upgrades (not strictly required for buttons)
        ETouch.EnhancedTouchSupport.Enable();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        ETouch.EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        // Optional keyboard fallback for testing in editor
        if (allowKeyboardTest && Keyboard.current != null)
        {
            leftHeld = leftButton != null ? leftButton.IsHeld : leftHeld || Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
            rightHeld = rightButton != null ? rightButton.IsHeld : rightHeld || Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
        }

        // Resolve current steer. If both pressed (or neither), go straight.
        if (leftHeld && !rightHeld) CurrentSteer = SteerDirection.Left;
        else if (rightHeld && !leftHeld) CurrentSteer = SteerDirection.Right;
        else CurrentSteer = SteerDirection.None;
    }
}





/*

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class TouchInputManager : MonoBehaviour
{
    public PaperAirplaneController airplaneController;

    public GameObject leftButton;
    public GameObject rightButton;

    [Header("Gesture Settings")]
    [SerializeField] private float doubleTapTime = 0.3f;
    [SerializeField] private float swipeThreshold = 50f; // pixels

    private float lastTapTime;
    private readonly Dictionary<int, Vector2> swipeStartByFinger = new Dictionary<int, Vector2>();
    private bool fingerActive;

    public static TouchInputManager Instance;

    // Prevent double-enabling EnhancedTouch across scenes
    private static bool s_enhancedTouchInitialized;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Wire UI button events once
        AddEventTrigger(leftButton, EventTriggerType.PointerDown, () => ResolvePlane()?.MoveToLeftLane());
        AddEventTrigger(leftButton, EventTriggerType.PointerUp, () => ResolvePlane()?.MoveToCenterLane());

        AddEventTrigger(rightButton, EventTriggerType.PointerDown, () => ResolvePlane()?.MoveToRightLane());
        AddEventTrigger(rightButton, EventTriggerType.PointerUp, () => ResolvePlane()?.MoveToCenterLane());
    }

    void OnEnable()
    {
        if (!s_enhancedTouchInitialized)
        {
            ETouch.EnhancedTouchSupport.Enable();
            ETouch.TouchSimulation.Enable();
            s_enhancedTouchInitialized = true;
        }

        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerMove += OnFingerMove;
        ETouch.Touch.onFingerUp += OnFingerUp;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        ETouch.Touch.onFingerUp -= OnFingerUp;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        // Do NOT disable EnhancedTouch here because this object persists and other scenes still use it.
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the plane in any scene (don’t rely on scene name)
        airplaneController = FindFirstObjectByType<PaperAirplaneController>();
    }

    // --- Helpers ---
    private PaperAirplaneController ResolvePlane()
    {
        // Lazy re-resolve if null (covers “first few taps” after load)
        return airplaneController ??= FindFirstObjectByType<PaperAirplaneController>();
    }

    private static bool PointerOverUIThisFrame(int fingerIndex)
    {
        // In the new Input System, for touch, pass fingerIndex; for mouse it’s -1
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR
        // In editor we often click with mouse
        return EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current.IsPointerOverGameObject(fingerIndex);
#endif
    }

    // ---- Enhanced Touch callbacks ----
    private void OnFingerDown(ETouch.Finger finger)
    {
        // If this touch started over UI, let UI have it; don’t start a gesture.
        if (PointerOverUIThisFrame(finger.index))
            return;

        // Double-tap
        float dt = Time.time - lastTapTime;
        if (dt <= doubleTapTime)
            ResolvePlane()?.OnDoubleTap();
        lastTapTime = Time.time;

        swipeStartByFinger[finger.index] = finger.screenPosition;
        fingerActive = true;
    }

    private void OnFingerMove(ETouch.Finger finger)
    {
        if (!fingerActive) return;
        if (!swipeStartByFinger.TryGetValue(finger.index, out var start)) return;

        // If finger moved over UI mid-gesture, stop consuming (prevents fighting UI)
        if (PointerOverUIThisFrame(finger.index))
        {
            fingerActive = false;
            return;
        }

        Vector2 delta = finger.screenPosition - start;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > swipeThreshold) { ResolvePlane()?.OnSwipeRight(); fingerActive = false; }
            if (delta.x < -swipeThreshold) { ResolvePlane()?.OnSwipeLeft(); fingerActive = false; }
        }
        else
        {
            if (delta.y > swipeThreshold) { ResolvePlane()?.OnSwipeUp(); fingerActive = false; }
            if (delta.y < -swipeThreshold) { ResolvePlane()?.OnSwipeDown(); fingerActive = false; }
        }
    }

    private void OnFingerUp(ETouch.Finger finger)
    {
        // If release happens over UI, ignore gesture release callback
        if (!PointerOverUIThisFrame(finger.index))
            ResolvePlane()?.OnTouchRelease();

        swipeStartByFinger.Remove(finger.index);
        fingerActive = false;
    }

    private void AddEventTrigger(GameObject obj, EventTriggerType type, UnityAction action)
    {
        if (obj == null) return;
        var trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(_ => action?.Invoke());
        trigger.triggers.Add(entry);
    }
}
*/