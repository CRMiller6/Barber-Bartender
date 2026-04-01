using UnityEngine;

public class FlavorPoints : MonoBehaviour
{
    [Header("Scoring")]
    public int wrongPoints = -1;

    public int CurrentScore { get; private set; }

    public void AddPoints(int points)
    {
        CurrentScore += points;
        Debug.Log($"Correct! +{points} points.");
        Debug.Log("Current Score: " + CurrentScore);
    }

    public void AddWrong()
    {
        CurrentScore += wrongPoints;
        Debug.Log($"Wrong drink! {wrongPoints} points.");
        Debug.Log("Current Score: " + CurrentScore);
    }
}