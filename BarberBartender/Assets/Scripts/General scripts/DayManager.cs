using UnityEngine;
using System.Collections;

public class DayCycleManager : MonoBehaviour
{
    [Header("Day Settings")]
    public float dayDuration = 180f;
    public float morningDuration = 60f;
    public float rushDuration = 60f;
    public float nightDuration = 60f;

    [Header("References")]
    public CustomerSpawner drinkSpawner;
    public HairCustomerSpawner hairSpawner;
    public GameWinManager winManager; // reference to save results at end of day

    [Header("Lighting")]
    public Transform visualChild;        
    public Transform visualChild2;        

    public Color nightColor = new Color(0.1f, 0.1f, 0.3f);
    public Color dayColor = Color.white;

    private float currentTime;
    private bool dayRunning = false;
    private SpriteRenderer sr;
    private SpriteRenderer sr2;

    void Start()
    {
        if (visualChild != null)
            sr = visualChild.GetComponent<SpriteRenderer>();
        if (visualChild2 != null)
            sr2 = visualChild2.GetComponent<SpriteRenderer>();


        StartDay();
    }

    void Update()
    {
        if (!dayRunning) return;

        currentTime += Time.deltaTime;
        UpdateLighting();

        if (currentTime >= dayDuration)
            EndDay();
    }

    public void StartDay()
    {
        currentTime = 0f;
        dayRunning = true;

        if (hairSpawner != null && hairSpawner.autoSpawn)
            hairSpawner.StartCoroutine("AutoSpawnRoutine");

        StartCoroutine(DayRoutine());
        Debug.Log("Day Started");
    }

    private IEnumerator DayRoutine()
    {
        // Morning Phase
        Debug.Log("Morning Phase");
        SetSpawnRates(1f, 5f);
        yield return new WaitForSeconds(morningDuration);

        // Rush Hour Phase
        Debug.Log("Rush Hour!");
        SetSpawnRates(0.5f, 2f);
        yield return new WaitForSeconds(rushDuration);

        // Night Phase
        Debug.Log("Night Phase");
        SetSpawnRates(2f, 6f);
        yield return new WaitForSeconds(nightDuration);

        EndDay();
    }

    private void SetSpawnRates(float min, float max)
    {
        if (drinkSpawner != null)
        {
            drinkSpawner.minSpawnDelay = min;
            drinkSpawner.maxSpawnDelay = max;
        }

        if (hairSpawner != null)
        {
            hairSpawner.minSpawnDelay = min;
            hairSpawner.maxSpawnDelay = max;
        }
    }

    private void UpdateLighting()
    {
        if (sr == null) return;

        float halfDay = dayDuration / 2f;
        float t = currentTime <= halfDay
            ? currentTime / halfDay
            : 1f - ((currentTime - halfDay) / halfDay);

        t = Mathf.SmoothStep(0f, 1f, t);
        sr.color = Color.Lerp(nightColor, dayColor, t);
        sr2.color = Color.Lerp(nightColor, dayColor, t);

    }

    private void EndDay()
    {
        if (!dayRunning) return;
        dayRunning = false;

        if (hairSpawner != null)
            hairSpawner.StopAutoSpawn();

        Debug.Log("Day Ended");

        // Save results at end of day
        if (winManager != null)
            winManager.SaveResults();

        // GameWinManager will handle scene transition
    }
}