using UnityEngine;
using System.Linq;

public class HairPoints : MonoBehaviour
{

    public int trueCount;
    public int falseCount;


    public void PointsReceived()
    {
        WantToBeCut[] childScripts = GetComponentsInChildren<WantToBeCut>(true);

        trueCount = 0;
        falseCount = 0;

        foreach (WantToBeCut childScript in childScripts)
        {
            if (childScript.wantCutting == true)
            {
                trueCount++;
            }
            
            else
            {
                falseCount = falseCount + 2;
            }
        }
        Debug.Log($"results - True {trueCount}, False: {falseCount}");
    }

    public int QuickTrueTally => GetComponentsInChildren<WantToBeCut>().Count(s => s.wantCutting);
}
