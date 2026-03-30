using UnityEngine;

public class WetHair : MonoBehaviour
{
    void ScanForDetectors(Vector3 center, float radius)
{
    // 1. Get every collider inside the sphere
    Collider[] hitColliders = Physics.OverlapSphere(center, radius);

    foreach (var hitCollider in hitColliders)
    {
        // 2. See if this specific object has your script
        if (hitCollider.TryGetComponent<CollisionDetectorForPrefabs>(out var detector))
        {
            Debug.Log($"Found detector on: {hitCollider.gameObject.name}");

            // 3. Optional: You can manually assign or trigger the OnHit action here
            detector.OnHit = (col) => {
                Debug.Log("Manually triggered logic for " + col.gameObject.name);
            };
        }
    }
}
}
