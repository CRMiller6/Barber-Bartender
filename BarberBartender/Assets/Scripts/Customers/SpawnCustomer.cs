using UnityEngine;

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

    private CustomerBehavior currentCustomer;
    private bool previousDrinkActive = false;

    void Update()
    {
        if (drinkManager == null) return;

        // Spawn when manager indicates ready and no current customer exists
        if (drinkManager.ReadyToSpawn && currentCustomer == null)
        {
            SpawnDrinkCustomer();
        }

        // Watch the drink active flag: when it transitions from true -> false, instruct customer to leave
        if (currentCustomer != null)
        {
            bool nowActive = drinkManager.IsDrinkActive;

            if (previousDrinkActive && !nowActive)
            {
                // drink just finished -> tell customer to leave
                currentCustomer.Leave();
                currentCustomer = null;
            }

            previousDrinkActive = nowActive;
        }
        else
        {
            // keep previous state synced when no customer
            previousDrinkActive = drinkManager.IsDrinkActive;
        }
    }

    void SpawnDrinkCustomer()
    {
        Transform spawnDoor = Random.value > 0.5f ? doorA : doorB;
        Transform exitDoor = Random.value > 0.5f ? doorA : doorB;

        GameObject obj = Instantiate(customerPrefab, spawnDoor.position, Quaternion.identity);
        currentCustomer = obj.GetComponent<CustomerBehavior>();

        // Initialize with spawn, stop (rightStop), and the chosen exit position.
        currentCustomer.Initialize(spawnDoor.position, rightStop.position, exitDoor.position, drinkManager);

        // After spawn, the DrinkOrderManager remains in ReadyToSpawn==true until CustomerArrivedAtCounter() consumes it,
        // which happens when the customer reaches rightStop. previousDrinkActive should be synced so we detect drink start.
        previousDrinkActive = drinkManager.IsDrinkActive;
    }
}