using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{



    /*

    public static PlayerManager Instance;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;

    public PlayerController playerController { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsurePlayerExists();
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= HandleSceneLoaded;
    }



    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerController == null)
        {
            // In case something destroyed it, recreate.
            EnsurePlayerExists();
            if (playerController == null) return;
        }

        // Menu scene: keep player hidden
        if (scene.name == "1.MainMenu")
        {
            if (playerController)
            {
                var rb = playerController.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                playerController.gameObject.SetActive(false);
            }
            return;
        }

        // Gameplay scene: move, reset-per-level, then show
        MovePlayerToSpawnPoint();
        //playerController.ResetForLevel();
        playerController.gameObject.SetActive(true);


    }


    private void EnsurePlayerExists()
    {
        if (playerController != null) return;

        if (playerPrefab == null)
        {
            Debug.LogError("[PlayerManager] Player prefab not assigned on the manager.");
            return;
        }

        var go = Instantiate(playerPrefab);
        playerController = go.GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogWarning("[PlayerManager] Player prefab is missing a PlayerController component.");

        // Start hidden in main menu; shown in gameplay scenes
        playerController.gameObject.SetActive(false);
    }



    public void BeginNewRun()
    {
        EnsurePlayerExists();

        if (playerController != null)
        {
            playerController.ResetPlayerForLevel();
        }

    }


    public void MovePlayerToSpawnPoint()
    {
        if (playerController == null) return;

        var spawn = Object.FindFirstObjectByType<PlayerSpawnPoint>();
        if (!spawn)
        {
            Debug.LogError("[PlayerManager] No PlayerSpawnPoint found in scene.");
            return;
        }

        var t = playerController.transform;
        t.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);

        var rb = playerController.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;        // <- fixed
            rb.angularVelocity = Vector3.zero;
        }
    }


    public void ResetEnergyBar()
    {

    }
    */


}