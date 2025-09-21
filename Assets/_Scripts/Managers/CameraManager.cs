using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform paperAirplane;
    public PlayerController player;           // drag your PlayerController here
    public Vector3 offset = new Vector3(0, 2, -4);

    [Header("Follow")]
    public float followSpeed = 5f;            // post-launch follow speed
    public float preLaunchFollowSpeed = 0f;   // 0 = no follow before launch
    public bool rampAfterLaunch = true;
    public float rampDuration = 0.4f;

    [Header("Look")]
    [Range(0f, 1f)]
    public float lookHorizontalWeight = 0.5f; // 0 = ignore player's X, 1 = fully track X
    public float lookAhead = 8f;              // how far ahead to look along forward
    public float lookAtVerticalOffset = 1.0f;
    public float yawDamp = 10f;               // how quickly the yaw catches up (optional smoothing)

    float currentFollowSpeed;
    float launchT; // 0→1
    float yawVel;  // for SmoothDampAngle

    void Start()
    {
        currentFollowSpeed = preLaunchFollowSpeed;
    }

    void LateUpdate()
    {
        if (paperAirplane == null || player == null) return;

        // Ramp follow speed after launch
        if (player.launched && rampAfterLaunch && currentFollowSpeed < followSpeed)
        {
            launchT = Mathf.Clamp01(launchT + Time.deltaTime / rampDuration);
            currentFollowSpeed = Mathf.Lerp(preLaunchFollowSpeed, followSpeed, launchT);
        }
        else if (!player.launched)
        {
            currentFollowSpeed = preLaunchFollowSpeed;
            launchT = 0f;
        }

        // --- Position: lock X, follow Y/Z with exponential smoothing ---
        Vector3 desired = new Vector3(
            transform.position.x,                        // keep camera X fixed for stability
            paperAirplane.position.y + offset.y,
            paperAirplane.position.z + offset.z
        );

        float s = 1f - Mathf.Exp(-currentFollowSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desired, s);

        // --- Look-at: blend X tracking + look-ahead along forward (flattened) ---
        // 1) Flatten the plane's forward so banking (roll) doesn't sway the look target.
        Vector3 fwdFlat = paperAirplane.forward;
        fwdFlat.y = 0f;
        if (fwdFlat.sqrMagnitude > 0.0001f) fwdFlat.Normalize();

        // 2) Blend how much of the player's X we follow (0..1)
        float blendedX = Mathf.Lerp(transform.position.x, paperAirplane.position.x, lookHorizontalWeight);

        // 3) Build the look target a bit ahead on the track
        Vector3 lookAtPoint = new Vector3(
            blendedX,
            paperAirplane.position.y + lookAtVerticalOffset,
            paperAirplane.position.z
        ) + fwdFlat * lookAhead;

        // 4) Optional: smooth yaw so it doesn’t whip when changing lanes quickly
        Vector3 toTarget = lookAtPoint - transform.position;
        if (toTarget.sqrMagnitude > 0.0001f)
        {
            float targetYaw = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;
            float currentYaw = transform.eulerAngles.y;
            float smoothedYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVel, 1f / Mathf.Max(0.0001f, yawDamp));
            Quaternion rot = Quaternion.Euler(0f, smoothedYaw, 0f);

            // Pitch toward the target too (simple look rotation with preserved smoothed yaw)
            float targetPitch = -Mathf.Atan2(toTarget.y, new Vector2(toTarget.x, toTarget.z).magnitude) * Mathf.Rad2Deg;
            Quaternion pitchRot = Quaternion.Euler(targetPitch, 0f, 0f);

            transform.rotation = rot * pitchRot;
        }
    }
}