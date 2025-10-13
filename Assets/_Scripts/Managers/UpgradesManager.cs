using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    public PlayerController playerController;
    public GameManager gameManager;
    public StatsManager statsManager;


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













}

    /*

    // ------------------------------------ABILITIES ---------------------------------------
    [Header("Pause Energy Depletion Variables")]
    public float energyPauseDuration = 2.0f;
    public bool energyDepletionPaused = false;
    private Coroutine energyPauseCoroutine;
    public Slider energyPauseSlider;
    [Header("Boost Variables")]
    public float boostDuration = 2.0f;
    public bool boostEnabled = false;
    private Coroutine boostCoroutine;
    public Slider boostSlider;
    [Header("Invincible Variables")]
    public float invincibleDuration = 2.0f;
    public bool invincibleEnabled = false;
    private Coroutine invincibleCoroutine;
    public Slider invincibleSlider;
    [Header("Dash Variables")]
    public float dashDuration = 0.3f;
    public bool dashEnabled = false;
    private Coroutine dashCoroutine;
    public Slider dashSlider;
    [Header("Missile Variables")]
    public float missileCooldown = 0.5f;
    public bool missileFired = false;
    private Coroutine missileCooldownCoroutine;
    public Slider missileSliderLeft;
    public Slider missileSliderRight;


    [Header("In Game Abilities")]
    public int pauseEnergyAmmo = 1;
    public int boostAmmo = 0;
    public int invincibilityAmmo = 0;
    public int dashAmmo = 1;
    public int missileAmmo = 0;

    public int tempPauseEnergyAmmo;
    public int tempBoostAmmo;
    public int tempInvincibilityAmmo;
    public int tempDashAmmo;
    public int tempMissileAmmo;


    [Header("HUD Abilities Ammo Numbers")]
    public TMP_Text distanceTravelledText;
    public TMP_Text pickupCreditsText;
    public TMP_Text pauseEnergyAmmoText;
    public TMP_Text boostAmmoText;
    public TMP_Text invincibilityAmmaText;
    public TMP_Text dashAmmoText;
    public TMP_Text leftMissileAmmoText;
    public TMP_Text rightMissileAmmoText;


    [Header("Ability & Stats Upgrads Menu Text")]
    public TMP_Text upgradesMenuTotalCreditsText;
    public TMP_Text upgradesMenuTotalAbilityPointsText;
    public TMP_Text upgradesStatsMenuTotalCreditsText;
    public TMP_Text upgradesAbilitiesMenuTotalAbilityPointsText;


    [Header("Abilities Upgrade Page Current / Max")]
    public TMP_Text edrCurrent;
    public TMP_Text edrMax;
    public TMP_Text lcsCurrent;
    public TMP_Text lcsMax;

    public TMP_Text pedLengthCurrent;
    public TMP_Text pedLengthMax;
    public TMP_Text pedAmmoCurrent;
    public TMP_Text pedAmmoMax;

    public TMP_Text boostLengthCurrent;
    public TMP_Text boostLengthMax;
    public TMP_Text boostAmmoCurrent;
    public TMP_Text boostAmmoMax;

    public TMP_Text invincibilityLengthCurrent;
    public TMP_Text invincibilityLengthMax;
    public TMP_Text invincibilityAmmoCurrent;
    public TMP_Text invincibilityAmmoMax;

    public TMP_Text dashAmmoCurrent;
    public TMP_Text dashAmmoMax;

    public TMP_Text missileAmmoCurrent;
    public TMP_Text missileAmmoMax;

    public int edrCurrentLevel;
    public int edrMaxLevel;
    public int lcsCurrentLevel;
    public int lcsMaxLevel;

    public int pedLengthCurrentLevel;
    public int pedLengthMaxLevel;
    public int pedAmmoCurrentLevel;
    public int pedAmmoMaxLevel;

    public int boostLengthCurrentLevel;
    public int boostLengthMaxLevel;
    public int boostAmmoCurrentLevel;
    public int boostAmmoMaxLevel;

    public int invincibilityLengthCurrentLevel;
    public int invincibilityLengthMaxLevel;
    public int invincibilityAmmoCurrentLevel;
    public int invincibilityAmmoMaxLevel;

    public int dashAmmoCurrentLevel;
    public int dashAmmoMaxLevel;

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
       



        switch (scene.name)
        {
            case "1.MainMenu":
                edrCurrentLevel = 0;
                edrMaxLevel = 16;
                lcsCurrentLevel = 0;
                lcsMaxLevel = 4;

                pedLengthCurrentLevel = 0;
                pedLengthMaxLevel = 6;
                pedAmmoCurrentLevel = 0;
                pedAmmoMaxLevel = 3;

                boostLengthCurrentLevel = 0;
                boostLengthMaxLevel = 6;
                boostAmmoCurrentLevel = 0;
                boostAmmoMaxLevel = 2;

                invincibilityLengthCurrentLevel = 0;
                invincibilityLengthMaxLevel = 6;
                invincibilityAmmoCurrentLevel = 0;
                invincibilityAmmoMaxLevel = 2;

                dashAmmoCurrentLevel = 1;
                dashAmmoMaxLevel = 3;

                missileAmmoCurrentLevel = 0;
                missileAmmoMaxLevel = 5;
                break;

            case "2.Level1":
                Debug.Log("Scene 2 loaded (Level 1): " + scene.name);
                edrCurrentLevel = 0;
                edrMaxLevel = 16;
                lcsCurrentLevel = 0;
                lcsMaxLevel = 4;

                pedLengthCurrentLevel = 0;
                pedLengthMaxLevel = 6;
                pedAmmoCurrentLevel = 0;
                pedAmmoMaxLevel = 3;

                boostLengthCurrentLevel = 0;
                boostLengthMaxLevel = 6;
                boostAmmoCurrentLevel = 0;
                boostAmmoMaxLevel = 2;

                invincibilityLengthCurrentLevel = 0;
                invincibilityLengthMaxLevel = 6;
                invincibilityAmmoCurrentLevel = 0;
                invincibilityAmmoMaxLevel = 2;

                dashAmmoCurrentLevel = 1;
                dashAmmoMaxLevel = 3;

                missileAmmoCurrentLevel = 0;
                missileAmmoMaxLevel = 5;
                break;

            default:
                Debug.Log("No ability config for scene: " + scene.name);
                break;

        }


        UpdateAmmoUI();
        UpdateAbilityLevelsText();
    }








    public void UpdateAmmoUI() // Call this any time you use an ability. 
    {
        if (pauseEnergyAmmoText != null)
            pauseEnergyAmmoText.text = pauseEnergyAmmo.ToString();

        if (boostAmmoText != null)
            boostAmmoText.text = boostAmmo.ToString();

        if (invincibilityAmmaText != null)
            invincibilityAmmaText.text = invincibilityAmmo.ToString();

        if (dashAmmoText != null)
            dashAmmoText.text = dashAmmo.ToString();

        if (leftMissileAmmoText != null)
            leftMissileAmmoText.text = missileAmmo.ToString();

        if (rightMissileAmmoText != null)
            rightMissileAmmoText.text = missileAmmo.ToString();
    }




    // ABILITY ---------- ENERGY DEPLETION PAUSED
    public void EnergyDepletionPaused()
    {
        // If already running a pause, restart the timer
        if (energyPauseCoroutine != null)
            StopCoroutine(energyPauseCoroutine);

        energyPauseCoroutine = StartCoroutine(PauseEnergyDepletionForSeconds(energyPauseDuration));
    }

    private IEnumerator PauseEnergyDepletionForSeconds(float duration)
    {
        energyDepletionPaused = true;

        // Enable slider and reset value
        if (energyPauseSlider != null)
        {
            energyPauseSlider.gameObject.SetActive(true);
            energyPauseSlider.value = 1f;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (energyPauseSlider != null)
                energyPauseSlider.value = Mathf.Lerp(1f, 0f, elapsed / duration);

            yield return null;
        }

        // Hide slider and reset
        if (energyPauseSlider != null)
        {
            energyPauseSlider.value = 0f;
            energyPauseSlider.gameObject.SetActive(false);
        }

        energyDepletionPaused = false;
        energyPauseCoroutine = null;
    }




    // ABILITY ---------- BOOST
    public void Boost()
    {
        // If already running a pause, restart the timer
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        boostCoroutine = StartCoroutine(BoostForSeconds(boostDuration));
    }

    private IEnumerator BoostForSeconds(float duration)
    {
        boostEnabled = true;
        playerController.AirplaneBoost();

        // Enable slider and reset value
        if (boostSlider != null)
        {
            boostSlider.gameObject.SetActive(true);
            boostSlider.value = 1f;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (boostSlider != null)
                boostSlider.value = Mathf.Lerp(1f, 0f, elapsed / duration);

            yield return null;
        }

        // Hide slider and reset
        if (boostSlider != null)
        {
            boostSlider.value = 0f;
            boostSlider.gameObject.SetActive(false);
        }

        playerController.AirplaneBoostEnd();
        boostEnabled = false;
        boostCoroutine = null;
    }



    // ABILITY ---------- INVINCIBLE
    public void Invincible()
    {
        // If already running a pause, restart the timer
        if (invincibleCoroutine != null)
            StopCoroutine(invincibleCoroutine);

        invincibleCoroutine = StartCoroutine(InvincibleForSeconds(invincibleDuration));
    }
    private IEnumerator InvincibleForSeconds(float duration)
    {
        invincibleEnabled = true;

        // Enable slider and reset value
        if (invincibleSlider != null)
        {
            invincibleSlider.gameObject.SetActive(true);
            invincibleSlider.value = 1f;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (invincibleSlider != null)
                invincibleSlider.value = Mathf.Lerp(1f, 0f, elapsed / duration);

            yield return null;
        }

        // Hide slider and reset
        if (invincibleSlider != null)
        {
            invincibleSlider.value = 0f;
            invincibleSlider.gameObject.SetActive(false);
        }

        invincibleEnabled = false;
        invincibleCoroutine = null;
    }



    // ABILITY ---------- DASH
    public void Dash()
    {
        // If already running a pause, restart the timer
        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);

        dashCoroutine = StartCoroutine(DashForSeconds(dashDuration));
    }
    private IEnumerator DashForSeconds(float duration)
    {
        dashEnabled = true;
        playerController.AirplaneDash();

        // Enable slider and reset value
        if (dashSlider != null)
        {
            dashSlider.gameObject.SetActive(true);
            dashSlider.value = 1f;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (dashSlider != null)
                dashSlider.value = Mathf.Lerp(1f, 0f, elapsed / duration);

            yield return null;
        }

        // Hide slider and reset
        if (dashSlider != null)
        {
            dashSlider.value = 0f;
            dashSlider.gameObject.SetActive(false);
        }

        playerController.AirplaneDashEnd();
        dashEnabled = false;
        dashCoroutine = null;
    }


    // ABILITY ---------- MISSILE
    public void Missile()
    {
        if (missileCooldownCoroutine != null)
            StopCoroutine(missileCooldownCoroutine);

        missileCooldownCoroutine = StartCoroutine(MissileCooldownForSeconds(missileCooldown));
    }

    private IEnumerator MissileCooldownForSeconds(float duration)
    {
        missileFired = true;

        // Enable and reset both sliders
        if (missileSliderLeft != null)
        {
            missileSliderLeft.gameObject.SetActive(true);
            missileSliderLeft.value = 1f;
        }

        if (missileSliderRight != null)
        {
            missileSliderRight.gameObject.SetActive(true);
            missileSliderRight.value = 1f;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float value = Mathf.Lerp(1f, 0f, elapsed / duration);

            if (missileSliderLeft != null)
                missileSliderLeft.value = value;

            if (missileSliderRight != null)
                missileSliderRight.value = value;

            yield return null;
        }

        // Hide and reset both sliders
        if (missileSliderLeft != null)
        {
            missileSliderLeft.value = 0f;
            missileSliderLeft.gameObject.SetActive(false);
        }

        if (missileSliderRight != null)
        {
            missileSliderRight.value = 0f;
            missileSliderRight.gameObject.SetActive(false);
        }

        missileFired = false;
        missileCooldownCoroutine = null;
    }






    // Upgrading Abilities ---------------------------------------
    public void UpgradeEnergyDepletionRate()
    {
        if (statsManager.totalCredits >= 10)
        {
            if (edrCurrentLevel < edrMaxLevel)
            {
                playerController.energyDepletionRate -= 0.01f;
                statsManager.totalCredits -= 10;
                edrCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
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
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
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
                energyPauseDuration += 1;
                statsManager.totalAbilityPoints -= 1;
                pedLengthCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradePauseEnergyDepletionAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (pedAmmoCurrentLevel < pedAmmoMaxLevel)
            {
                pauseEnergyAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                pedAmmoCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradeBoostLength()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (boostLengthCurrentLevel < boostLengthMaxLevel)
            {
                boostDuration += 0.2f;
                statsManager.totalAbilityPoints -= 1;
                boostLengthCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradeBoostAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (boostAmmoCurrentLevel < boostAmmoMaxLevel)
            {
                boostAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                boostAmmoCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradeInvincibilityLength()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (invincibilityLengthCurrentLevel < invincibilityLengthMaxLevel)
            {
                invincibleDuration += 0.2f;
                statsManager.totalAbilityPoints -= 1;
                invincibilityLengthCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradeInvincibilityAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (invincibilityAmmoCurrentLevel < invincibilityAmmoMaxLevel)
            {
                invincibilityAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                invincibilityAmmoCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradeDashAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (dashAmmoCurrentLevel < dashAmmoMaxLevel)
            {
                dashAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                dashAmmoCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }

    public void UpgradeMissileAmmo()
    {
        if (statsManager.totalAbilityPoints >= 1)
        {
            if (missileAmmoCurrentLevel < missileAmmoMaxLevel)
            {
                missileAmmo += 1;
                statsManager.totalAbilityPoints -= 1;
                missileAmmoCurrentLevel += 1;
                UpdateUpgradesMenuStats();
                UpdateAbilityLevelsText();
            }

        }
    }



    public void UpdateAbilityLevelsText()
    {
        edrCurrent.text = edrCurrentLevel.ToString();
        lcsCurrent.text = lcsCurrentLevel.ToString();

        pedLengthCurrent.text = pedLengthCurrentLevel.ToString();
        pedAmmoCurrent.text = pedAmmoCurrentLevel.ToString();

        boostLengthCurrent.text = boostLengthCurrentLevel.ToString();
        boostAmmoCurrent.text = boostAmmoCurrentLevel.ToString();

        invincibilityLengthCurrent.text = invincibilityLengthCurrentLevel.ToString();
        invincibilityAmmoCurrent.text = invincibilityAmmoCurrentLevel.ToString();

        dashAmmoCurrent.text = dashAmmoCurrentLevel.ToString();

        missileAmmoCurrent.text = missileAmmoCurrentLevel.ToString();
    }


    public void UpdateUpgradesMenuStats()
    {
        if (upgradesMenuTotalCreditsText != null)
        {
            upgradesMenuTotalCreditsText.text = "Credits: " + statsManager.totalCredits;
            upgradesMenuTotalAbilityPointsText.text = "Ability Points: " + statsManager.totalAbilityPoints;
            upgradesStatsMenuTotalCreditsText.text = "Credits: " + statsManager.totalCredits;
            upgradesAbilitiesMenuTotalAbilityPointsText.text = "Ability Points: " + statsManager.totalAbilityPoints;
        }
    }





}   */