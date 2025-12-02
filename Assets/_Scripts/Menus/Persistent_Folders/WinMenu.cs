using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text distanceTravelledText;
    [SerializeField] private TMP_Text distanceCreditsText;
    [SerializeField] private TMP_Text pickupCreditsText;
    [SerializeField] private TMP_Text totalCreditsText;

    public Button backToMenuButton;
    public Button quitGameButton;


    void Start()
    {
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonPressed);
        quitGameButton.onClick.AddListener(OnQuitGameButtonPressed);
    }

    void OnEnable()
    {
        UpdateWinMenuStats();
    }

    private void UpdateWinMenuStats()
    {
        if (distanceTravelledText) distanceTravelledText.text = $"Distance Travelled: {StatsManager.Instance.DistanceMeters}m";
        if (distanceCreditsText) distanceCreditsText.text = $"Travelled Credits: {StatsManager.Instance.DistanceTravelledCredits}";
        if (pickupCreditsText) pickupCreditsText.text = $"Credits Collected: {StatsManager.Instance.PickupCreditsThisRun}";
        if (totalCreditsText) totalCreditsText.text = $"Total Credits: {StatsManager.Instance.TotalCreditsAllTime}";
    }


    // ---------------------------------------------------------------------Buttons----------------------------------
    public void OnPlayAgainButtonPressed()
    {
        GameManager.Instance?.RestartLevelScene();
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