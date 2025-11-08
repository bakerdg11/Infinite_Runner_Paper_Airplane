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
    private PlayerController _player;
    public PlayerController PlayerController
    {
        get
        {
            if (_player == null)
                _player = PlayerManager.Instance?.PlayerController
                          ?? FindFirstObjectByType<PlayerController>();
            return _player;
        }
    }

    [Header("Stats Levels In Upgrade Menu")]
    public int edrCurrentLevel;
    public int edrMaxLevel;
    public int lcsCurrentLevel;
    public int lcsMaxLevel;

    [Header("Tuning")]
    [Range(0f, 1f)] public float edrReductionPerLevel = 0.10f; // 10% per level
    [Range(0.01f, 1f)] public float edrMinMultiplier = 0.10f; // never below 10% of base

    public float lcsDeltaPerLevel = 0.6f;  // +0.6 units per level
    public float lcsMaxAbsolute = 20f;   // optional cap

    [Header("Ability Levels In Upgrade Menu")]
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




    /*
    [Header("Upgarded Stats/Abilities For Player")]
    public float upgradedEnergyDepletionRate;
    public float upgradedLaneChangeSpeed;
    public float upgradedPauseEnergyDepletionLength;
    public float upgradedBoostLength;
    public float upgradedInvincibilityLength;

    [Header("Upgraded StatsAbilieis Ammo For HUD")]
    public int upgradedPauseEnergyDepletionAmmo;
    public int upgradedBoostAmmo;
    public int upgradedInvincibilityAmmo;
    public int upgradedDashAmmo;
    public int upgradedMissileAmmo;
    */

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

    }



    // Upgrading Stats ---------------------------------------
    public void UpgradeEnergyDepletionRate()
    {
        if (StatsManager.Instance == null) return;

        const int cost = 10;
        if (edrCurrentLevel >= edrMaxLevel) return;
        if (StatsManager.Instance.totalCredits < cost) return;

        StatsManager.Instance.totalCredits -= cost;
        edrCurrentLevel += 1;
    }

    public void UpgradeLaneChangeSpeed()
    {
        if (StatsManager.Instance == null) return;

        const int cost = 10;
        if (lcsCurrentLevel >= lcsMaxLevel) return;
        if (StatsManager.Instance.totalCredits < cost) return;

        StatsManager.Instance.totalCredits -= cost;
        lcsCurrentLevel += 1;
    }



    // Upgrading Abilities ----------------------------------------
    public void UpgradePauseEnergyDepletionLength()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (pedLengthCurrentLevel < pedLengthMaxLevel)
            {
                //energyPauseDuration += 1;
                StatsManager.Instance.totalAbilityPoints -= 1;
                pedLengthCurrentLevel += 1;
            }
        }
    }

    public void UpgradePauseEnergyDepletionAmmo()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (pedAmmoCurrentLevel < pedAmmoMaxLevel)
            {
                //pauseEnergyAmmo += 1;
                StatsManager.Instance.totalAbilityPoints -= 1;
                pedAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeBoostLength()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (boostLengthCurrentLevel < boostLengthMaxLevel)
            {
                //boostDuration += 0.2f;
                StatsManager.Instance.totalAbilityPoints -= 1;
                boostLengthCurrentLevel += 1;
            }
        }
    }

    public void UpgradeBoostAmmo()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (boostAmmoCurrentLevel < boostAmmoMaxLevel)
            {
                //boostAmmo += 1;
                StatsManager.Instance.totalAbilityPoints -= 1;
                boostAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeInvincibilityLength()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (invincibilityLengthCurrentLevel < invincibilityLengthMaxLevel)
            {
                //invincibleDuration += 0.2f;
                StatsManager.Instance.totalAbilityPoints -= 1;
                invincibilityLengthCurrentLevel += 1;
            }
        }
    }

    public void UpgradeInvincibilityAmmo()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (invincibilityAmmoCurrentLevel < invincibilityAmmoMaxLevel)
            {
                //invincibilityAmmo += 1;
                StatsManager.Instance.totalAbilityPoints -= 1;
                invincibilityAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeDashAmmo()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (dashAmmoCurrentLevel < dashAmmoMaxLevel)
            {
                //dashAmmo += 1;
                StatsManager.Instance.totalAbilityPoints -= 1;
                dashAmmoCurrentLevel += 1;
            }
        }
    }

    public void UpgradeMissileAmmo()
    {
        if (StatsManager.Instance?.totalAbilityPoints >= 1)
        {
            if (missileAmmoCurrentLevel < missileAmmoMaxLevel)
            {
                //missileAmmo += 1;
                StatsManager.Instance.totalAbilityPoints -= 1;
                missileAmmoCurrentLevel += 1;
            }
        }
    }



    public void ApplyUpgradedStats(PlayerController pc)
    {
        if (!pc) return;

        // ---- Energy Depletion Rate (multiplier pattern) ----
        // multiplier = 1 - (per-level reduction), clamped to floor
        float edrMult = 1f - (edrReductionPerLevel * edrCurrentLevel);
        edrMult = Mathf.Max(edrMinMultiplier, edrMult);

        pc.energyDepletionRate = pc.baselineEnergyDepletionRate * edrMult;

        // ---- Lane Change Speed (additive pattern) ----
        float lcs = pc.baselineLateralMoveSpeed + (lcsDeltaPerLevel * lcsCurrentLevel);
        if (lcsMaxAbsolute > 0f) lcs = Mathf.Min(lcs, lcsMaxAbsolute);

        pc.lateralMoveSpeed = lcs;
    }


    public void ApplyUpgradedAmmo()
    {
        // Pause Energy Depletion Ammo
        // Boost Ammo
        // Invincibility Ammo
        // Dash Ammo
        // Missile Ammo




    }







}
