using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    public GameManager gameManager;
    public UpgradesManager upgradesManager;

    [Header("Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backButton;

    private void Start()
    {
        // Hide locked levels (null-safe)
        if (level2Button != null)
        {
            level2Button.gameObject.SetActive(false);
        }

        if (level3Button != null) 
        {
            level3Button.gameObject.SetActive(false);
        }

        if (level1Button != null) 
        {
            level1Button.onClick.AddListener(BeginLevel1);
        }

        if (backButton != null) 
        {
            backButton.onClick.AddListener(BackToMain);
        }
    }


    private void BeginLevel1()
    {
        // If you’ve added the PersistentPlayerManager to Bootstrap, start a fresh run
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.BeginNewRun();
        }
        else
        {
            Debug.LogWarning("PersistentPlayerManager not found. Player may not be created/moved.");
        }

        // Close all menus if it’s up
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.HideLevelSelectPanel();
        }


        // Load the gameplay scene (single load is fine—persistent systems survive)
        SceneManager.LoadScene("2.Level1", LoadSceneMode.Single);


    }


    private void BackToMain()
    {
        PersistentMenuManager.Instance.HideLevelSelectPanel();
    }


}

