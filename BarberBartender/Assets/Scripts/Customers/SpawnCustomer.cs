using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    public DrinkOrderManager drinkManager;

    [Header("Prefab")]
    public GameObject customerPrefab;

    [Header("Doors (spawn & exit)")]
    public Transform doorA;
    public Transform doorB;

    [Header("Stop Positions")]
    public Transform rightStop;  // Drink customers always go here

    [Header("Spawn Delay (randomized)")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;

    private CustomerBehavior currentCustomer;
    private bool previousDrinkActive = false;
    private bool spawnScheduled = false;

    void Update()
    {
        if (drinkManager == null) return;

        // Schedule spawn if manager is ready and no customer exists
        if (drinkManager.ReadyToSpawn && currentCustomer == null && !spawnScheduled)
        {
            spawnScheduled = true;
            StartCoroutine(SpawnWithRandomDelay());
        }

        // Watch drink active flag: when it transitions from true -> false, instruct customer to leave
        if (currentCustomer != null)
        {
            bool nowActive = drinkManager.IsDrinkActive;

            if (previousDrinkActive && !nowActive)
            {
                // Drink just finished -> tell customer to leave
                currentCustomer.Leave();
                currentCustomer = null;
            }

            previousDrinkActive = nowActive;
        }
        else
        {
            // Keep previous state synced when no customer exists
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

        // Initialize with spawn, stop (rightStop), and exit
        currentCustomer.Initialize(spawnDoor.position, rightStop.position, exitDoor.position, drinkManager);

        previousDrinkActive = drinkManager.IsDrinkActive;
    }
}