using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public PaperAirplaneController airplaneController;

    [Header("Distance Travelled")]
    public GameObject startingPoint;
    public float distanceTravelled;
    public TMP_Text distanceTravelledText;
    public int distanceTravelledCredits;

    [Header("In Game Pickups/Stats")]
    public int pickupCredits;
    public TMP_Text pickupCreditsText;
    public int totalCredits;
    public int totalAbilityPoints = 10;


    [Header("Crashed Menu Results Texts")]
    public TMP_Text crashedMenuDistanceTravelledText;
    public TMP_Text crashedMenuDistanceCreditsText;
    public TMP_Text crashedMenuPickupCreditsText;
    public TMP_Text crashedMenuTotalCreditsText;


    [Header("Upgrades Menu")]
    public TMP_Text upgradesMenuTotalCreditsText;
    public TMP_Text upgradesMenuTotalAbilityPointsText;
    public TMP_Text upgradesStatsMenuTotalCreditsText;
    public TMP_Text upgradesAbilitiesMenuTotalAbilityPointsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Listen for scene changes to re-hook references
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Needed to find the AirplaneController from Scene 2
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        airplaneController = FindFirstObjectByType<PaperAirplaneController>();
        startingPoint = GameObject.Find("StartingPoint");
    }


    public void RestartLevelScene()
    {
        if (PersistentMenuManager.Instance != null)
        {
            PersistentMenuManager.Instance.CloseAllMenus();
        }

        pickupCredits = 0;
        SceneManager.LoadScene("2.Level1", LoadSceneMode.Single);
        Time.timeScale = 1f;

        /*
        if (airplaneController.energySlider != null)
        {
            airplaneController.energySlider.value = 1f;
        }*/
    }


    void Update()
    {
        //Updates distance travelled text in HUD when plane is moving
        if (airplaneController != null && startingPoint != null && distanceTravelledText != null)
        {
            distanceTravelled = Vector3.Distance(airplaneController.transform.position, startingPoint.transform.position);
            distanceTravelledText.text = "Distance: " + Mathf.FloorToInt(distanceTravelled) + "m";
        }
    }


    public void UpdatePickupCredits(int amount)
    {
        //Updates credits amount in HUD when picking up credits
        pickupCredits += amount;

        if (pickupCreditsText != null)
        {
            pickupCreditsText.text = "Credits: " + pickupCredits;
        }
        else
        {
            Debug.LogWarning("Pickup Credits Text not assigned");
        }
    }


    public void DetermineDistanceTravelled()
    {
        if (airplaneController == null || startingPoint == null)
        {
            Debug.LogWarning("Missing references for distance calculation.");
            return;
        }

        // Final distance already calculated in Update(), so just round and calculate credits
        int finalDistance = Mathf.FloorToInt(distanceTravelled);
        distanceTravelledCredits = Mathf.FloorToInt(finalDistance * 0.01f); // 1% conversion rate

        totalCredits += distanceTravelledCredits;
    }


    public void UpdateTotalCredits()
    {
        totalCredits += pickupCredits;
    }




    public void UpdateCrashedMenuStats()
    {
        int finalDistance = Mathf.FloorToInt(distanceTravelled);

        if (crashedMenuDistanceTravelledText != null)
        {
            crashedMenuDistanceTravelledText.text = "Distance Travelled: " + finalDistance + "m";
        }
        else
        {
            Debug.LogWarning("Distance Travelled Text not assigned");
        }

        if (crashedMenuDistanceCreditsText != null)
        {
            crashedMenuDistanceCreditsText.text = "Distance Credits: " + distanceTravelledCredits;
        }
        else
        {
            Debug.LogWarning("Distance Credits Text not assigned");
        }

        if (crashedMenuPickupCreditsText != null)
        {
            crashedMenuPickupCreditsText.text = "Credits Collected: " + pickupCredits;
        }
        else
        {
            Debug.LogWarning("Pickup Credits Text not assigned");
        }

        if (crashedMenuTotalCreditsText != null)
        {
            crashedMenuTotalCreditsText.text = "Total Credits: " + totalCredits;
        }
        else
        {
            Debug.LogWarning("Total Credits Text not assigned");
        }
    }




    public void UpdateUpgradesMenuStats()
    {
        if (upgradesMenuTotalCreditsText != null)
        {
            upgradesMenuTotalCreditsText.text = "Credits: " + totalCredits;
            upgradesMenuTotalAbilityPointsText.text = "Ability Points: " + totalAbilityPoints;
            upgradesStatsMenuTotalCreditsText.text = "Credits: " + totalCredits;
            upgradesAbilitiesMenuTotalAbilityPointsText.text = "Ability Points: " + totalAbilityPoints;
        }
    }


    public void BuyAbilityPoint()
    {
        if (totalCredits >= 50)
        {
            totalCredits -= 50;
            totalAbilityPoints += 1;
            UpdateUpgradesMenuStats();
        }
    }










}
