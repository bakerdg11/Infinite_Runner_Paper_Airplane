using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    public UpgradesManager upgradesManager;

    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backButton;



    // Start is called before the first frame update
    void Start()
    {
        level2Button.gameObject.SetActive(false);
        level3Button.gameObject.SetActive(false);

        level1Button.onClick.AddListener(BeginLevel1);
        backButton.onClick.AddListener(BackToMain);
    }


    private void BeginLevel1()
    {
        if (PersistentMenuManager.Instance != null)
        {
            BeginGame();
            //upgradesManager.GameStartAmmoAmounts();
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }


    private void BeginGame()
    {
        PersistentMenuManager.Instance.CloseAllMenus();
        SceneManager.LoadScene("2.Level1", LoadSceneMode.Additive);
    }



    private void BackToMain()
    {
        PersistentMenuManager.Instance.HideLevelSelectPanel();
    }


}

