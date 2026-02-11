using UnityEngine;

public class SpawnHairStyle : MonoBehaviour
{
    public HairPoints hairP;

    public int trueCountStart;
    public int falseCountStart;

    public int trueCountEnd;
    public int falseCountEnd;


    public void OnSpawn()
    {
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
