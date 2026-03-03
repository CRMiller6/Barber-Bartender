using UnityEngine;
using System.Collections;

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

    [Header("Timing")]
    public float drinkDisplayTime = 5f;  // How long the drink stays active
    public float timeBetweenDrinks = 3f; // Delay before next drink appears

    private string currentTargetDrinkID;
    private SpriteRenderer spriteRenderer;
    private bool isDrinkActive = false;

    public string CurrentTargetDrinkID => currentTargetDrinkID;
    public bool IsDrinkActive => isDrinkActive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Start fully invisible
        SetColorRecursively(gameObject, new Color(0, 0, 0, 0));
    }

    private void Start()
    {
        StartCoroutine(DrinkCycle());
    }

    private IEnumerator DrinkCycle()
    {
        while (true)
        {
            // Wait between drinks (invisible)
            isDrinkActive = false;
            SetColorRecursively(gameObject, new Color(0, 0, 0, 0));
            yield return new WaitForSeconds(timeBetweenDrinks);

            // Pick and show a new drink
            PickNewTargetDrink();
            isDrinkActive = true;

            // Display drink for drinkDisplayTime
            float timer = drinkDisplayTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // Hide after time expires
            isDrinkActive = false;
            SetColorRecursively(gameObject, new Color(0, 0, 0, 0));
        }
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

        // Apply the drink color **with full alpha**
        Color colorToShow = possibleDrinks[randomIndex].displayColor;
        colorToShow.a = 1f; // ensure fully visible
        SetColorRecursively(gameObject, colorToShow);

        Debug.Log("New Order: " + currentTargetDrinkID);
    }

    public void EndDrinkEarly()
    {
        // Called when player submits early
        StopAllCoroutines();
        isDrinkActive = false;
        SetColorRecursively(gameObject, new Color(0, 0, 0, 0)); // hide
        StartCoroutine(DrinkCycle()); // start delay for next drink
    }

    private void SetColorRecursively(GameObject obj, Color color)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color; // overwrite fully, including alpha
        }

        foreach (Transform child in obj.transform)
        {
            SetColorRecursively(child.gameObject, color);
        }
    }
}