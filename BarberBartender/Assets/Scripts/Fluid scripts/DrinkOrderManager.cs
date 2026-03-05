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
        SetColorRecursively(gameObject, new Color(0, 0, 0, 0));
    }

    private void Start()
    {
        // Start the initial between-drinks timer so the first spawn happens after timeBetweenDrinks.
        StartBetweenTimer();
    }

    private void StartBetweenTimer()
    {
        // Ensure only one between coroutine runs
        if (betweenCoroutine != null) StopCoroutine(betweenCoroutine);
        betweenCoroutine = StartCoroutine(BetweenTimerCoroutine());
    }

    private IEnumerator BetweenTimerCoroutine()
    {
        // Hide any UI while waiting for spawn
        isDrinkActive = false;
        SetColorRecursively(gameObject, new Color(0, 0, 0, 0));
        readyToSpawn = false;

        yield return new WaitForSeconds(timeBetweenDrinks);

        // After waiting, signal spawner that it may spawn a customer.
        readyToSpawn = true;
        betweenCoroutine = null;
    }

    /// <summary>
    /// Called by the customer when they reach their stop point.
    /// If startDrinkOnCustomerAtCounter==true and ReadyToSpawn==true, this will pick the drink and start the drink timer.
    /// If startDrinkOnCustomerAtCounter==false, this method does nothing (manager behaves as before, auto-picking).
    /// </summary>
    public void CustomerArrivedAtCounter()
    {
        if (!startDrinkOnCustomerAtCounter)
        {
            Debug.LogWarning("CustomerArrivedAtCounter called while startDrinkOnCustomerAtCounter is false.");
            return;
        }

        if (!readyToSpawn)
        {
            // Either the manager hasn't finished the between timer, or a drink is already active.
            Debug.Log("Customer arrived but manager is not ready to start a drink.");
            return;
        }

        // Consume the ready flag so no other customer spawns for this round.
        readyToSpawn = false;

        // Start the drink timer / selection
        StartDrinkTimer();
    }

    private void StartDrinkTimer()
    {
        // Stop any running coroutines safely
        if (drinkCoroutine != null) StopCoroutine(drinkCoroutine);
        if (betweenCoroutine != null) StopCoroutine(betweenCoroutine);

        // Choose and display a new drink
        PickNewTargetDrink();
        isDrinkActive = true;

        // Start the drink countdown
        drinkCoroutine = StartCoroutine(DrinkTimerCoroutine());
    }

    private IEnumerator DrinkTimerCoroutine()
    {
        float timer = drinkDisplayTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        // Drink timer finished — hide and schedule next between timer
        isDrinkActive = false;
        SetColorRecursively(gameObject, new Color(0, 0, 0, 0));
        drinkCoroutine = null;

        // Start the waiting timer for the next spawn
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

        // Apply the drink color with full alpha
        Color colorToShow = possibleDrinks[randomIndex].displayColor;
        colorToShow.a = 1f;
        SetColorRecursively(gameObject, colorToShow);

        Debug.Log("New Order: " + currentTargetDrinkID);
    }

    public void EndDrinkEarly()
    {
        // Called when player submits early or you need to cancel.
        // Stops current drink and restarts the between timer.
        if (drinkCoroutine != null)
        {
            StopCoroutine(drinkCoroutine);
            drinkCoroutine = null;
        }

        isDrinkActive = false;
        SetColorRecursively(gameObject, new Color(0, 0, 0, 0));

        // Restart the between timer
        StartBetweenTimer();
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