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
        upgradesManager.GameStartAmmoAmounts();
    }

    public void OnBackToMenuButtonPressed()
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

        playerController.ResetPlayerForLevel();
    }


    private void OnQuitGameButtonPressed()
    {
        Application.Quit();
        Debug.Log("Quit game button pressed");
    }






    private void BeginGame()
    {
        gameManager.RestartLevelScene();
        upgradesManager.GameStartAmmoAmounts();
        PersistentMenuManager.Instance.CloseAllMenus();
        SceneManager.LoadScene("2.Level1", LoadSceneMode.Additive);
    }


}
