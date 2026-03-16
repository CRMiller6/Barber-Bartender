using UnityEngine;
using System.Collections;

public class HairCustomerSpawner : MonoBehaviour
{
    public GameObject hairCustomerPrefab;
    public Transform doorA;
    public Transform doorB;
    public Transform leftStop;
    public HairSpawnBridge hairBridge;

    [Header("Spawn Delay (randomized)")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;

    [Header("Max Points Threshold")]
    public int maxHairPoints = 50;

    [Header("Behavior")]
    public bool stopSpawningAtMaxPoints = true; //stop spawning when max points reached

    public bool autoSpawn = false;

    private HairCustomerBehavior currentHairCustomer;
    private bool spawnScheduled = false;
    private Coroutine autoSpawnCoroutine;

    void Start()
    {
        if (autoSpawn)
            autoSpawnCoroutine = StartCoroutine(AutoSpawnRoutine());
    }

    public void SpawnNow()
    {
        if (currentHairCustomer != null) return;

        // Stop spawn if max points reached AND stopSpawningAtMaxPoints is true
        if (stopSpawningAtMaxPoints && hairBridge != null && hairBridge.spawnHairStyleRef.totalPoints >= maxHairPoints)
            return;

        Transform spawnDoor = Random.value > 0.5f ? doorA : doorB;
        Transform exitDoor = Random.value > 0.5f ? doorA : doorB;

        GameObject obj = Instantiate(hairCustomerPrefab, spawnDoor.position, Quaternion.identity);
        currentHairCustomer = obj.GetComponent<HairCustomerBehavior>();
        currentHairCustomer.Initialize(spawnDoor.position, leftStop.position, exitDoor.position, hairBridge);

        StartCoroutine(WatchCustomerLife(obj));
    }

    private IEnumerator WatchCustomerLife(GameObject customerObj)
    {
        while (customerObj != null)
            yield return null;

        currentHairCustomer = null;
    }

    private IEnumerator AutoSpawnRoutine()
    {
        while (true)
        {
            if (currentHairCustomer == null && !spawnScheduled)
            {
                spawnScheduled = true;

                // Stop spawn if max points reached AND stopSpawningAtMaxPoints is true
                if (stopSpawningAtMaxPoints && hairBridge != null && hairBridge.spawnHairStyleRef.totalPoints >= maxHairPoints)
                {
                    spawnScheduled = false;
                    yield return null;
                    continue;
                }

                float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
                yield return new WaitForSeconds(delay);
                SpawnNow();
                spawnScheduled = false;
            }

            yield return null;
        }
    }

    public void StopAutoSpawn()
    {
        if (autoSpawnCoroutine != null)
        {
            StopCoroutine(autoSpawnCoroutine);
            autoSpawnCoroutine = null;
        }
    }
}