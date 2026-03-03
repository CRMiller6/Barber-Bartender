using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SpawnHairStyle : MonoBehaviour
{
    public List<GameObject> hairStyles = new List<GameObject>();
    public int totalPoints = 0;
    public TMP_Text cutScore;
    
    private GameObject currentHairInstance;

    void Start()
    {

        StartCoroutine(HairCycleRoutine());
    }

    IEnumerator HairCycleRoutine()
    {
        yield return new WaitForSeconds(40f);
        float startTime = Time.time;
        float durationLimit = 300f;

        while (Time.time < startTime + durationLimit) 
        {
            if (hairStyles.Count > 0)
            {
                int randomIndex = Random.Range(0, hairStyles.Count);
                currentHairInstance = Instantiate(hairStyles[randomIndex], transform.position, transform.rotation);
                
                CountBools(currentHairInstance, out int startTrue, out int startFalse);

                yield return new WaitForSeconds(30f);

                CountBools(currentHairInstance, out int endTrue, out int endFalse);

                int correctCuts = startTrue - endTrue;
                int wrongCuts = startFalse - endFalse;
                int missedCuts = endTrue;
                
                int roundScore = (correctCuts) - (wrongCuts * 2) - (missedCuts);

                totalPoints += roundScore;

                Debug.Log($"Round Over! Correct: {correctCuts}, Wrong: {wrongCuts}. Points added: {roundScore}");
                
                Destroy(currentHairInstance);
            }

            if (Time.time >= startTime + durationLimit) break;

            yield return new WaitForSeconds(10f);
        }

        Debug.Log("Game Over! Final Score: " + totalPoints);
    }

    void CountBools(GameObject parent, out int trues, out int falses)
    {
        trues = 0; falses = 0;
        if (parent == null) return;

        WantToBeCut[] scripts = parent.GetComponentsInChildren<WantToBeCut>(true);
        foreach (var script in scripts)
        {
            if (script.wantCutting) trues++;
            else falses++;
        }
    }

    void Update()
    {
        cutScore.text = "Haircut Score: " + totalPoints;
    }
}