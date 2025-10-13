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
    public TMP_Text edrCurrentLevel;
    public TMP_Text edrMaxLevel;
    public TMP_Text lcsCurrentLevel;
    public TMP_Text lcsMaxLevel;


    void Start()
    {
        upgradeEnergyDepletionRateButton.onClick.AddListener(OnUpgradeEnergyDepletionRateButtonPressed);
        upgradeLaneChangeSpeedButton.onClick.AddListener(OnUpgradeLaneChangeSpeedButtonPressed);
        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }



    private void OnUpgradeEnergyDepletionRateButtonPressed()
    {
        //upgradesManager.UpgradeEnergyDepletionRate();
    }

    private void OnUpgradeLaneChangeSpeedButtonPressed()
    {
        //upgradesManager.UpgradeLaneChangeSpeed();
    }




}