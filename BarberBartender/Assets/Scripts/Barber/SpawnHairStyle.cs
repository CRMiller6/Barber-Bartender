using UnityEngine;
using System.Collections.Generic;

public class SpawnHairStyle : MonoBehaviour
{
    public HairPoints hairP;

    public int trueCountStart;
    public int falseCountStart;

    public int trueCountEnd;
    public int falseCountEnd;

    public List<GameObject> hairStyles = new List<GameObject>();


    public void OnSpawn()
    {
        if (hairStyles != null && hairStyles.Count > 0)
        {
            int randomIndex = Random.Range(0, hairStyles.Count);
            Debug.Log(randomIndex);

            GameObject prefabHairToSpawn = hairStyles[randomIndex];
            Debug.Log(prefabHairToSpawn);

            Instantiate(prefabHairToSpawn, transform.position, transform.rotation);
            Debug.Log(hairStyles);
        }

        if (hairP != null)
        {
            hairP.PossiblePoints();
            trueCountStart = hairP.trueCount;
            falseCountStart = hairP.falseCount;
            
            Debug.Log($"SHS results - True {trueCountStart}, False: {falseCountStart}");
        }
    }

    public void AtEnd()
    {
        if (hairP != null)
        {
            hairP.PossiblePoints();
            trueCountEnd = hairP.trueCount;
            falseCountEnd = hairP.falseCount;
            
            Debug.Log($"SHS results - True {trueCountEnd}, False: {falseCountEnd}");
        }
    }
}
