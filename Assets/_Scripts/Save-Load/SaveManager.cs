using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public GameData CurrentData { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Always load (or create a new GameData if no file exists)
        CurrentData = SaveSystem.Load();

        // Try to push into managers that already exist
        ApplyLoadedDataToManagers();
    }

    /// <summary>
    /// Called by managers (or manually) when they spawn after SaveManager.
    /// </summary>
    public void ApplyLoadedDataToManagers()
    {
        if (CurrentData == null)
            CurrentData = new GameData();

        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.LoadFromGameData(CurrentData);
        }

        if (UpgradesManager.Instance != null)
        {
            UpgradesManager.Instance.LoadFromGameData(CurrentData);
        }

        // Later:
        // SettingsManager.Instance?.LoadFromGameData(CurrentData);
    }

    public void SaveGame()
    {
        if (CurrentData == null)
            CurrentData = new GameData();

        // Ask managers to write their stuff into CurrentData
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.SaveToGameData(CurrentData);
        }

        if (UpgradesManager.Instance != null)
        {
            UpgradesManager.Instance.SaveToGameData(CurrentData);
        }

        SaveSystem.Save(CurrentData);
    }

    /// <summary>
    /// Completely resets progress:
    /// - deletes the save file on disk
    /// - creates a fresh GameData with default values
    /// - pushes those defaults into Stats/Upgrades
    /// - optionally reloads the current scene
    /// </summary>
    /// <param name="reloadScene">If true, reloads the active scene after reset.</param>
    public void ResetProgress(bool reloadScene = true)
    {
        // 1) Delete file on disk
        SaveSystem.DeleteSave();

        // 2) Reset in-memory data to brand new defaults
        CurrentData = new GameData();   // make sure GameData constructor has "fresh game" values

        // 3) Push defaults into managers
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.LoadFromGameData(CurrentData);
        }

        if (UpgradesManager.Instance != null)
        {
            UpgradesManager.Instance.LoadFromGameData(CurrentData);
        }

        Debug.Log("[SaveManager] Progress reset and save file deleted.");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}