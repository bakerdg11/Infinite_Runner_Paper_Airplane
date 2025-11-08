using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button selectLevelButton;
    public Button upgradesButton;
    public Button statisticsButton;
    public Button settingsButton;
    public Button quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectLevelButton.onClick.AddListener(OnSelectLevelButtonPressed);
        settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        statisticsButton.onClick.AddListener(OnStatisticsButtonPressed);
        upgradesButton.onClick.AddListener(OnUpgradesButtonPressed);
        quitButton.onClick.AddListener(QuitGame);
    }



    private void OnSelectLevelButtonPressed()
    {
        PersistentMenuManager.Instance.ShowLevelSelectPanel();
    }

    private void OnUpgradesButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenUpgrades();
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }

    private void OnStatisticsButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenStatistics();
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }

    private void OnSettingsButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenSettings();
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }



    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game button pressed");
    }


}
