using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance { get; private set; }

    [Header("Player Setup")]
    [SerializeField] private GameObject playerPrefab;     // assign in Bootstrap
    [SerializeField] private Transform explicitSpawnPoint; // optional override

    [Header("Runtime")]
    public PlayerController PlayerController { get; private set; }
    public GameObject PlayerGO { get; private set; }    // Physical player object in the scene once instantiated. 

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
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        EnsurePlayerExists();
        BindPlayerComponents();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-locate spawn and re-bind after scene loads
        EnsurePlayerExists();
        MovePlayerToSpawnPoint();
        BindPlayerComponents();
    }

    /// <summary>
    /// Spawns the player if none exists yet. If one is already present (placed in scene), uses it.
    /// </summary>
    public void EnsurePlayerExists()
    {
        if (PlayerGO != null) return;

        var existing = FindFirstObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        if (existing != null)
        {
            PlayerGO = existing.gameObject;
            PlayerController = existing;
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("[PlayerManager] Player Prefab not assigned.");
            return;
        }

        // Spawn with a neutral position; don't query SpawnPoint here.
        PlayerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        PlayerController = PlayerGO.GetComponent<PlayerController>();
        if (PlayerController == null)
            Debug.LogError("[PlayerManager] PlayerController missing on prefab.");
    }

    /// <summary>
    /// Attempts to bind PlayerController and notify it of managers.
    /// </summary>
    public void BindPlayerComponents()
    {
        if (PlayerGO == null)
        {
            Debug.LogWarning("[PlayerManager] No PlayerGO to bind.");
            return;
        }

        if (PlayerController == null)
            PlayerController = PlayerGO.GetComponent<PlayerController>();

        if (PlayerController == null)
        {
            Debug.LogError("[PlayerManager] PlayerController missing on PlayerGO.");
            return;
        }

        // Provide references from your existing singletons (already DontDestroyOnLoad)
        var gameManager = GameManager.Instance;
        var statsManager = StatsManager.Instance;
        var upgradesManager = UpgradesManager.Instance;

        if (gameManager == null) Debug.LogWarning("[PlayerManager] GameManager.Instance not found.");
        if (statsManager == null) Debug.LogWarning("[PlayerManager] StatsManager.Instance not found.");
        if (upgradesManager == null) Debug.LogWarning("[PlayerManager] UpgradesManager.Instance not found.");

        PlayerController.InjectManagers(gameManager, statsManager, upgradesManager);
    }

    /// <summary>
    /// Finds a spawn position and moves the player there.
    /// </summary>
    public void MovePlayerToSpawnPoint()
    {
        if (PlayerGO == null) return;

        var spawn = GameObject.FindWithTag("SpawnPoint") ?? GameObject.Find("SpawnPoint");
        if (spawn != null)
        {
            // move the player
            PlayerGO.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
        }
        else
        {
            Debug.LogWarning("[PlayerManager] No SpawnPoint found in this scene.");
        }
    }

    private Vector3 GetSpawnPosition()
    {
        var found = GameObject.Find("SpawnPoint");
        if (found != null) return found.transform.position;

        Debug.LogWarning("[PlayerManager] No SpawnPoint found in this scene.");
        return Vector3.zero; // fallback
    }









}