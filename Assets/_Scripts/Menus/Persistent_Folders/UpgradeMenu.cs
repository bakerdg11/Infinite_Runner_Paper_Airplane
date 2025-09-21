using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradesMenu : MonoBehaviour
{
    public GameManager gameManager;
    //public UpgradesManager upgradesManager;

    public Button buyAbilityPointButton;
    public Button upgradeStatsButton;
    public Button upgradeAbilitiesButton;
    public Button upgradesBackButton;

    void Start()
    {
        buyAbilityPointButton.onClick.AddListener(OnBuyAbilityPointButtonPressed);
        upgradeStatsButton.onClick.AddListener(OnUpgradeStatsButtonPressed);
        upgradeAbilitiesButton.onClick.AddListener(OnUpgradeAbilitiesButtonPressed);
        upgradesBackButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnBuyAbilityPointButtonPressed()
    {
        //gameManager.BuyAbilityPoint();
    }

    private void OnUpgradeStatsButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenUpgradeStats();
            Debug.Log("Upgrade Stats Menu Open");
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
            Debug.Log("Upgrade Abilities Menu Open");
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
}