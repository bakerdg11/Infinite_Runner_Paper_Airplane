using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameManager gameManager;
    public PaperAirplaneController airplaneController;
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
        while (airplaneController == null)
        {
            airplaneController = FindFirstObjectByType<PaperAirplaneController>();
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
                airplaneController.FireMissile();
                upgradesManager.missileAmmo -= 1;
                upgradesManager.UpdateAmmoUI();
            }

        }

    }

    */
}
