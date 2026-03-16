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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Start fully invisible
        DisableAllChildren();
    }

    private void Start()
    {
        // Start the initial between-drinks timer so the first spawn happens after timeBetweenDrinks.
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
        if (!startDrinkOnCustomerAtCounter)
        {
            Debug.LogWarning("CustomerArrivedAtCounter called while startDrinkOnCustomerAtCounter is false.");
            return;
        }

        if (!readyToSpawn)
        {
            Debug.Log("Customer arrived but manager is not ready to start a drink.");
            return;
        }

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
        if (possibleDrinks == null || possibleDrinks.Length == 0)
        {
            Debug.LogWarning("No drinks assigned!");
            return;
        }

        int randomIndex = Random.Range(0, possibleDrinks.Length);
        currentTargetDrinkID = possibleDrinks[randomIndex].objectID;

        // Set the color for direct children
        Color colorToShow = possibleDrinks[randomIndex].displayColor;
        colorToShow.a = 1f;
        SetColorOnDirectChildren(colorToShow);

        // Enable all children (and grandchildren will automatically be visible if parent active)
        EnableAllChildren();

        Debug.Log("New Order: " + currentTargetDrinkID);
    }

    public void EndDrinkEarly()
    {
        if (drinkCoroutine != null)
        {
            StopCoroutine(drinkCoroutine);
            drinkCoroutine = null;
        }

        isDrinkActive = false;
        DisableAllChildren();
        StartBetweenTimer();
    }

    // --- Helper methods ---

    // Only changes color of immediate children
    private void SetColorOnDirectChildren(Color color)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = color;
            }
        }
    }

    // Disables all children (children + grandchildren)
    private void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Enables all children (children + grandchildren)
    private void EnableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}