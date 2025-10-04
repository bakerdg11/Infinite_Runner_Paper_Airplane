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

    [Header("Crashed Menu Results Texts")]
    public TMP_Text crashedMenuDistanceTravelledText;
    public TMP_Text crashedMenuDistanceCreditsText;
    public TMP_Text crashedMenuPickupCreditsText;
    public TMP_Text crashedMenuTotalCreditsText;

    [Header("Win Menu Results Texts")]
    public TMP_Text winMenuDistanceTravelledText;
    public TMP_Text winMenuDistanceCreditsText;
    public TMP_Text winMenuPickupCreditsText;
    public TMP_Text winMenuTotalCreditsText;


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
        if (distanceTravelledText)
            distanceTravelledText.text = "Distance: " + Mathf.FloorToInt(distanceTravelled) + "m";
    }

    private void BindRuntimeRefs()
    {
        playerController = PlayerManager.Instance ? PlayerManager.Instance.PlayerController : null;

        // Spawn by Tag or Name ("SpawnPoint")
        spawnPoint = GameObject.FindWithTag("SpawnPoint");
        if (spawnPoint == null) spawnPoint = GameObject.Find("SpawnPoint");
    }

    public void DetermineDistanceTravelled()
    {
        int finalDistance = Mathf.FloorToInt(distanceTravelled);
        distanceTravelledCredits = Mathf.FloorToInt(finalDistance * 0.01f); // example conversion
        totalCredits += distanceTravelledCredits;
    }

    public void UpdatePickupCredits(int amount)
    {
        pickupCredits += amount;
        if (pickupCreditsText != null)
            pickupCreditsText.text = "Credits: " + pickupCredits;
    }

    public void UpdateTotalCredits()
    {
        totalCredits += pickupCredits;
    }

    public void UpdateCrashedMenuStats()
    {
        int finalDistance = Mathf.FloorToInt(distanceTravelled);

        if (crashedMenuDistanceTravelledText) crashedMenuDistanceTravelledText.text = "Distance Travelled: " + finalDistance + "m";
        if (crashedMenuDistanceCreditsText) crashedMenuDistanceCreditsText.text = "Travelled Credits: " + distanceTravelledCredits;
        if (crashedMenuPickupCreditsText) crashedMenuPickupCreditsText.text = "Credits Collected: " + pickupCredits;
        if (crashedMenuTotalCreditsText) crashedMenuTotalCreditsText.text = "Total Credits: " + totalCredits;
    }

    public void UpdateWinMenuStats()
    {
        int finalDistance = Mathf.FloorToInt(distanceTravelled);

        if (winMenuDistanceTravelledText) winMenuDistanceTravelledText.text = "Distance Travelled: " + finalDistance + "m";
        if (winMenuDistanceCreditsText) winMenuDistanceCreditsText.text = "Travelled Credits: " + distanceTravelledCredits;
        if (winMenuPickupCreditsText) winMenuPickupCreditsText.text = "Credits Collected: " + pickupCredits;
        if (winMenuTotalCreditsText) winMenuTotalCreditsText.text = "Total Credits: " + totalCredits;
    }



}