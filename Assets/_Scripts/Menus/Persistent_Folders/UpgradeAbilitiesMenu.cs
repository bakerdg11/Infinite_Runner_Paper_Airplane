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

    [Header("Texts")]
    public TMP_Text abilityMenuPoints;
    [Header("Pause Energy Depletion")]
    public TMP_Text pedLengthCurrentLevelText;
    public TMP_Text pedLengthMaxLevelText;
    public TMP_Text pedAmmoCurrentLevelText;
    public TMP_Text pedAmmoMaxLevelText;
    [Header("Boost")]
    public TMP_Text boostLengthCurrentLevelText;
    public TMP_Text boostLengthMaxLevelText;
    public TMP_Text boostAmmoCurrentLevelText;
    public TMP_Text boostAmmoMaxLevelText;
    [Header("Invincibility")]
    public TMP_Text invincibilityLengthCurrentLevelText;
    public TMP_Text invincibilityLengthMaxLevelText;
    public TMP_Text invincibilityAmmoCurrentLevelText;
    public TMP_Text invincibilityAmmoMaxLevelText;
    [Header("Dash")]
    public TMP_Text dashAmmoCurrentLevelText;
    public TMP_Text dashAmmoMaxLevelText;
    [Header("Missile")]
    public TMP_Text missileAmmoCurrentLevelText;
    public TMP_Text missileAmmoMaxLevelText;



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

    private void OnEnable()
    {
        UpdateAbilitiesUpgradesMenuStats();
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }


    public void UpdateAbilitiesUpgradesMenuStats()
    {
        // Ability Points currency
        if (abilityMenuPoints != null)
            abilityMenuPoints.text = "Ability Points: " + statsManager.totalAbilityPoints;
        // Pause Energy Depletion
        if (pedLengthCurrentLevelText != null)
            pedLengthCurrentLevelText.text = upgradesManager?.pedLengthCurrentLevel.ToString();
        if (pedLengthMaxLevelText != null)
            pedLengthMaxLevelText.text = upgradesManager?.pedLengthMaxLevel.ToString();
        if (pedAmmoCurrentLevelText != null)
            pedAmmoCurrentLevelText.text = upgradesManager?.pedAmmoCurrentLevel.ToString();
        if (pedAmmoMaxLevelText != null)
            pedAmmoMaxLevelText.text = upgradesManager?.pedAmmoMaxLevel.ToString();
        // Boost
        if (boostLengthCurrentLevelText != null)
            boostLengthCurrentLevelText.text = upgradesManager?.boostLengthCurrentLevel.ToString();
        if (boostLengthMaxLevelText != null)
            boostLengthMaxLevelText.text = upgradesManager?.boostLengthMaxLevel.ToString();
        if (boostAmmoCurrentLevelText != null)
            boostAmmoCurrentLevelText.text = upgradesManager?.boostAmmoCurrentLevel.ToString();
        if (boostAmmoMaxLevelText!= null)
            boostAmmoMaxLevelText.text = upgradesManager?.boostAmmoMaxLevel.ToString();
        // Invincibility
        if (invincibilityLengthCurrentLevelText!= null)
            invincibilityLengthCurrentLevelText.text = upgradesManager?.invincibilityLengthCurrentLevel.ToString();
        if (invincibilityLengthMaxLevelText!= null)
            invincibilityLengthMaxLevelText.text = upgradesManager?.invincibilityLengthMaxLevel.ToString();
        if (invincibilityAmmoCurrentLevelText!= null)
            invincibilityAmmoCurrentLevelText.text = upgradesManager?.invincibilityAmmoCurrentLevel.ToString();
        if (invincibilityAmmoMaxLevelText != null)
            invincibilityAmmoMaxLevelText.text = upgradesManager?.invincibilityAmmoMaxLevel.ToString();
        // Dash
        if (dashAmmoCurrentLevelText != null)
            dashAmmoCurrentLevelText.text = upgradesManager?.dashAmmoCurrentLevel.ToString();
        if (dashAmmoMaxLevelText != null)
            dashAmmoMaxLevelText.text = upgradesManager?.dashAmmoMaxLevel.ToString();
        // Missiles
        if (missileAmmoCurrentLevelText != null)
            missileAmmoCurrentLevelText.text = upgradesManager?.missileAmmoCurrentLevel.ToString();
        if (missileAmmoMaxLevelText != null)
            missileAmmoMaxLevelText.text = upgradesManager?.missileAmmoMaxLevel.ToString();
    }




    // ------------------------------------------------------------------------PED Ability ----------------
    private void OnUpgradePauseEnergyDepletionLengthButtonPressed()
    {
        upgradesManager.UpgradePauseEnergyDepletionLength();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    private void OnUpgradePauseEnergyDepletionAmmoButtonPressed()
    {
        upgradesManager.UpgradePauseEnergyDepletionAmmo();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    // ------------------------------------------------------------------------Boost Ability
    private void OnUpgradeBoostLengthButtonPressed()
    {
        upgradesManager.UpgradeBoostLength();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    private void OnUpgradeBoostAmmoButtonPressed()
    {
        upgradesManager.UpgradeBoostAmmo();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    // ------------------------------------------------------------------------Invincibility Ability
    private void OnUpgradeInvincibilityLengthButtonPressed()
    {
        upgradesManager.UpgradeInvincibilityLength();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    private void OnUpgradeInvincibilityAmmoButtonPressed()
    {
        upgradesManager.UpgradeInvincibilityAmmo();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    // -------------------------------------------------------------------------Dash Ability
    private void OnUpgradeDashAmmoButtonPressed()
    {
        upgradesManager.UpgradeDashAmmo();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }

    // -------------------------------------------------------------------------Missile Depletion
    private void OnUpgradeMissileAmmoButtonPressed()
    {
        upgradesManager.UpgradeMissileAmmo();
        UpdateAbilitiesUpgradesMenuStats();
        SoundManager.Instance.PlayUpgradeSound();
        SaveManager.Instance?.SaveGame();
    }



}