using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    public FlavorPoints flavorPoints;
    public SpawnHairStyle hairPoints;

    [Header("Win Conditions")]
    public int flavorPointsNeeded = 10;
    public int hairPointsNeeded = 5;
    public int totalPointsNeeded = 20;
    public int balanceMargin = 5;

    [Header("Next Scene")]
    public string nextSceneName;
    public float sceneLoadDelay = 2f;

    private bool winTriggered = false;

    void Update()
    {
        if (flavorPoints == null || hairPoints == null) return;

        int flavorPointTotal = flavorPoints.CurrentScore;
        int hairPointTotal = hairPoints.totalPoints;
        int totalPoints = flavorPointTotal + hairPointTotal;

        if (!winTriggered &&
            flavorPointTotal >= flavorPointsNeeded &&
            hairPointTotal >= hairPointsNeeded &&
            totalPoints >= totalPointsNeeded)
        {
            winTriggered = true;

            string balanceMsg;
            if (Mathf.Abs(flavorPointTotal - hairPointTotal) <= balanceMargin)
                balanceMsg = "Your work was balanced!";
            else
                balanceMsg = flavorPointTotal > hairPointTotal ? "You did more Flavor Points!" : "You did more Hair Points!";

            Debug.Log($"You won! Flavor: {flavorPointTotal}, Hair: {hairPointTotal}. {balanceMsg}");
        }
    }

    // Call this at end of day from DayCycleManager
    public void SaveResults()
    {
        int flavorPointTotal = flavorPoints != null ? flavorPoints.CurrentScore : 0;
        int hairPointTotal = hairPoints != null ? hairPoints.totalPoints : 0;
        int totalPoints = flavorPointTotal + hairPointTotal;

        string balanceMsg;
        if (Mathf.Abs(flavorPointTotal - hairPointTotal) <= balanceMargin)
            balanceMsg = "Your work was balanced!";
        else
            balanceMsg = flavorPointTotal > hairPointTotal ? "You did more Flavor Points!" : "You did more Hair Points!";

        // Save current day results
        DayResults.flavorPoints = flavorPointTotal;
        DayResults.hairPoints = hairPointTotal;
        DayResults.totalPoints = totalPoints;
        DayResults.balanceMessage = balanceMsg;

        // Update cumulative points
        DayResults.AddToCumulative();

        Debug.Log($"Day results saved. Cumulative Flavor: {DayResults.cumulativeFlavorPoints}, Hair: {DayResults.cumulativeHairPoints}, Total: {DayResults.cumulativeTotalPoints}");

        StartCoroutine(LoadNextScene());
    }

    private System.Collections.IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}