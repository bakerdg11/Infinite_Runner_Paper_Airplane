using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public Button resumeGame;
    public Button settings;
    public Button backToMain;
    public Button quitGame;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resumeGame.onClick.AddListener(OnResumeGameButtonPressed);
        settings.onClick.AddListener(OnSettingsButtonPressed);
        backToMain.onClick.AddListener(OnBackToMainButtonPressed);
        quitGame.onClick.AddListener(OnQuitGameButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnResumeGameButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
        Time.timeScale = 1.0f;
    }

    private void OnSettingsButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenSettings();
            Debug.Log("Settings Menu Open");
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }

    private void OnBackToMainButtonPressed()
    {
        // Make sure time resumes normally when switching scenes
        Time.timeScale = 1f;

        // Load your main menu scene
        SceneManager.LoadScene("1.MainMenu");
    }

    private void OnQuitGameButtonPressed()
    {
        Application.Quit();
        Debug.Log("Quit game button pressed");
    }








}
