using UnityEngine;


public class HairPoints : MonoBehaviour
{

    public int trueCount;
    public int falseCount;


    public void PossiblePoints()
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

    


}
