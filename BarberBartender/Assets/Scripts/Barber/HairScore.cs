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

    public int totalPoints = 0;
    private GameObject currentHairInstance;
    private HairData currentHairData;
    private bool isRoundActive = false;

    public float timer;
    public float timeLimit;
    private bool waiting;

    void Start()
    {
        scoreDisplay.text = "Total Score: " + totalPoints; // Initialize UI
        StartNewRound();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeLimit && waiting == false)
        {
            EndRound();
            waiting = true;
        }
        else if (timer > timeLimit/2 && waiting == true)
        {
            StartNewRound();
            waiting = false;
            timer = 0;
        }
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
        
        WantToBeCut[] allCuttingScripts = currentHairInstance.GetComponentsInChildren<WantToBeCut>();

    foreach (var script in allCuttingScripts)
    {
        // Add -2 if true, +1 if false
        roundScore += script.wantCutting ? -2 : 1;
    }

        totalPoints += roundScore;

        Destroy(currentHairInstance);
        currentHairInstance = null; // Clear reference
        isRoundActive = false;

        timer = 0;
        
        scoreDisplay.text = "Total Score: " + totalPoints;
    }

    void UpdateScoreUI()
    {
        // if (scoreDisplay != null)
        // {
        Debug.Log ("ahahdah");
        
        // }
    }
}
