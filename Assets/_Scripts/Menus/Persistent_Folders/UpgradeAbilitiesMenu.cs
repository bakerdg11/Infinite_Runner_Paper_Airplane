using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UpgradeAbilitiesMenu : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public GameManager gameManager;
    public StatsManager statsManager;
    public UpgradesManager upgradesManager;

    [Header("Texts")]
    public TMP_Text abilityMenuPoints;
    [Header("Pause Energy Depletion")]
    public TMP_Text pedLengthCurrentLevel;
    public TMP_Text pedLengthMaxLevel;
    public TMP_Text pedAmmoCurrentLevel;
    public TMP_Text pedAmmoMaxLevel;
    [Header("Boost")]
    public TMP_Text boostLengthCurrentLevel;
    public TMP_Text boostLengthMaxLevel;
    public TMP_Text boostAmmoCurrentLevel;
    public TMP_Text boostAmmoMaxLevel;
    [Header("Invincibility")]
    public TMP_Text invincibilityLengthCurrentLevel;
    public TMP_Text invincibilityLengthMaxLevel;
    public TMP_Text invincibilityAmmoCurrentLevel;
    public TMP_Text invincibilityAmmoMaxLevel;
    [Header("Dash")]
    public TMP_Text dashAmmoCurrentLevel;
    public TMP_Text dashAmmoMaxLevel;
    [Header("Missile")]
    public TMP_Text missileAmmoCurrentLevel;
    public TMP_Text missileAmmoMaxLevel;

    [Header("Buttons")]
    public Button upgradePedLengthButton;
    public Button upgradePedAmmoButton;
    public Button upgradeBoostLengthButton;
    public Button upgradeBoostAmmoButton;
    public Button upgradeInvincibilityLengthButton;
    public Button upgradeInvincibilityAmmoButton;
    public Button upgradeDashAmmoButton;
    public Button upgradeMissileAmmoButton;
    public Button upgradesBackButton;

    void Start()
    {
        upgradePedLengthButton.onClick.AddListener(OnUpgradePauseEnergyDepletionLengthButtonPressed);
        upgradePedAmmoButton.onClick.AddListener(OnUpgradePauseEnergyDepletionAmmoButtonPressed);
        upgradeBoostLengthButton.onClick.AddListener(OnUpgradeBoostLengthButtonPressed);
        upgradeBoostAmmoButton.onClick.AddListener(OnUpgradeBoostAmmoButtonPressed);
        upgradeInvincibilityLengthButton.onClick.AddListener(OnUpgradeInvincibilityLengthButtonPressed);
        upgradeInvincibilityAmmoButton.onClick.AddListener(OnUpgradeInvincibilityAmmoButtonPressed);
        upgradeDashAmmoButton.onClick.AddListener(OnUpgradeDashAmmoButtonPressed);
        upgradeMissileAmmoButton.onClick.AddListener(OnUpgradeMissileAmmoButtonPressed);

        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }


    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }


    public void UpdateAbilitiesUpgradesMenuStats()
    {

    }




    // ------------------------------------------------------------------------PED Ability ----------------
    private void OnUpgradePauseEnergyDepletionLengthButtonPressed()
    {
        //upgradesManager.UpgradePauseEnergyDepletionLength();
    }

    private void OnUpgradePauseEnergyDepletionAmmoButtonPressed()
    {
        //upgradesManager.UpgradePauseEnergyDepletionAmmo();
    }

    // ------------------------------------------------------------------------Boost Ability
    private void OnUpgradeBoostLengthButtonPressed()
    {
        //upgradesManager.UpgradeBoostLength();
    }

    private void OnUpgradeBoostAmmoButtonPressed()
    {
        //upgradesManager.UpgradeBoostAmmo();
    }

    // ------------------------------------------------------------------------Invincibility Ability
    private void OnUpgradeInvincibilityLengthButtonPressed()
    {
        //upgradesManager.UpgradeInvincibilityLength();
    }

    private void OnUpgradeInvincibilityAmmoButtonPressed()
    {
        //upgradesManager.UpgradeInvincibilityAmmo();
    }

    // -------------------------------------------------------------------------Dash Ability
    private void OnUpgradeDashAmmoButtonPressed()
    {
        //upgradesManager.UpgradeDashAmmo();
    }

    // -------------------------------------------------------------------------Missile Depletion
    private void OnUpgradeMissileAmmoButtonPressed()
    {
        //upgradesManager.UpgradeMissileAmmo();
    }



}