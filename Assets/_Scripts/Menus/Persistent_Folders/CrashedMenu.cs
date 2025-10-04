using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrashedMenu : MonoBehaviour
{
    public GameManager gameManager;
    public UpgradesManager upgradesManager;
    public PlayerController playerController;

    public Button playAgainButton;
    public Button backToMenuButton;
    public Button quitGameButton;


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playAgainButton.onClick.AddListener(OnPlayAgainButtonPressed);
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonPressed);
        quitGameButton.onClick.AddListener(OnQuitGameButtonPressed);
    }





    public void OnPlayAgainButtonPressed()
    {
        gameManager.RestartLevelScene();
    }

    public void OnBackToMenuButtonPressed()
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
