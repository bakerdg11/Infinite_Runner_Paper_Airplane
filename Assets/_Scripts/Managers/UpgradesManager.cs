using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    [Header("References")]
    public PlayerController playerController;
    public GameManager gameManager;
    public StatsManager statsManager;

    [Header("Stats Upgrade Menu")]
    public int edrCurrentLevel;
    public int edrMaxLevel;
    public int lcsCurrentLevel;
    public int lcsMaxLevel;

    [Header("Abilities Upgrade Men")]
    // Pause Energy Depletion
    public int pedLengthCurrentLevel;
    public int pedLengthMaxLevel;
    public int pedAmmoCurrentLevel;
    public int pedAmmoMaxLevel;
    // Boost
    public int boostLengthCurrentLevel;
    public int boostLengthMaxLevel;
    public int boostAmmoCurrentLevel;
    public int boostAmmoMaxLevel;
    // Invincibility
    public int invincibilityLengthCurrentLevel;
    public int invincibilityLengthMaxLevel;
    public int invincibilityAmmoCurrentLevel;
    public int invincibilityAmmoMaxLevel;
    // Dash
    public int dashAmmoCurrentLevel;
    public int dashAmmoMaxLevel;
    // Missile
    public int missileAmmoCurrentLevel;
    public int missileAmmoMaxLevel;





    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerController = FindFirstObjectByType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogWarning("PaperplayerController not found in scene " + scene.name);
        }
    }



    // Upgrading Stats ---------------------------------------
    public void UpgradeEnergyDepletionRate()
    {
        if (statsManager.totalCredits >= 10)
        {
            if (edrCurrentLevel < edrMaxLevel)
            {
                playerController.energyDepletionRate -= 0.01f;
                statsManager.totalCredits -= 10;
                edrCurrentLevel += 1;
            }
        }
    }

    public void UpgradeLaneChangeSpeed()
    {
        if (statsManager.totalCredits >= 10)
        {
            if (lcsCurrentLevel < lcsMaxLevel)
            {
                playerController.lateralMoveSpeed += 0.5f;
                statsManager.totalCredits -= 10;
                lcsCurrentLevel += 1;
            }
        }
    }



    // Upgrading Abilities ----------------------------------------
    public void UpgradePauseEnergyDepletionLength()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (pedLengthCurrentLevel < pedLengthMaxLevel)
            {
                //energyPauseDuration += 1;
                statsManager.totalAbilityPoints -= 1;
                pedLengthCurrentLevel += 1;
            }
        }
    }

    public void UpgradePauseEnergyDepletionAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (pedAmmoCurrentLevel < pedAmmoMaxLevel)
            {
                //pauseEnergyAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                pedAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeBoostLength()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (boostLengthCurrentLevel < boostLengthMaxLevel)
            {
                //boostDuration += 0.2f;
                statsManager.totalAbilityPoints -= 1;
                boostLengthCurrentLevel += 1;
            }
        }
    }

    public void UpgradeBoostAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (boostAmmoCurrentLevel < boostAmmoMaxLevel)
            {
                //boostAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                boostAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeInvincibilityLength()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (invincibilityLengthCurrentLevel < invincibilityLengthMaxLevel)
            {
                //invincibleDuration += 0.2f;
                statsManager.totalAbilityPoints -= 1;
                invincibilityLengthCurrentLevel += 1;
            }
        }
    }

    public void UpgradeInvincibilityAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (invincibilityAmmoCurrentLevel < invincibilityAmmoMaxLevel)
            {
                //invincibilityAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                invincibilityAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeDashAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (dashAmmoCurrentLevel < dashAmmoMaxLevel)
            {
                //dashAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                dashAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeMissileAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (missileAmmoCurrentLevel < missileAmmoMaxLevel)
            {
                //missileAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                missileAmmoCurrentLevel += 1;
            }
        }
    }






}
