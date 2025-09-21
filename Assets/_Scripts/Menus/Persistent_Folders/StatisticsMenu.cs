using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatisticsMenu : MonoBehaviour
{
    public Button statisticsBackButton;

    void Start()
    {
        statisticsBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }
}