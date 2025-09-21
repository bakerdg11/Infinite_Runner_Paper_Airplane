using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;


    [Header("Forward Motion")]
    public float forwardSpeed = 12f;
    public bool launched = false;

    [Header("Lanes")]
    public float laneOffset = 5.5f;
    public float lateralMoveSpeed = 7.5f;

    [Header("Banking (Visual Tilt)")]
    public float maxBankAngle = 25f;
    public float bankLerpSpeed = 7f;

    // Internals
    private float _targetX;   // desired lane x
    private float _bank;      // current roll angle in degrees

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // Start in center lane
        _targetX = 0f;
    }
    void Update()
    {
        // --- Wait for any touch or click to launch ---
        if (!launched)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                launched = true;
            }
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed) // editor fallback
            {
                launched = true;
            }

            return; // don’t move until launched
        }


        // 1) Decide target lane from TouchInputManager’s steer state
        var steer = TouchInputManager.Instance != null ? TouchInputManager.Instance.CurrentSteer : SteerDirection.None;

        switch (steer)
        {
            case SteerDirection.Left: _targetX = -laneOffset; break;
            case SteerDirection.Right: _targetX = laneOffset; break;
            default: _targetX = 0f; break; // center
        }

        // 2) Move forward constantly
        Vector3 pos = transform.position;
        pos += Vector3.forward * (forwardSpeed * Time.deltaTime);

        // 3) Slide toward target lane on X
        float newX = Mathf.MoveTowards(pos.x, _targetX, lateralMoveSpeed * Time.deltaTime);
        pos.x = newX;
        transform.position = pos;

        // 4) Banking logic driven by movement direction (not button state)
        float deltaX = _targetX - pos.x;                 // < 0 = moving left, > 0 = moving right
        bool stillMovingHorizontally = Mathf.Abs(deltaX) > 0.01f;

        float targetBank = 0f;
        if (stillMovingHorizontally)
        {
            // If we’re moving left, bank left (+Z). If moving right, bank right (-Z).
            targetBank = (deltaX < 0f) ? maxBankAngle : -maxBankAngle;

            // Optional: make bank scale with how far we are from target (feels extra nice)
            float t = Mathf.Clamp01(Mathf.Abs(deltaX) / laneOffset);
            targetBank *= t;
        }
        else
        {
            // At lane: level out
            targetBank = 0f;
        }

        _bank = Mathf.Lerp(_bank, targetBank, 1f - Mathf.Exp(-bankLerpSpeed * Time.deltaTime));
        var rot = transform.rotation.eulerAngles;
        rot.z = _bank;
        transform.rotation = Quaternion.Euler(rot);
    }
}