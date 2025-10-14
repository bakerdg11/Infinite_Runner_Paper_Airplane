using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradesMenu : MonoBehaviour
{
    public GameManager gameManager;
    public StatsManager statsManager;
    public UpgradesManager upgradesManager;

    public Button buyAbilityPointButton;
    public Button upgradeStatsButton;
    public Button upgradeAbilitiesButton;
    public Button upgradesBackButton;

    public TMP_Text upgradeMenuCredits;
    public TMP_Text upgradeMenuAbilityPoints;

    public 

    void Start()
    {
        buyAbilityPointButton.onClick.AddListener(OnBuyAbilityPointButtonPressed);
        upgradeStatsButton.onClick.AddListener(OnUpgradeStatsButtonPressed);
        upgradeAbilitiesButton.onClick.AddListener(OnUpgradeAbilitiesButtonPressed);
        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }


    private void OnEnable()
    {
        UpdateUpgradesMenuNumbers();
    }


    private void OnUpgradeStatsButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenUpgradeStats();
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }

    private void OnUpgradeAbilitiesButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenUpgradeAbilities();
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }

    private void OnBackButtonPressed()
    {
        PersistentMenuManager.Instance.Back();
    }


    private void UpdateUpgradesMenuNumbers()
    {
        if (upgradeMenuCredits != null)
            upgradeMenuCredits.text = "Credits: " + statsManager.totalCredits;

        if (upgradeMenuAbilityPoints != null)
            upgradeMenuAbilityPoints.text = "Ability Points: " + statsManager.totalAbilityPoints;

    }

    private void OnBuyAbilityPointButtonPressed()
    {
        statsManager.BuyAbilityPoints();
        UpdateUpgradesMenuNumbers();
    }



}