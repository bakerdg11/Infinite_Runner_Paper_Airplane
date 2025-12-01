using UnityEngine;

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

    // Make this PUBLIC so managers can call it later if they spawn after us
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

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}


/* Tell SaveManager to save on any type of change. 
 * Example - buying ability points, upgrading stats etc

SaveManager.Instance?.SaveGame();

*/