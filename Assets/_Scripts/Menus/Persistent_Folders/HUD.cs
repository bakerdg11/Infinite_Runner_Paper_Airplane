using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;


[Serializable]
public struct AbilityCounter
{
    public string abilityId;
    public TMP_Text label; // the tiny number in the top-right of the button
}



public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [Header("References")]
    private PlayerController playerController;
    public GameManager gameManager;
    public UpgradesManager upgradesManager;
    [SerializeField] private AbilitySystem abilitySystem;

    [Header("General")]
    public Button pauseButton;
    public Slider energySlider;
    public TMP_Text runDistance;
    public TMP_Text runCurrency;

    public Button steerLeftButton;
    public Button steerRightButton;

    [Header("Abilities")]
    public Button dash;
    public Button boost;
    public Button invincible;
    public Button pauseEnergyDepletion;
    public Button[] missileLaunch;


    [Header("Ability Counters")]
    [SerializeField] private List<AbilityCounter> counters;

    private Dictionary<string, TMP_Text> _map;


    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _map = new(StringComparer.OrdinalIgnoreCase);
        foreach (var c in counters)
        {
            if (!string.IsNullOrEmpty(c.abilityId) && c.label != null)
                _map[c.abilityId] = c.label;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dash.onClick.AddListener(OnPressDash);
        boost.onClick.AddListener(OnPressBoost);
        invincible.onClick.AddListener(OnPressInvincible);
        pauseEnergyDepletion.onClick.AddListener(OnPressPauseEnergy);

        foreach (Button btn in missileLaunch)
        {
            btn.onClick.AddListener(OnPressMissile);
        }

        pauseButton.onClick.AddListener(OnPauseButtonPressed);
    }


    void OnEnable()
    {
        StartCoroutine(FindAirplaneWhenReady());
        EnsureAbilitySystem();
    }

    private IEnumerator FindAirplaneWhenReady()
    {
        while (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
            yield return null;
        }

        Debug.Log("Airplane found and ready for HUD interaction");
    }




    public void OnPauseButtonPressed()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.OpenPauseMenu();
            Debug.Log("Pause Menu Open");
        }
        else
        {
            Debug.LogWarning("PersistentMenuManager not found.");
        }
    }


    // ----------------------------------------------------------------Energy Slider--------------------
    public void SetEnergyRange(float min, float max)
    {
        if (!energySlider) return;
        energySlider.minValue = min;
        energySlider.maxValue = max;
        energySlider.wholeNumbers = false;   // make sure it’s smooth
    }

    public void UpdateEnergy(float value)
    {
        if (!energySlider) return;
        energySlider.value = value;
    }



    // ----------------------------------------------------------------Distance Travelled--------------------
    public void DisplayDistanceTravelled(int meters)
    {
        if (!runDistance) return;
        runDistance.text = $"Distance: {meters}m";
    }



    // ----------------------------------------------------------------Credits Collected--------------------
    public void UpdatePickupCredits(int creditsCollected)
    {
        if (!runCurrency) return;
        runCurrency.text = $"Credits: {creditsCollected}";
    }

    public void ResetPickupCredits()
    {
        if (runCurrency == null) return;
        runCurrency.text = "Credits: 0";
    }



    // ---------------------------------------------------------------- Buttons and activating abilities ----
    private bool EnsureAbilitySystem()
    {
        if (abilitySystem != null) return true;

        // Try from PlayerManager (best, since your player is managed there)
        var pm = PlayerManager.Instance;
        if (pm != null && pm.PlayerGO != null)
            abilitySystem = pm.PlayerGO.GetComponent<AbilitySystem>();

        // Fallbacks
        if (abilitySystem == null)
            abilitySystem = FindFirstObjectByType<AbilitySystem>(FindObjectsInactive.Exclude);

        if (abilitySystem == null)
            Debug.LogWarning("[HUD] AbilitySystem not found. Is the player spawned yet?");

        return abilitySystem != null;
    }

    public void SetAbilityAmmo(string abilityId, int count)
    {
        if (_map != null && _map.TryGetValue(abilityId, out var label) && label != null)
            label.text = count.ToString();
    }

    public void OnPressDash()
    {
        if (!EnsureAbilitySystem()) return;      // guard against null
        abilitySystem.TryActivate("dash");
    }

    public void OnPressBoost()
    {
        if (!EnsureAbilitySystem()) return;      // guard against null
        abilitySystem.TryActivate("boost");
    }

    public void OnPressInvincible()
    {
        if (!EnsureAbilitySystem()) return;      // guard against null
        abilitySystem.TryActivate("invincible");
    }

    public void OnPressPauseEnergy()
    {
        if (!EnsureAbilitySystem()) return;      // guard against null
        abilitySystem.TryActivate("pauseenergy");
    }

    public void OnPressMissile()
    {
        if (!EnsureAbilitySystem()) return;      // guard against null
        abilitySystem.TryActivate("missile");
    }


}


    /*
    // ----------------------ABILITY BUTTONS-----------------------
    public void OnPauseEnergyDepletionButtonPressed()
    {
        if (upgradesManager.energyDepletionPaused)
        {
            return;
        }
        else
        {
            if (upgradesManager.pauseEnergyAmmo >= 1)
            {
                upgradesManager.EnergyDepletionPaused();
                upgradesManager.pauseEnergyAmmo -= 1;
                upgradesManager.UpdateAmmoUI();
            }

        }
    }

    public void OnBoostButtonPressed()
    {
        if (upgradesManager.boostEnabled || upgradesManager.dashEnabled)
        {
            return;
        }
        else
        {
            if (upgradesManager.boostAmmo >= 1)
            {
                upgradesManager.Boost();
                upgradesManager.boostAmmo -= 1;
                upgradesManager.UpdateAmmoUI();
            }

        }
    }

    public void OnInvincibleButtonPressed()
    {
        if (upgradesManager.invincibleEnabled)
        {
            return;
        }
        else
        {
            if (upgradesManager.invincibilityAmmo >= 1)
            {
                upgradesManager.Invincible();
                upgradesManager.invincibilityAmmo -= 1;
                upgradesManager.UpdateAmmoUI();
            }

        }
    }

    public void OnDashButtonPressed()
    {
        if (upgradesManager.dashEnabled || upgradesManager.boostEnabled)
        {
            return;
        }
        else
        {
            if (upgradesManager.dashAmmo >= 1)
            {
                upgradesManager.Dash();
                upgradesManager.dashAmmo -= 1;
                upgradesManager.UpdateAmmoUI();
            }

        }
    }

    public void OnMissileLaunchButtonPressed()
    {
        if (upgradesManager.missileFired)
        {
            return;
        }
        else
        {
            if (upgradesManager.missileAmmo >= 1)
            {
                upgradesManager.Missile();
                playerController.FireMissile();
                upgradesManager.missileAmmo -= 1;
                upgradesManager.UpdateAmmoUI();
            }

        }

    }

    */