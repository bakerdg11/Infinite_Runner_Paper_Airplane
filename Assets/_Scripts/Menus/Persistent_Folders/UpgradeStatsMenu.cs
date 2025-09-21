using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeStatsMenu : MonoBehaviour
{
    public UpgradesManager upgradesManager;

    public Button upgradeEnergyDepletionRateButton;
    public Button upgradeLaneChangeSpeedButton;
    public Button upgradesBackButton;

    void Start()
    {
        upgradeEnergyDepletionRateButton.onClick.AddListener(OnUpgradeEnergyDepletionRateButtonPressed);
        upgradeLaneChangeSpeedButton.onClick.AddListener(OnUpgradeLaneChangeSpeedButtonPressed);
        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnUpgradeEnergyDepletionRateButtonPressed()
    {
        //upgradesManager.UpgradeEnergyDepletionRate();
    }

    private void OnUpgradeLaneChangeSpeedButtonPressed()
    {
        //upgradesManager.UpgradeLaneChangeSpeed();
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }
}