using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UpgradeAbilitiesMenu : MonoBehaviour
{
    public UpgradesManager upgradesManager;

    public Button upgradePEDLButton;
    public Button upgradePEDAButton;
    public Button upgradeBoostLengthButton;
    public Button upgradeBoostAmmoButton;
    public Button upgradeInvincibilityLengthButton;
    public Button upgradeInvincibilityAmmoButton;
    public Button upgradeDashAmmoButton;
    public Button upgradeMissileAmmoButton;

    public Button upgradesBackButton;

    void Start()
    {
        upgradePEDLButton.onClick.AddListener(OnUpgradePauseEnergyDepletionLengthButtonPressed);
        upgradePEDAButton.onClick.AddListener(OnUpgradePauseEnergyDepletionAmmoButtonPressed);
        upgradeBoostLengthButton.onClick.AddListener(OnUpgradeBoostLengthButtonPressed);
        upgradeBoostAmmoButton.onClick.AddListener(OnUpgradeBoostAmmoButtonPressed);
        upgradeInvincibilityLengthButton.onClick.AddListener(OnUpgradeInvincibilityLengthButtonPressed);
        upgradeInvincibilityAmmoButton.onClick.AddListener(OnUpgradeInvincibilityAmmoButtonPressed);
        upgradeDashAmmoButton.onClick.AddListener(OnUpgradeDashAmmoButtonPressed);
        upgradeMissileAmmoButton.onClick.AddListener(OnUpgradeMissileAmmoButtonPressed);

        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }


    // PED Ability ----------------
    private void OnUpgradePauseEnergyDepletionLengthButtonPressed()
    {
        //upgradesManager.UpgradePauseEnergyDepletionLength();
    }

    private void OnUpgradePauseEnergyDepletionAmmoButtonPressed()
    {
        //upgradesManager.UpgradePauseEnergyDepletionAmmo();
    }

    // Boost Ability
    private void OnUpgradeBoostLengthButtonPressed()
    {
        //upgradesManager.UpgradeBoostLength();
    }

    private void OnUpgradeBoostAmmoButtonPressed()
    {
        //upgradesManager.UpgradeBoostAmmo();
    }

    // Invincibility Ability
    private void OnUpgradeInvincibilityLengthButtonPressed()
    {
        //upgradesManager.UpgradeInvincibilityLength();
    }

    private void OnUpgradeInvincibilityAmmoButtonPressed()
    {
        //upgradesManager.UpgradeInvincibilityAmmo();
    }

    // Dash Ability
    private void OnUpgradeDashAmmoButtonPressed()
    {
        //upgradesManager.UpgradeDashAmmo();
    }

    // Missile Depletion
    private void OnUpgradeMissileAmmoButtonPressed()
    {
        //upgradesManager.UpgradeMissileAmmo();
    }


    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }
}