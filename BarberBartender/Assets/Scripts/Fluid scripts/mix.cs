using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    public static MixManager Instance;

    [Header("Mix Recipes")]
    public List<MixRecipe> recipes = new List<MixRecipe>();

    [Header("Delay Between Spawns (seconds)")]
    public float spawnDelay = 0.1f; // tiny delay to avoid crash when many objects mix

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TryMix(Mixable a, Mixable b)
    {
        if (a == null || b == null) return;

        foreach (var recipe in recipes)
        {
            if (Matches(recipe, a.objectID, b.objectID))
            {
                StartCoroutine(MixWithDelay(a, b, recipe.result));
                return;
            }
        }
    }

    bool Matches(MixRecipe recipe, string idA, string idB)
    {
        return (recipe.objectAID == idA && recipe.objectBID == idB) ||
               (recipe.objectAID == idB && recipe.objectBID == idA);
    }

    IEnumerator MixWithDelay(Mixable a, Mixable b, GameObject resultPrefab)
    {
        if (a == null || b == null || resultPrefab == null)
            yield break;

        // Cache positions before destroying
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;

        // Destroy originals immediately
        Destroy(a.gameObject);
        Destroy(b.gameObject);

        // Spawn a result at a's position
        Instantiate(resultPrefab, posA, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay);

        // Spawn a result at b's position
        Instantiate(resultPrefab, posB, Quaternion.identity);
    }
}

[System.Serializable]
public struct MixRecipe
{
    public string objectAID;
    public string objectBID;
    public GameObject result;
}