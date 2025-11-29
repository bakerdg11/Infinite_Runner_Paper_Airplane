using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    Camera _camera;

    public Transform paperAirplane;     // will auto-fill from the player
    public PlayerController player;     // will auto-fill from PlayerManager
    public Vector3 offset = new Vector3(0, 2, -4);

    [Header("Follow")]
    public float followSpeed = 5f;
    public float preLaunchFollowSpeed = 0f;   // 0 = no follow before launch
    public bool rampAfterLaunch = true;
    public float rampDuration = 0.4f;

    [Header("Look")]
    [Range(0f, 1f)]
    public float lookHorizontalWeight = 0.5f;
    public float lookAhead = 8f;
    public float lookAtVerticalOffset = 1.0f;
    public float yawDamp = 10f;

    float currentFollowSpeed;
    float launchT;  // 0→1
    float yawVel;   // for SmoothDampAngle

    private void Awake()
    {
        // Singleton pattern: keep only one CameraManager + Camera alive
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // another instance already exists, kill this one
            return;
        }

        Instance = this;
        _camera = GetComponent<Camera>();     // should be on the same GameObject as CameraManager
        DontDestroyOnLoad(gameObject);        // ⬅ this keeps the camera across scenes

        // Listen for scene loads so we can clean up any extra cameras
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ⬅ NEW: called whenever a new scene has finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure this is still our active instance
        if (Instance != this) return;

        // Find all cameras in the scene
        var allCameras = FindObjectsOfType<Camera>();

        foreach (var cam in allCameras)
        {
            // If it's NOT our persistent camera, destroy it
            if (cam != _camera)
            {
                Destroy(cam.gameObject);
            }
        }

        // After level loads, your LateUpdate logic will re-hook the player from PlayerManager
        // thanks to this block already in your script:
        //
        // if ((player == null || paperAirplane == null) && PlayerManager.Instance != null)
        // {
        //     player = PlayerManager.Instance.PlayerController;
        //     paperAirplane = player != null ? player.transform : null;
        // }
    }

    void Start()
    {
        currentFollowSpeed = preLaunchFollowSpeed;
        
        // Auto-hook the persistent player if not set in the Inspector
        if (player == null && PlayerManager.Instance != null)
            player = PlayerManager.Instance.PlayerController;

        if (player != null && paperAirplane == null)
            paperAirplane = player.transform;
        
    }

    void LateUpdate()
    {
        // One-time fallback in case script order delayed the player hookup
        if ((player == null || paperAirplane == null) && PlayerManager.Instance != null)
        {
            player = PlayerManager.Instance.PlayerController;
            paperAirplane = player != null ? player.transform : null;
        }
        
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

        // Position follow (lock X for stability)
        Vector3 desired = new Vector3(
            transform.position.x,
            paperAirplane.position.y + offset.y,
            paperAirplane.position.z + offset.z
        );
        float s = 1f - Mathf.Exp(-currentFollowSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desired, s);

        // Look target: blend X + look-ahead along flattened forward
        Vector3 fwdFlat = paperAirplane.forward; fwdFlat.y = 0f;
        if (fwdFlat.sqrMagnitude > 0.0001f) fwdFlat.Normalize();

        float blendedX = Mathf.Lerp(transform.position.x, paperAirplane.position.x, lookHorizontalWeight);
        Vector3 lookAtPoint = new Vector3(
            blendedX,
            paperAirplane.position.y + lookAtVerticalOffset,
            paperAirplane.position.z
        ) + fwdFlat * lookAhead;

        Vector3 toTarget = lookAtPoint - transform.position;
        if (toTarget.sqrMagnitude > 0.0001f)
        {
            float targetYaw = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;
            float currentYaw = transform.eulerAngles.y;
            float smoothedYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVel, 1f / Mathf.Max(0.0001f, yawDamp));
            Quaternion rot = Quaternion.Euler(0f, smoothedYaw, 0f);

            float targetPitch = -Mathf.Atan2(toTarget.y, new Vector2(toTarget.x, toTarget.z).magnitude) * Mathf.Rad2Deg;
            Quaternion pitchRot = Quaternion.Euler(targetPitch, 0f, 0f);

            transform.rotation = rot * pitchRot;
        }
    }
}