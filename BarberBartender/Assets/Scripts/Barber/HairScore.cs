using UnityEngine;
using System.Collections.Generic;
using TMPro; // Ensure TextMeshPro is imported

public class HairGameManager : MonoBehaviour
{
    [Header("Setup")]
    public List<GameObject> hairPrefabs;
    public Transform spawnPoint;
    
    [Header("UI")]
    public TMP_Text scoreDisplay;

    private int totalPoints = 0;
    private GameObject currentHairInstance;
    private HairData currentHairData;
    private bool isRoundActive = false;

    void Start()
    {
        UpdateScoreUI(); // Initialize UI
        StartNewRound();
    }

    public void StartNewRound()
    {
        if (hairPrefabs.Count == 0) return;

        // Cleanup previous round if it wasn't cleaned up
        if (currentHairInstance != null)
        {
            Destroy(currentHairInstance);
        }

        int index = Random.Range(0, hairPrefabs.Count);
        currentHairInstance = Instantiate(hairPrefabs[index], spawnPoint.position, spawnPoint.rotation);
        
        // Ensure the scale is set
        currentHairInstance.transform.localScale = new Vector3(3f, 3f, 3f);

        currentHairData = currentHairInstance.GetComponentInChildren<HairData>();

        if (currentHairData != null)
        {
            isRoundActive = true;
            Debug.Log("Round Started");
        }
        else
        {
            Debug.LogError("The spawned prefab is missing the HairData script!");
        }
    }

    // Call this from your "Finish" button
    public void EndRound()
    {
        if (!isRoundActive || currentHairData == null) return;

        int roundScore = currentHairData.GetFinalScore();
        totalPoints += roundScore;
        Debug.Log($"Round Score: {roundScore}. New Total: {totalPoints}");

        Destroy(currentHairInstance);
        currentHairInstance = null; // Clear reference
        isRoundActive = false;
        
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.text = "Total Score: " + totalPoints;
        }
    }
}
