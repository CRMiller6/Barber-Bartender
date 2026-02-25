using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnHairStyle : MonoBehaviour
{
    public List<GameObject> hairStyles = new List<GameObject>();
    public int totalPoints = 0;
    
    private GameObject currentHairInstance;

    void Start()
    {
        StartCoroutine(HairCycleRoutine());
    }

    IEnumerator HairCycleRoutine()
    {
        float startTime = Time.time;
        float durationLimit = 300f; 

        while (Time.time < startTime + durationLimit) 
        {
            if (hairStyles.Count > 0)
            {
                int randomIndex = Random.Range(0, hairStyles.Count);
                currentHairInstance = Instantiate(hairStyles[randomIndex], transform.position, transform.rotation);
                
                int startTrue, startFalse;
                CountBools(currentHairInstance, out startTrue, out startFalse);

                yield return new WaitForSeconds(30f);

                int endTrue, endFalse;
                CountBools(currentHairInstance, out endTrue, out endFalse);

                totalPoints += (startTrue - endTrue);
                Destroy(currentHairInstance);
            }

            if (Time.time >= startTime + durationLimit) break;

            Debug.Log("Waiting 10s for next round...");
            yield return new WaitForSeconds(10f);
        }

        Debug.Log("5 minutes are up! Final Score: " + totalPoints);
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
}
//ahhhhhh