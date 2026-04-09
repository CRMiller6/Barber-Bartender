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
        public int points; // Added: points for this drink
    }

    [Header("Drink Options")]
    public DrinkDefinition[] possibleDrinks;

    [Header("Timing")]
    public float drinkDisplayTime = 5f;  
    public float timeBetweenDrinks = 3f; 

    [Header("Behavior")]
    [Tooltip("When true, the manager will wait until Customer calls CustomerArrivedAtCounter() to pick/start the drink timer.")]
    public bool startDrinkOnCustomerAtCounter = true;

    private string currentTargetDrinkID;
    private SpriteRenderer spriteRenderer;
    private bool isDrinkActive = false;
    private bool readyToSpawn = false;

    private Coroutine betweenCoroutine = null;
    private Coroutine drinkCoroutine = null;

    public string CurrentTargetDrinkID => currentTargetDrinkID;
    public bool IsDrinkActive => isDrinkActive;
    public bool ReadyToSpawn => readyToSpawn;
    public bool tutorial = false;
    public DialogueManager dialogueManager; // Reference to DialogueManager for tutorial integration
    public Collider2D tutorialzoneCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DisableAllChildren();
    }

    private void Start()
    {
        if (tutorial) return;
        else
        StartBetweenTimer();
    }

    private void StartBetweenTimer()
    {
        if (betweenCoroutine != null) StopCoroutine(betweenCoroutine);
        betweenCoroutine = StartCoroutine(BetweenTimerCoroutine());
    }

    private IEnumerator BetweenTimerCoroutine()
    {
        isDrinkActive = false;
        DisableAllChildren();
        readyToSpawn = false;

        yield return new WaitForSeconds(timeBetweenDrinks);

        readyToSpawn = true;
        betweenCoroutine = null;
    }

    public void CustomerArrivedAtCounter()
    {
        if (!startDrinkOnCustomerAtCounter) return;
        if (!readyToSpawn) return;

        readyToSpawn = false;
        StartDrinkTimer();
    }

    private void StartDrinkTimer()
    {
        if (drinkCoroutine != null) StopCoroutine(drinkCoroutine);
        if (betweenCoroutine != null) StopCoroutine(betweenCoroutine);

        PickNewTargetDrink();
        isDrinkActive = true;

        drinkCoroutine = StartCoroutine(DrinkTimerCoroutine());
    }

    private IEnumerator DrinkTimerCoroutine()
    {
        yield return new WaitForSeconds(drinkDisplayTime);

        isDrinkActive = false;
        DisableAllChildren();
        drinkCoroutine = null;

        StartBetweenTimer();
    }

    public void PickNewTargetDrink()
    {
        if (possibleDrinks == null || possibleDrinks.Length == 0) return;

        int randomIndex = Random.Range(0, possibleDrinks.Length);
        currentTargetDrinkID = possibleDrinks[randomIndex].objectID;

        Color colorToShow = possibleDrinks[randomIndex].displayColor;
        colorToShow.a = 1f;
        SetColorOnDirectChildren(colorToShow);

        EnableAllChildren();

        Debug.Log("New Order: " + currentTargetDrinkID + " (Points: " + possibleDrinks[randomIndex].points + ")");
    }

    public void EndDrinkEarly()
    {
        if (drinkCoroutine != null) StopCoroutine(drinkCoroutine);

        isDrinkActive = false;
        DisableAllChildren();
        StartBetweenTimer();
    }

    public int GetCurrentDrinkPoints()
    {
        if (possibleDrinks == null || possibleDrinks.Length == 0) return 0;

        foreach (var drink in possibleDrinks)
        {
            if (drink.objectID == currentTargetDrinkID) return drink.points;
        }
        return 0;
    }

    // --- Helper methods ---

    private void SetColorOnDirectChildren(Color color)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = color;
        }
    }

    private void DisableAllChildren()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }

    private void EnableAllChildren()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(true);
    }

    public void tutorialStart()
    {
        PickNewTutorialDrink();
    }

    private void PickNewTutorialDrink()
    {
        if (possibleDrinks == null || possibleDrinks.Length == 0) return;

        // For tutorial, we can just pick the first drink or a specific one
        currentTargetDrinkID = possibleDrinks[0].objectID;

        Color colorToShow = possibleDrinks[0].displayColor;
        colorToShow.a = 1f;
        SetColorOnDirectChildren(colorToShow);

        EnableAllChildren();

        Debug.Log("Tutorial Order: " + currentTargetDrinkID + " (Points: " + possibleDrinks[0].points + ")");

        tutorialzoneCollider.gameObject.SetActive(true); // Enable tutorial zone to detect drink submission
        
    }

}