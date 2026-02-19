using System.Collections.Generic;
using UnityEngine;

public class MixManager : MonoBehaviour
{
    [Header("Mix Recipes")]
    public List<MixRecipe> recipes = new List<MixRecipe>();

    private Dictionary<string, GameObject> mixDict = new Dictionary<string, GameObject>();

    void Awake()
    {
        foreach (var recipe in recipes)
        {
            string key1 = GetKey(recipe.objectAID, recipe.objectBID);
            string key2 = GetKey(recipe.objectBID, recipe.objectAID);

            if (!mixDict.ContainsKey(key1)) mixDict.Add(key1, recipe.result);
            if (!mixDict.ContainsKey(key2)) mixDict.Add(key2, recipe.result);
        }
    }

    string GetKey(string idA, string idB)
    {
        return idA + "+" + idB;
    }

    public static void TryMix(Mixable a, Mixable b)
    {
        if (a == null || b == null) return;

        MixManager mixManager = Object.FindFirstObjectByType<MixManager>();
        if (mixManager == null) return;

        string key = mixManager.GetKey(a.objectID, b.objectID);

        if (mixManager.mixDict.ContainsKey(key))
        {
            GameObject resultPrefab = mixManager.mixDict[key];

            // Spawn the result at each object's position
            Instantiate(resultPrefab, a.transform.position, Quaternion.identity);
            Instantiate(resultPrefab, b.transform.position, Quaternion.identity);

            Destroy(a.gameObject);
            Destroy(b.gameObject);
        }
    }
}

[System.Serializable]
public struct MixRecipe
{
    public string objectAID;
    public string objectBID;
    public GameObject result;
}
