using UnityEngine;
using TMPro;

public class ShowResults : MonoBehaviour
{
    [Header("UI Text References for Daily Results")]
    public TMP_Text flavorPointsText;
    public TMP_Text hairPointsText;
    public TMP_Text totalPointsText;
    public TMP_Text balanceMessageText;

    [Header("UI Text References for Cumulative Results")]
    public TMP_Text cumulativeFlavorText;
    public TMP_Text cumulativeHairText;
    public TMP_Text cumulativeTotalText;

    void Start()
    {
        // Display daily results
        if (flavorPointsText != null)
            flavorPointsText.text = "Flavor Points: " + DayResults.flavorPoints;

        if (hairPointsText != null)
            hairPointsText.text = "Hair Points: " + DayResults.hairPoints;

        if (totalPointsText != null)
            totalPointsText.text = "Total Points: " + DayResults.totalPoints;

        if (balanceMessageText != null)
            balanceMessageText.text = DayResults.balanceMessage;

        // Display cumulative results
        if (cumulativeFlavorText != null)
            cumulativeFlavorText.text = "Flavor Points: " + DayResults.cumulativeFlavorPoints;

        if (cumulativeHairText != null)
            cumulativeHairText.text = "Hair Points: " + DayResults.cumulativeHairPoints;

        if (cumulativeTotalText != null)
            cumulativeTotalText.text = "Total Points: " + DayResults.cumulativeTotalPoints;

        // Reset daily results AFTER showing them
        DayResults.ResetDayResults();
    }
}