using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinSpawner : MonoBehaviour
{
    [Header("Definitions (assign your SOs)")]
    public CoinDefinition copperDef;
    public CoinDefinition silverDef;
    public CoinDefinition goldDef;

    [Header("Fixed per-level counts")]
    [Min(0)] public int copperCount = 11;
    [Min(0)] public int silverCount = 3;
    [Min(0)] public int goldCount = 1;

    [Header("Spawn Point Tag")]
    [SerializeField] private string spawnPointTag = "CreditsSpawnPoint";

    // Runtime
    private readonly List<Transform> _spawnPoints = new();
    private readonly List<GameObject> _liveCoins = new();
    private string _currentScene = "";
    private bool _spawnedThisScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentScene = scene.name;
        _spawnedThisScene = false;

        // Clear any leftovers (e.g., returning from another scene)
        DespawnAll();

        // Skip main menu
        if (_currentScene == "1.MainMenu") return;

        // Collect spawn points in this scene and spawn once
        RefreshSpawnPoints();
        SpawnFixedSet();
        _spawnedThisScene = true;
    }

    private void RefreshSpawnPoints()
    {
        _spawnPoints.Clear();

        // include inactive objects, then filter by tag
        var all = FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var t in all)
            if (t.CompareTag(spawnPointTag))
                _spawnPoints.Add(t);
    }

    private void SpawnFixedSet()
    {
        // Build the desired sequence: e.g., [G, S, S, S, C...]
        var plan = BuildPlan();
        if (plan.Count == 0 || _spawnPoints.Count == 0)
        {
            return;
        }

        // Shuffle both the plan and the points so placement looks random
        Shuffle(plan);
        Shuffle(_spawnPoints);

        int toSpawn = Mathf.Min(plan.Count, _spawnPoints.Count);
        for (int i = 0; i < toSpawn; i++)
        {
            var def = plan[i];
            if (def == null || def.coinPrefab == null)
            {
                Debug.LogWarning("[CoinSpawner] Missing CoinDefinition or prefab in plan; skipping one.");
                continue;
            }

            var p = _spawnPoints[i];
            var coin = Instantiate(def.coinPrefab, p.position, p.rotation, p); // parent to point
            _liveCoins.Add(coin);
        }

        if (toSpawn < plan.Count)
        {
            Debug.LogWarning($"[CoinSpawner] Had a plan for {plan.Count} coins but only {_spawnPoints.Count} spawn points. Spawned {toSpawn}.");
        }
    }

    private List<CoinDefinition> BuildPlan()
    {
        var plan = new List<CoinDefinition>(copperCount + silverCount + goldCount);
        for (int i = 0; i < goldCount; i++) plan.Add(goldDef);
        for (int i = 0; i < silverCount; i++) plan.Add(silverDef);
        for (int i = 0; i < copperCount; i++) plan.Add(copperDef);
        return plan;
    }

    private void DespawnAll()
    {
        for (int i = _liveCoins.Count - 1; i >= 0; i--)
        {
            if (_liveCoins[i] != null) Destroy(_liveCoins[i]);
        }
        _liveCoins.Clear();
    }

    // Fisher–Yates shuffle
    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // Optional: manual respawn if you ever need it
    public void RespawnForCurrentScene()
    {
        if (_currentScene == "1.MainMenu") return;
        DespawnAll();
        RefreshSpawnPoints();
        SpawnFixedSet();
        _spawnedThisScene = true;
    }
}