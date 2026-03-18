using UnityEngine;
using TMPro;

public class ShowResults : MonoBehaviour
{
    [Header("UI Text References")]
    public TMP_Text flavorPointsText;
    public TMP_Text hairPointsText;
    public TMP_Text totalPointsText;
    public TMP_Text balanceMessageText;

    void Start()
    {
        if (flavorPointsText != null)
            flavorPointsText.text = "Flavor Points: " + DayResults.flavorPoints;

        if (hairPointsText != null)
            hairPointsText.text = "Hair Points: " + DayResults.hairPoints;

        if (totalPointsText != null)
            totalPointsText.text = "Total Points: " + DayResults.totalPoints;

        if (balanceMessageText != null)
            balanceMessageText.text = DayResults.balanceMessage;

        ResetDayResults();
    }

    private void ResetDayResults()
    {
        DayResults.flavorPoints = 0;
        DayResults.hairPoints = 0;
        DayResults.totalPoints = 0;
        DayResults.balanceMessage = "";
    }
}