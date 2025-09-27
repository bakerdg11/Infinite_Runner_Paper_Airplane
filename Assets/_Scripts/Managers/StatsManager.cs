using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Runtime Refs")]
    public PlayerController playerController; // bound each gameplay scene
    public GameObject startingPoint;          // spawn reference for distance

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset when entering gameplay scenes; clear when returning to main menu
        if (scene.name == "1.MainMenu")
        {
            playerController = null;
            startingPoint = null;
            distanceTravelled = 0f;
            if (distanceTravelledText) distanceTravelledText.text = "Distance: 0m";
            return;
        }

        // Bind to the persistent player for gameplay scenes
        playerController = PlayerManager.Instance != null ? PlayerManager.Instance.playerController : null;

        // Find the starting/spawn point of this scene (prefer PlayerSpawnPoint)
        startingPoint = FindStartingPointInScene(scene);

        // Fresh counters every time a gameplay scene loads
        ResetRunCounters();
        RefreshHudImmediately();
    }

    private void Update()
    {
        if (playerController != null && startingPoint != null && distanceTravelledText != null)
        {
            distanceTravelled = Vector3.Distance(
                playerController.transform.position,
                startingPoint.transform.position
            );
            distanceTravelledText.text = "Distance: " + Mathf.FloorToInt(distanceTravelled) + "m";
        }
    }

    // ───────── Public API ─────────

    public void UpdatePickupCredits(int amount)
    {
        pickupCredits += amount;
        if (pickupCreditsText != null)
            pickupCreditsText.text = "Credits: " + pickupCredits;
    }

    public void DetermineDistanceTravelled()
    {
        if (playerController == null || startingPoint == null) return;

        int finalDistance = Mathf.FloorToInt(distanceTravelled);
        distanceTravelledCredits = Mathf.FloorToInt(finalDistance * 0.01f); // example conversion
        totalCredits += distanceTravelledCredits;
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

    // ───────── Helpers ─────────

    private void ResetRunCounters()
    {
        pickupCredits = 0;
        distanceTravelled = 0f;
        distanceTravelledCredits = 0;

        if (pickupCreditsText) pickupCreditsText.text = "Credits: 0";
        if (distanceTravelledText) distanceTravelledText.text = "Distance: 0m";
    }

    private void RefreshHudImmediately()
    {
        if (pickupCreditsText) pickupCreditsText.text = "Credits: " + pickupCredits;
        if (distanceTravelledText) distanceTravelledText.text = "Distance: " + Mathf.FloorToInt(distanceTravelled) + "m";
    }

    private GameObject FindStartingPointInScene(Scene scene)
    {
        if (!scene.IsValid() || !scene.isLoaded) return null;

        // Preferred: PlayerSpawnPoint component
        var spawn = Object.FindFirstObjectByType<PlayerSpawnPoint>();
        if (spawn) return spawn.gameObject;

        // Fallback: Tag or Name "StartingPoint"
        var tagged = GameObject.FindGameObjectWithTag("StartingPoint");
        if (tagged) return tagged;

        var named = GameObject.Find("StartingPoint");
        if (named) return named;

        Debug.LogWarning("[StatsManager] No PlayerSpawnPoint/'StartingPoint' found.");
        return null;
    }
}