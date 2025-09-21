using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Button settingsBackButton;

    void Start()
    {
        settingsBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }
}