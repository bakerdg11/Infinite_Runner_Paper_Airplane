using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string FileName = "savegame.json";

    private static string GetFullPath()
    {
        return Path.Combine(Application.persistentDataPath, FileName);
    }

    /// <summary>
    /// Returns true if a save file exists on disk.
    /// </summary>
    public static bool SaveFileExists()
    {
        return File.Exists(GetFullPath());
    }

    public static void Save(GameData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(GetFullPath(), json);
#if UNITY_EDITOR
            Debug.Log($"[SaveSystem] Saved:\n{json}");
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogError("[SaveSystem] Error saving: " + e);
        }
    }

    public static GameData Load()
    {
        string path = GetFullPath();
        if (!File.Exists(path))
        {
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] No save file, creating new GameData.");
#endif
            return new GameData(); // fresh defaults
        }

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<GameData>(json);
            return data ?? new GameData();
        }
        catch (System.Exception e)
        {
            Debug.LogError("[SaveSystem] Error loading: " + e);
            return new GameData();
        }
    }

    /// <summary>
    /// Deletes the save file from disk, if it exists.
    /// </summary>
    public static void DeleteSave()
    {
        string path = GetFullPath();

        if (File.Exists(path))
        {
            File.Delete(path);
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] Save file deleted.");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] No save file to delete.");
#endif
        }
    }
}