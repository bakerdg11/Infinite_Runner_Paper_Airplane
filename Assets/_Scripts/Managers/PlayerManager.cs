using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Player Setup")]
    [SerializeField] private GameObject playerPrefab;     // assign in Bootstrap
    [SerializeField] private Transform explicitSpawnPoint; // optional override (unused here)

    [Header("Runtime")]
    public PlayerController PlayerController { get; private set; }
    public GameObject PlayerGO { get; private set; }

    [SerializeField] private string mainMenuSceneName = "1.MainMenu";

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
        EnsurePlayerExists();
        BindPlayerComponents();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsurePlayerExists();

        // If Main Menu, doesn't make player visible
        bool isMainMenu = string.Equals(scene.name, mainMenuSceneName, System.StringComparison.OrdinalIgnoreCase);
        if (isMainMenu)
        {
            if (PlayerGO != null && PlayerGO.activeSelf)
                PlayerGO.SetActive(false);
            return;
        }

        // If Not Main Menu
        if (PlayerGO != null && !PlayerGO.activeSelf)
            PlayerGO.SetActive(true);

        MovePlayerToSpawnPoint();
        BindPlayerComponents();
        UpgradesManager.Instance?.ApplyUpgradedStats(PlayerController);
        PlayerController?.SetupPlayerForNewRun();
    }

    public void EnsurePlayerExists()
    {
        if (PlayerGO != null && PlayerController != null) return;

        var existing = FindFirstObjectByType<PlayerController>(FindObjectsInactive.Exclude);
        if (existing != null)
        {
            PlayerGO = existing.gameObject;
            PlayerController = existing;
            DontDestroyOnLoad(PlayerGO);
            return;
        }

        if (playerPrefab == null)
        {
            Debug.LogError("[PlayerManager] Player Prefab not assigned.");
            return;
        }

        PlayerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        PlayerController = PlayerGO.GetComponent<PlayerController>();
        if (PlayerController == null)
        {
            Debug.LogError("[PlayerManager] PlayerController missing on prefab.");
            return;
        }

        DontDestroyOnLoad(PlayerGO);
    }

    public void MovePlayerToSpawnPoint()
    {
        if (PlayerGO == null) return;

        var spawn = GameObject.FindWithTag("SpawnPoint") ?? GameObject.Find("SpawnPoint");
        if (spawn != null)
        {
            PlayerGO.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
        }
        else
        {
            Debug.LogWarning("[PlayerManager] No SpawnPoint found in this scene.");
        }
    }

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

        var gameManager = GameManager.Instance;
        var statsManager = StatsManager.Instance;
        var upgradesManager = UpgradesManager.Instance;

        if (gameManager == null) Debug.LogWarning("[PlayerManager] GameManager.Instance not found.");
        if (statsManager == null) Debug.LogWarning("[PlayerManager] StatsManager.Instance not found.");
        if (upgradesManager == null) Debug.LogWarning("[PlayerManager] UpgradesManager.Instance not found.");
    }

}