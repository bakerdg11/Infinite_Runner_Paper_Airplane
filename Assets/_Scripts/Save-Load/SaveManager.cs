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

        // Load from disk
        //CurrentData = SaveSystem.Load(); -------------------------------------------------------- Remove this to make loading work

        // Push loaded data into managers that exist
        ApplyLoadedDataToManagers();
    }

    private void ApplyLoadedDataToManagers()
    {
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.LoadFromGameData(CurrentData);
        }

        // Later:
        // if (UpgradesManager.Instance != null)
        //     UpgradesManager.Instance.LoadFromGameData(CurrentData);
        // if (SettingsManager.Instance != null)
        //     SettingsManager.Instance.LoadFromGameData(CurrentData);
    }

    public void SaveGame()
    {
        // Ask managers to write their stuff into CurrentData
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.SaveToGameData(CurrentData);
        }

        // Later: add more managers here
        // UpgradesManager.Instance?.SaveToGameData(CurrentData);

        // Now write to disk
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