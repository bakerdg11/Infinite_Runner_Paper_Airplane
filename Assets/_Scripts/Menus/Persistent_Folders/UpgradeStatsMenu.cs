using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UpgradeStatsMenu : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public GameManager gameManager;
    public StatsManager statsManager;
    public UpgradesManager upgradesManager;

    [Header("Buttons")]
    public Button upgradeEnergyDepletionRateButton;
    public Button upgradeLaneChangeSpeedButton;
    public Button upgradesBackButton;

    [Header("Texts")]
    public TMP_Text statsMenuCredits;
    public TMP_Text edrCurrentLevelText;
    public TMP_Text edrMaxLevelText;
    public TMP_Text lcsCurrentLevelText;
    public TMP_Text lcsMaxLevelText;




    void Start()
    {
        upgradeEnergyDepletionRateButton.onClick.AddListener(OnUpgradeEnergyDepletionRateButtonPressed);
        upgradeLaneChangeSpeedButton.onClick.AddListener(OnUpgradeLaneChangeSpeedButtonPressed);
        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnEnable()
    {
        UpdateStatsUpgradesMenuNumbers();
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }
    


    private void UpdateStatsUpgradesMenuNumbers()
    {
        if (statsMenuCredits != null)
            statsMenuCredits.text = "Credits: " + statsManager.totalCredits;

        if (edrCurrentLevelText != null)
            edrCurrentLevelText.text = upgradesManager.edrCurrentLevel.ToString();

        if (edrMaxLevelText != null)
            edrMaxLevelText.text = upgradesManager.edrMaxLevel.ToString();

        if (lcsCurrentLevelText != null)
            lcsCurrentLevelText.text = upgradesManager.lcsCurrentLevel.ToString();

        if (lcsMaxLevelText != null)
            lcsMaxLevelText.text = upgradesManager.lcsMaxLevel.ToString();
    }


    private void OnUpgradeEnergyDepletionRateButtonPressed()
    {
        upgradesManager.UpgradeEnergyDepletionRate();
        UpdateStatsUpgradesMenuNumbers();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    private void OnUpgradeLaneChangeSpeedButtonPressed()
    {
        upgradesManager.UpgradeLaneChangeSpeed();
        UpdateStatsUpgradesMenuNumbers();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }




}