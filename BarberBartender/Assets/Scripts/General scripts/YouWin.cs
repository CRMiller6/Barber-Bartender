using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    public FlavorPoints flavorPoints;   // Reference to your FlavorPoints script
    public SpawnHairStyle hairPoints;   // Reference to your SpawnHairStyle script

    [Header("Win Conditions")]
    public int flavorPointsNeeded = 10; // Minimum Flavor Points needed
    public int hairPointsNeeded = 5;    // Minimum Hair Points needed
    public int totalPointsNeeded = 20;  // Minimum total points needed

    [Header("Balance Settings")]
    public int balanceMargin = 0;       // Difference allowed to consider the work "balanced"

    private bool winTriggered = false;

    void Update()
    {
        if (flavorPoints == null || hairPoints == null) return;

        int flavorPointTotal = flavorPoints.CurrentScore;  // Flavor points (barber/bartending)
        int hairPointTotal = hairPoints.totalPoints;       // Hair points (haircuts)

        int totalPoints = flavorPointTotal + hairPointTotal;

        // Win condition
        if (!winTriggered &&
            flavorPointTotal >= flavorPointsNeeded &&
            hairPointTotal >= hairPointsNeeded &&
            totalPoints >= totalPointsNeeded)
        {
            winTriggered = true;

            // Check if work is balanced
            if (Mathf.Abs(flavorPointTotal - hairPointTotal) <= balanceMargin)
            {
                Debug.Log($"You won! Flavor Points = {flavorPointTotal}, Hair Points = {hairPointTotal}. Your work was balanced!");
            }
            else
            {
                string moreOf = flavorPointTotal > hairPointTotal ? "Flavor Points" : "Hair Points";
                Debug.Log($"You won! Flavor Points = {flavorPointTotal}, Hair Points = {hairPointTotal}. You did more {moreOf}!");
            }
        }
    }
}