using System.Collections;
using UnityEngine;

public class HairSpawnBridge : MonoBehaviour
{
    public SpawnHairStyle spawnHairStyleRef;
    public float hairDisplayTime = 30f;

    private GameObject currentHairInstance;
    private bool roundRunning = false;

    public float xScale = 1f;
    public float yScale = 1f;

    public bool SpawnHairRound(System.Action onComplete = null)
    {
        if (spawnHairStyleRef == null) return false;
        if (spawnHairStyleRef.hairStyles == null || spawnHairStyleRef.hairStyles.Count == 0) return false;
        if (roundRunning) return false;

        StartCoroutine(HairRoundCoroutine(onComplete));
        return true;
    }

    private IEnumerator HairRoundCoroutine(System.Action onComplete)
    {
        roundRunning = true;

        int randomIndex = Random.Range(0, spawnHairStyleRef.hairStyles.Count);
        GameObject prefab = spawnHairStyleRef.hairStyles[randomIndex];

        currentHairInstance = Instantiate(prefab, spawnHairStyleRef.transform.position, spawnHairStyleRef.transform.rotation);

        currentHairInstance.transform.localScale = new Vector3 (xScale, yScale, 3f);
        Debug.Log($"hair: {currentHairInstance.transform} or {currentHairInstance.transform.localScale}");

        CountBools(currentHairInstance, out int startTrue, out int startFalse);

        float timer = hairDisplayTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        CountBools(currentHairInstance, out int endTrue, out int endFalse);

        int correctCuts = startTrue - endTrue;
        int wrongCuts = startFalse - endFalse;
        int missedCuts = endTrue;
        int roundScore = correctCuts - (wrongCuts * 2) - missedCuts;

        spawnHairStyleRef.totalPoints += roundScore;

        if (currentHairInstance != null) Destroy(currentHairInstance);

        roundRunning = false;
        onComplete?.Invoke();
    }

    private void CountBools(GameObject parent, out int trues, out int falses)
    {
        trues = 0; falses = 0;
        if (parent == null) return;

        WantToBeCut[] scripts = parent.GetComponentsInChildren<WantToBeCut>(true);
        foreach (var script in scripts)
            if (script.wantCutting) trues++; else falses++;
    }

    public bool IsRoundRunning() => roundRunning;
}