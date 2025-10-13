using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        EnsureSingleEventSystem();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Time.timeScale != 1f) Time.timeScale = 1f;
        EnsureSingleEventSystem();
    }

    private void EnsureSingleEventSystem()
    {
        // Use new API
        var systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        if (systems.Length <= 1) return;

        // keep the first enabled one; destroy the rest
        var keeper = systems.FirstOrDefault(es => es.isActiveAndEnabled) ?? systems[0];
        foreach (var es in systems)
        {
            if (es != keeper)
            {
                Debug.LogWarning($"[GameManager] Destroying duplicate EventSystem on {es.gameObject.name}");
                Destroy(es.gameObject);
            }
        }
    }




/// <summary>Restart the current level scene.</summary>
public void RestartLevelScene()
    {
        // Close menus if you want (optional)
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.CloseAllMenus();
        }

        // Reset time & reload active scene
        Time.timeScale = 1f;

        var active = SceneManager.GetActiveScene();
        if (active.IsValid())
            SceneManager.LoadScene(active.name, LoadSceneMode.Single);

        HUD.Instance.ResetPickupCredits();
        StatsManager.Instance.ResetPickupCredits();
    }





    

}