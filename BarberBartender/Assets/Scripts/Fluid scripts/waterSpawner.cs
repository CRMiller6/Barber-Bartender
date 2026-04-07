using UnityEngine;

[RequireComponent(typeof(Transform))]
public sealed class waterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private int spawnCount = 1;
    [SerializeField] private float minSpawnInterval = 0.2f;
    [SerializeField] private float maxSpawnInterval = 1.2f;

    [Header("Tilt Thresholds (Degrees)")]
    [SerializeField] private float uprightThreshold = 45f;

    private Transform parentTransform;
    private float spawnTimer;
    private Drag dragScript; // Reference to your drag script

    // Cached constants for smoothstep
    private float maxDistance;
    private float invMaxDistance;

    private void Awake()
    {
        parentTransform = transform.parent;

        if (parentTransform == null)
        {
            Debug.LogWarning("waterSpawner requires a parent with a Transform.");
            return;
        }

        dragScript = parentTransform.GetComponent<Drag>();
        if (dragScript == null)
        {
            Debug.LogWarning("Parent does not have a Drag component!");
        }

        // Precompute constants
        maxDistance = 180f - uprightThreshold;
        invMaxDistance = 1f / maxDistance;
    }

    private void Update()
    {
        if (parentTransform == null || dragScript == null)
            return;

        // Only spawn while dragging
        if (!dragScript.IsDragging)
        {
            spawnTimer = 0f;
            return;
        }

        // Get the parent's Z rotation
        float z = parentTransform.eulerAngles.z;
        z = z > 180f ? z - 360f : z; // Convert 0-360 to -180..180

        float absZ = Mathf.Abs(z);

        // Inside upright range -- no spawn
        if (absZ <= uprightThreshold)
        {
            spawnTimer = 0f;
            return;
        }

        spawnTimer += Time.deltaTime;

        // Distance from fully upside-down
        float distance = absZ >= 180f ? 0f : 180f - absZ;

        // Normalize [0..1]
        float t = Mathf.Clamp01(distance * invMaxDistance);

        // SmoothStep (faster inline version)
        t = t * t * (3f - 2f * t);

        float spawnInterval = minSpawnInterval + (maxSpawnInterval - minSpawnInterval) * t;

        if (spawnTimer >= spawnInterval)
        {
            SpawnPrefabs();
            spawnTimer = 0f;
        }
    }

    private void SpawnPrefabs()
    {
        Vector3 pos = transform.position;

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(prefabToSpawn, pos, Quaternion.identity);
        }
    }
}