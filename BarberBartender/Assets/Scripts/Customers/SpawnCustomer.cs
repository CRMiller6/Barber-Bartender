using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    public DrinkOrderManager drinkManager;
    public FlavorPoints flavorPointsInstance;

    [Header("Prefab")]
    public GameObject customerPrefab;

    [Header("Doors (spawn & exit)")]
    public Transform doorA;
    public Transform doorB;

    [Header("Stop Positions")]
    public Transform rightStop;

    [Header("Spawn Delay (randomized)")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;

    [Header("Max Points Threshold")]
    public int maxDrinkPoints = 50;

    [Header("Behavior")]
    public bool stopSpawningAtMaxPoints = true; // stop spawning when max points reached

    [Header("Customer Sprites")]
    public Sprite[] possibleCustomerSprites; // assign in inspector

    private CustomerBehavior currentCustomer;
    private bool previousDrinkActive = false;
    private bool spawnScheduled = false;

    void Update()
    {
        if (drinkManager == null) return;

        if (drinkManager.ReadyToSpawn && currentCustomer == null && !spawnScheduled)
        {
            if (stopSpawningAtMaxPoints && flavorPointsInstance != null &&
                flavorPointsInstance.CurrentScore >= maxDrinkPoints)
                return;

            spawnScheduled = true;
            StartCoroutine(SpawnWithRandomDelay());
        }

        if (currentCustomer != null)
        {
            bool nowActive = drinkManager.IsDrinkActive;
            if (previousDrinkActive && !nowActive)
            {
                currentCustomer.Leave();
                currentCustomer = null;
            }
            previousDrinkActive = nowActive;
        }
        else
        {
            previousDrinkActive = drinkManager.IsDrinkActive;
        }
    }

    private IEnumerator SpawnWithRandomDelay()
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(delay);
        SpawnDrinkCustomer();
        spawnScheduled = false;
    }

    void SpawnDrinkCustomer()
    {
        Transform spawnDoor = Random.value > 0.5f ? doorA : doorB;
        Transform exitDoor = Random.value > 0.5f ? doorA : doorB;

        GameObject obj = Instantiate(customerPrefab, spawnDoor.position, Quaternion.identity);
        currentCustomer = obj.GetComponent<CustomerBehavior>();
        currentCustomer.Initialize(spawnDoor.position, rightStop.position, exitDoor.position, drinkManager);

        // Assign a random sprite
        if (possibleCustomerSprites != null && possibleCustomerSprites.Length > 0)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.sprite = possibleCustomerSprites[Random.Range(0, possibleCustomerSprites.Length)];
        }

        previousDrinkActive = drinkManager.IsDrinkActive;
    }
}