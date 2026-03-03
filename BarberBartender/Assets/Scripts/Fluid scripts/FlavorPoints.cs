using UnityEngine;

public class FlavorPoints : MonoBehaviour
{
    [Header("Scoring")]
    public int correctPoints = 2;
    public int wrongPoints = -1;

    public int CurrentScore { get; private set; }

    public void AddCorrect()
    {
        CurrentScore += correctPoints;
        Debug.Log($"Correct! +{correctPoints} points.");
        Debug.Log("Current Score: " + CurrentScore);
    }

    public void AddWrong()
    {
        CurrentScore += wrongPoints;
        Debug.Log($"Wrong drink! {wrongPoints} points.");
        Debug.Log("Current Score: " + CurrentScore);
    }
}