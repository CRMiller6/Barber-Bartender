using UnityEngine;

// Holds the day results and cumulative points across all days
public static class DayResults
{
    // Current day points (reset each day after showing)
    public static int flavorPoints = 0;
    public static int hairPoints = 0;
    public static int totalPoints = 0;
    public static string balanceMessage = "";

    // Cumulative points across all days (never reset automatically)
    public static int cumulativeFlavorPoints = 0;
    public static int cumulativeHairPoints = 0;
    public static int cumulativeTotalPoints = 0;

    // Call this at end of day to add current day to cumulative totals
    public static void AddToCumulative()
    {
        cumulativeFlavorPoints += flavorPoints;
        cumulativeHairPoints += hairPoints;
        cumulativeTotalPoints += totalPoints;
    }

    // Reset only current day results
    public static void ResetDayResults()
    {
        flavorPoints = 0;
        hairPoints = 0;
        totalPoints = 0;
        balanceMessage = "";
    }
}