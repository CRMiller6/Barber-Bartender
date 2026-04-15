using UnityEngine;
using System;

public class HairData : MonoBehaviour
{
    [Header("Current State")]
    public bool isWet = false;
    private bool wasEverWet = false; // Tracks if the player used the spray

    [Header("Points Configuration")]
    public int basePoints = 0;        // Points earned from cutting
    public int wetToDryReward = 20;   // Reward for drying wet hair
    public int leftWetPenalty = -15;  // Penalty if still wet at the end

    // public SpriteRenderer spriteRenderer;
    // public Color wetColor = new Color(0.7f, 0.7f, 1.0f); // Slightly blue/darker
    // public Color dryColor = Color.white;

    private SpriteRenderer sr; 

    void Awake() 
    {
        // This runs the moment Instantiate() is called for THIS specific clone
        // sr = GetComponent<SpriteRenderer>(); 
        
        // Safety check in case the renderer is on a child object
        // if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Call this to spray the hair
    public void SetWet()
    {
        isWet = true;
        wasEverWet = true;
        Debug.Log("Hair is now Wet.");
        // spriteRenderer.color = wetColor;
        // UpdateVisuals();
    }

    // Call this to dry the hair (e.g., with a blowdryer)
    public void SetDry()
    {
        isWet = false;
        Debug.Log("Hair is now Dry.");
        // spriteRenderer.color = dryColor;
        // UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (sr != null)
        {
            // sr.color = isWet ? wetColor : dryColor;
        }
    }

    public int GetFinalScore()
    {
        int finalRoundScore = basePoints;

        // Case 1: Left Wet (Penalty)
        if (isWet)
        {
            finalRoundScore += leftWetPenalty;
            Debug.Log($"Penalty: Left wet. {leftWetPenalty} points.");
        }
        // Case 2: Went from Wet to Dry (Reward)
        else if (!isWet && wasEverWet)
        {
            finalRoundScore += wetToDryReward;
            Debug.Log($"Reward: Dried successfully! +{wetToDryReward} points.");
        }
        // Case 3: Left Dry (No change)
        else
        {
            Debug.Log("No moisture changes applied.");
        }

        return finalRoundScore;
    }

    // Call this from your cutting logic
    public void AddCutPoints(int points)
    {
        basePoints += points;
    }

    public Action OnTargetDestroyed;

    private void OnDestroy() 
    {
        OnTargetDestroyed?.Invoke();
    }
}