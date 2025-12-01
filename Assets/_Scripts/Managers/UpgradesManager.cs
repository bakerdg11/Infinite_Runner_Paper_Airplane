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

    [Header("EDR - Energy Depletion Rate")]
    public int edrCurrentLevel;
    public int edrMaxLevel;
    [Range(0f, 1f)] public float edrReductionPerLevel = 0.10f; // 10% per level
    [Range(0.01f, 1f)] public float edrMinMultiplier = 0.10f; // never below 10% of base

    [Header("LCS - Lane Change Speed")]
    public int lcsCurrentLevel;
    public int lcsMaxLevel;
    public float lcsDeltaPerLevel = 0.5f;  // +0.6 units per level
    public float lcsMaxAbsolute = 8.0f;   // optional cap

    [Header("Abilities Duration Tuning (Seconds Per Level")]
    public float pedDurationBonusPerLevel = 0.5f;   // Max 5 seconds
    public float boostDurationBonusPerLevel = 0.5f; // Max 5 seconds
    public float invDurationBonusPerLevel = 0.5f; // Max 10 seconds

    public float pedMaxDurationMultiplier = 2.5f;     // up to 3x base duration
    public float boostMaxDurationMultiplier = 2.5f;
    public float invMaxDurationMultiplier = 2.5f;


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


    [Header("Ammo Tuning (per ability)")]
    public int pedAmmoPerLevel = 1;
    public int pedAmmoMaxAbsolute = 5;

    public int boostAmmoPerLevel = 1;
    public int boostAmmoMaxAbsolute = 3;

    public int invAmmoPerLevel = 1;
    public int invAmmoMaxAbsolute = 5;

    public int dashAmmoPerLevel = 1;
    public int dashAmmoMaxAbsolute = 5;

    public int missileAmmoPerLevel = 1;
    public int missileAmmoMaxAbsolute = 10;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SaveManager.Instance != null && SaveManager.Instance.CurrentData != null)
        {
            SaveManager.Instance.ApplyLoadedDataToManagers();
        }
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }


    public void SaveToGameData(GameData data)
    {
        data.edrCurrentLevel = edrCurrentLevel;
        data.edrMaxLevel = edrMaxLevel;
        data.lcsCurrentLevel = lcsCurrentLevel;
        data.lcsMaxLevel = lcsMaxLevel;

        data.pedLengthCurrentLevel = pedLengthCurrentLevel;
        data.pedLengthMaxLevel = pedLengthMaxLevel;
        data.pedAmmoCurrentLevel = pedAmmoCurrentLevel;
        data.pedAmmoMaxLevel = pedAmmoMaxLevel;

        data.boostLengthCurrentLevel = boostLengthCurrentLevel;
        data.boostLengthMaxLevel = boostLengthMaxLevel;
        data.boostAmmoCurrentLevel = boostAmmoCurrentLevel;
        data.boostAmmoMaxLevel = boostAmmoMaxLevel;

        data.invincibilityLengthCurrentLevel = invincibilityLengthCurrentLevel;
        data.invincibilityLengthMaxLevel = invincibilityLengthMaxLevel;
        data.invincibilityAmmoCurrentLevel = invincibilityAmmoCurrentLevel;
        data.invincibilityAmmoMaxLevel = invincibilityAmmoMaxLevel;

        data.dashAmmoCurrentLevel = dashAmmoCurrentLevel;
        data.dashAmmoMaxLevel = dashAmmoMaxLevel;

        data.missileAmmoCurrentLevel = missileAmmoCurrentLevel;
        data.missileAmmoMaxLevel = missileAmmoMaxLevel;
    }

    public void LoadFromGameData(GameData data)
    {
        edrCurrentLevel = data.edrCurrentLevel;
        edrMaxLevel = data.edrMaxLevel;
        lcsCurrentLevel = data.lcsCurrentLevel;
        lcsMaxLevel = data.lcsMaxLevel;

        pedLengthCurrentLevel = data.pedLengthCurrentLevel;
        pedLengthMaxLevel = data.pedLengthMaxLevel;
        pedAmmoCurrentLevel = data.pedAmmoCurrentLevel;
        pedAmmoMaxLevel = data.pedAmmoMaxLevel;

        boostLengthCurrentLevel = data.boostLengthCurrentLevel;
        boostLengthMaxLevel = data.boostLengthMaxLevel;
        boostAmmoCurrentLevel = data.boostAmmoCurrentLevel;
        boostAmmoMaxLevel = data.boostAmmoMaxLevel;

        invincibilityLengthCurrentLevel = data.invincibilityLengthCurrentLevel;
        invincibilityLengthMaxLevel = data.invincibilityLengthMaxLevel;
        invincibilityAmmoCurrentLevel = data.invincibilityAmmoCurrentLevel;
        invincibilityAmmoMaxLevel = data.invincibilityAmmoMaxLevel;

        dashAmmoCurrentLevel = data.dashAmmoCurrentLevel;
        dashAmmoMaxLevel = data.dashAmmoMaxLevel;

        missileAmmoCurrentLevel = data.missileAmmoCurrentLevel;
        missileAmmoMaxLevel = data.missileAmmoMaxLevel;

        // Then recalc any derived values:
        //ApplyUpgradedStats();
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
                StatsManager.Instance.totalAbilityPoints -= 1;
                missileAmmoCurrentLevel += 1;
            }
        }
    }


    // Applying Stats at beginning of new run - Energy Depletion Rate + Lane Change Speed
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



    public float GetUpgradedDuration(string abilityID, float baseDuration)
    {
        if (baseDuration <= 0f) return baseDuration;

        float extra = 0f;
        float maxMult = 1f;

        switch (abilityID)
        {
            case "pauseenergy":   // e.g. your pause energy depletion abilityId
                extra = pedLengthCurrentLevel * pedDurationBonusPerLevel;
                maxMult = pedMaxDurationMultiplier;
                break;

            case "boost":         // boost abilityId
                extra = boostLengthCurrentLevel * boostDurationBonusPerLevel;
                maxMult = boostMaxDurationMultiplier;
                break;

            case "invincible": // invincibility abilityId
                extra = invincibilityLengthCurrentLevel * invDurationBonusPerLevel;
                maxMult = invMaxDurationMultiplier;
                break;

            default:
                // abilities we don't upgrade yet
                return baseDuration;
        }

        float upgraded = baseDuration + extra;

        // Optional clamping: never exceed (baseDuration * maxMult)
        float maxAllowed = baseDuration * maxMult;
        upgraded = Mathf.Min(upgraded, maxAllowed);

        return upgraded;
    }


    public int GetStartingAmmo(string abilityId, int baseCharges)
    {
        int bonus = 0;
        int max = int.MaxValue; // default big number, unless we set a proper max

        switch (abilityId)
        {
            case "pauseenergy":   // ← use your actual abilityId string
                bonus = pedAmmoCurrentLevel * pedAmmoPerLevel;
                max = pedAmmoMaxAbsolute;
                break;

            case "boost":
                bonus = boostAmmoCurrentLevel * boostAmmoPerLevel;
                max = boostAmmoMaxAbsolute;
                break;

            case "invincible":
                bonus = invincibilityAmmoCurrentLevel * invAmmoPerLevel;
                max = invAmmoMaxAbsolute;
                break;

            case "dash":
                bonus = dashAmmoCurrentLevel * dashAmmoPerLevel;
                max = dashAmmoMaxAbsolute;
                break;

            case "missile":
                bonus = missileAmmoCurrentLevel * missileAmmoPerLevel;
                max = missileAmmoMaxAbsolute;
                break;

            default:
                // ability not handled (no ammo upgrade)
                return baseCharges;
        }

        int upgraded = baseCharges + bonus;
        if (max > 0) upgraded = Mathf.Min(upgraded, max);

        // never negative
        return Mathf.Max(0, upgraded);
    }







}
