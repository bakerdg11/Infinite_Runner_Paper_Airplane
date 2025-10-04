using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;


    private PlayerController playerController;
    public GameManager gameManager;
    public UpgradesManager upgradesManager;

    public Slider energySlider;

    public Button steerLeftButton;
    public Button steerRightButton;

    public Button pauseButton;
    public Button pauseEnergyDepletion;
    public Button boost;
    public Button invincible;
    public Button dash;
    public Button[] missileLaunch;


    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        pauseButton.onClick.AddListener(OnPauseButtonPressed);
        pauseEnergyDepletion.onClick.AddListener(OnPauseEnergyDepletionButtonPressed);
        boost.onClick.AddListener(OnBoostButtonPressed);
        invincible.onClick.AddListener(OnInvincibleButtonPressed);
        dash.onClick.AddListener(OnDashButtonPressed);

        foreach (Button btn in missileLaunch)
        {
            btn.onClick.AddListener(OnMissileLaunchButtonPressed);
        }*/
    }


    void OnEnable()
    {
        StartCoroutine(FindAirplaneWhenReady());
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




    public void DisplayDistanceTravelled()
    {

    }

    public void DisplayCreditsCollected()
    {

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
}
