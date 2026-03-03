using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DrinkOrderManager : MonoBehaviour
{
    [System.Serializable]
    public struct DrinkDefinition
    {
        public string objectID;
        public Color displayColor;
    }

    [Header("Drink Options")]
    public DrinkDefinition[] possibleDrinks;

    private string currentTargetDrinkID;
    private SpriteRenderer spriteRenderer;

    public string CurrentTargetDrinkID => currentTargetDrinkID;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PickNewTargetDrink();
    }

    public void PickNewTargetDrink()
    {
        if (possibleDrinks == null || possibleDrinks.Length == 0)
        {
            Debug.LogWarning("No drinks assigned!");
            return;
        }

        int randomIndex = Random.Range(0, possibleDrinks.Length);
        currentTargetDrinkID = possibleDrinks[randomIndex].objectID;

        SetColorRecursively(gameObject, possibleDrinks[randomIndex].displayColor);

        Debug.Log("New Order: " + currentTargetDrinkID);
    }

    private void SetColorRecursively(GameObject obj, Color color)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color newColor = color;
            newColor.a = sr.color.a;
            sr.color = newColor;
        }

        foreach (Transform child in obj.transform)
        {
            SetColorRecursively(child.gameObject, color);
        }
    }
}