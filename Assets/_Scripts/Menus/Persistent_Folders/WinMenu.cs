using UnityEngine;
using TMPro;

public class WinMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text distanceTravelledText;
    [SerializeField] private TMP_Text distanceCreditsText;
    [SerializeField] private TMP_Text pickupCreditsText;
    [SerializeField] private TMP_Text totalCreditsText;

    void OnEnable()
    {
        PopulateFromStats();
    }

    private void PopulateFromStats()
    {
        StatsManager.Instance?.FinalizeRun();

        if (distanceTravelledText) distanceTravelledText.text = $"Distance Travelled: {StatsManager.Instance.DistanceMeters}m";
        if (distanceCreditsText) distanceCreditsText.text = $"Travelled Credits: {StatsManager.Instance.DistanceTravelledCredits}";
        if (pickupCreditsText) pickupCreditsText.text = $"Credits Collected: {StatsManager.Instance.PickupCreditsThisRun}";
        if (totalCreditsText) totalCreditsText.text = $"Total Credits: {StatsManager.Instance.TotalCreditsAllTime}";
    }
}