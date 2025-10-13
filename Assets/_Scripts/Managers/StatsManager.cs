using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [Header("Runtime Refs")]
    public PlayerController playerController; // bound each gameplay scene
    public GameObject spawnPoint;          // spawn reference for distance

    private bool _isGameplayScene;
    private bool _warnedMissing;      // log only once per scene
    private float _retryTimer;        // slow rebind attempts
    private const float RebindInterval = 0.5f; // seconds

    [Header("Credits and Ability Points")]
    public int pickupCredits;
    public TMP_Text pickupCreditsText;        // HUD
    public int totalCredits;
    public int totalAbilityPoints;

    [Header("Distance Travelled")]
    public float distanceTravelled;
    public TMP_Text distanceTravelledText;    // HUD
    public int distanceTravelledCredits;      // Win/Crashed Menu

    private int _lastDistanceShown = -1;
    private float _hudTimer = 0f;
    [SerializeField] private float hudUpdateInterval = 0.10f; // seconds (optional throttle)


    public int DistanceMeters => Mathf.FloorToInt(distanceTravelled);
    public int PickupCreditsThisRun => pickupCredits;
    public int TotalCreditsAllTime => totalCredits;
    public int DistanceTravelledCredits => distanceTravelledCredits;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Start()
    {
        // First bind attempt (in case you start in a gameplay scene)
        BindRuntimeRefs();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isGameplayScene = scene.name != "1.MainMenu";
        _warnedMissing = false;
        _retryTimer = 0f;

        if (!_isGameplayScene)
        {
            // Main menu: clear & show 0 once, then do nothing in Update
            playerController = null;
            spawnPoint = null;
            distanceTravelled = 0f;
            if (distanceTravelledText) distanceTravelledText.text = "Distance: 0m";
            return;
        }

        // Gameplay scene: try an initial bind
        BindRuntimeRefs();
    }


    void Update()
    {
        if (!_isGameplayScene) return; // no player/spawn expected on main menu

        // If refs are missing, retry binding at a slow cadence
        if (playerController == null || spawnPoint == null)
        {
            _retryTimer += Time.unscaledDeltaTime;
            if (_retryTimer >= RebindInterval)
            {
                _retryTimer = 0f;
                BindRuntimeRefs();

                if ((playerController == null || spawnPoint == null) && !_warnedMissing)
                {
                    _warnedMissing = true; // only log once per scene
                    Debug.LogWarning($"[StatsManager] Waiting for player/spawn… (player={(playerController ? "ok" : "null")}, spawn={(spawnPoint ? "ok" : "null")})");
                }
            }
            return;
        }

        // We have both refs — do the distance + HUD update
        distanceTravelled = Vector3.Distance(playerController.transform.position, spawnPoint.transform.position);
        int meters = Mathf.FloorToInt(distanceTravelled);

        // --- push to HUD only when needed ---
        _hudTimer += Time.unscaledDeltaTime;

        // Option A: update only when the shown integer changes (zero alloc most frames)
        if (meters != _lastDistanceShown)
        {
            _lastDistanceShown = meters;
            HUD.Instance?.DisplayDistanceTravelled(meters);
            _hudTimer = 0f; // reset timer if you also use throttle
        }

        // Option B (optional): also force an update every N seconds even if unchanged
        else if (_hudTimer >= hudUpdateInterval)
        {
            _hudTimer = 0f;
            HUD.Instance?.DisplayDistanceTravelled(meters);
        }
    }

    private void BindRuntimeRefs()
    {
        playerController = PlayerManager.Instance ? PlayerManager.Instance.PlayerController : null;

        // Spawn by Tag or Name ("SpawnPoint")
        spawnPoint = GameObject.FindWithTag("SpawnPoint");
        if (spawnPoint == null) spawnPoint = GameObject.Find("SpawnPoint");
    }




    // ------------------------------------------------------------Distance Travelled--------------------------

    public void ResetDistanceTravelled()
    {
        distanceTravelled = 0f;
        HUD.Instance?.DisplayDistanceTravelled(0);
    }





    // ------------------------------------------------------------Credits-------------------------------------
    public void UpdatePickupCredits(int amount)
    {
        pickupCredits += amount;
        HUD.Instance?.UpdatePickupCredits(pickupCredits);
    }

    public void ResetPickupCredits()
    {
        pickupCredits = 0;
    }




    // ------------------------------------------------------------Update Win/Loss Menus-------------------------
    public void FinalizeRun()
    {
        // credits from distance
        int finalDistance = Mathf.FloorToInt(distanceTravelled);
        distanceTravelledCredits = Mathf.FloorToInt(finalDistance * 0.01f); // your conversion
                                                                            // add this run's pickup credits to total when run ends
        totalCredits += distanceTravelledCredits;   // distance-based
        totalCredits += pickupCredits;              // pickups
    }




}