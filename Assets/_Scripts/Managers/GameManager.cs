using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        // Safety: make sure time scale is normal whenever a scene loads
        if (Time.timeScale != 1f) Time.timeScale = 1f;
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
    }

    // Optional helpers if you want pause/resume here:
    public void PauseGame()
    {
        Time.timeScale = 0f;
        PersistentMenuManager.Instance?.OpenPauseMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        // Your menu manager will unpause when closing the pause menu
    }








    /*






    */
}