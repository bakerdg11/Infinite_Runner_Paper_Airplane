using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Button resetStatsButton;
    public Button settingsBackButton;

    void Start()
    {
        resetStatsButton.onClick.AddListener(OnResetStatsButtonPressed);
        settingsBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }

    private void OnResetStatsButtonPressed()
    {
        SaveManager.Instance?.ResetProgress();
    }



}