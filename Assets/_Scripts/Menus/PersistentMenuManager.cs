using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PersistentMenuManager : MonoBehaviour
{
    public static PersistentMenuManager Instance;

    [Header("Persistent Menus")]
    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public GameObject upgradesMenu;
    public GameObject upgradeStatsMenu;
    public GameObject upgradeAbilitiesMenu;
    public GameObject settingsMenu;
    public GameObject statisticsMenu;
    public GameObject hudMenu;
    public GameObject pauseMenu;
    public GameObject crashMenu;
    public GameObject winMenu;


    private Stack<GameObject> menuStack = new Stack<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CloseAllMenus();

        if (scene.name == "1.MainMenu")
        {
            OpenMenu(mainMenu);
            Debug.Log("Main Menu opened on startup.");
        }
        else
        {
            OpenMenu(hudMenu);
            Debug.Log("HUD opened for gameplay scene.");
        }
    }





    // ------------------------
    // Core Menu Stack Logic
    // ------------------------
    #region ----------------------------------------------------Menu Stack Logic
    public void OpenMenu(GameObject menu)
    {
        if (menu == null)
        {
            Debug.LogWarning("Tried to open a null menu.");
            return;
        }

        if (menuStack.Count > 0)
        {
            GameObject current = menuStack.Peek();
            current.SetActive(false);
        }

        menu.SetActive(true);
        menuStack.Push(menu);
    }

    public void Back()
    {
        if (menuStack.Count == 0) return;

        // Close the current menu
        GameObject current = menuStack.Pop();
        current.SetActive(false);

        // Show the previous menu if there is one
        if (menuStack.Count > 0)
        {
            GameObject previous = menuStack.Peek();
            previous.SetActive(true);
        }
        else
        {
            // Optional fallback (e.g., always return to HUD)
            if (hudMenu != null)
            {
                hudMenu.SetActive(true);
                Debug.Log("Back() ? HUD (fallback after stack empty)");
            }
        }
    }

    public void CloseAllMenus()
    {
        mainMenu?.SetActive(false);
        levelSelectMenu?.SetActive(false);
        settingsMenu?.SetActive(false);
        statisticsMenu?.SetActive(false);
        upgradesMenu?.SetActive(false);
        upgradeStatsMenu?.SetActive(false);
        upgradeAbilitiesMenu?.SetActive(false);
        pauseMenu?.SetActive(false);
        crashMenu?.SetActive(false);
        hudMenu?.SetActive(false);
        menuStack.Clear();
    }
    #endregion





    #region ------------------------------------------Opening And Closing All Menus

    // Main Menu and Level Selects
    public void OpenMainMenu()
    {
        OpenMenu(mainMenu);
    }
    public void ShowLevelSelectPanel()
    {
        if (levelSelectMenu != null)
        {
            levelSelectMenu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Level Select Panel is not assigned.");
        }
    }
    public void HideLevelSelectPanel()
    {
        if (levelSelectMenu != null)
        {
            levelSelectMenu.SetActive(false);
        }
    }



    // Menus
    public void OpenSettings()
    {
        OpenMenu(settingsMenu);
    }
    public void OpenStatistics()
    {
        OpenMenu(statisticsMenu);
    }
    public void OpenUpgrades()
    {
        OpenMenu(upgradesMenu);
    }
    public void OpenUpgradeStats()
    {
        OpenMenu(upgradeStatsMenu);
    }
    public void OpenUpgradeAbilities()
    {
        OpenMenu(upgradeAbilitiesMenu);
    }



    // In Game
    public void OpenHUD()
    {
        OpenMenu(hudMenu);
    }
    public void OpenPauseMenu()
    {
        OpenMenu(pauseMenu);
        Time.timeScale = 0.0f;
    }
    
    public void OpenCrashMenu()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.launched = false;
        }

        OpenMenu(crashMenu);
    }
    #endregion












}
