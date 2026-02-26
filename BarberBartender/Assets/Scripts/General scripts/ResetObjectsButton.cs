using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class ResetObjectsButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Objects To Reset")]
    public List<GameObject> objectsToReset = new List<GameObject>();

    // Internal storage of original transforms
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();

    private void Start()
    {
        // Cache original positions and rotations
        foreach (GameObject obj in objectsToReset)
        {
            if (obj == null) continue;

            originalPositions[obj] = obj.transform.position;
            originalRotations[obj] = obj.transform.rotation;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetAllObjects();
    }

    private void ResetAllObjects()
    {
        foreach (GameObject obj in objectsToReset)
        {
            if (obj == null) continue;

            // Reset physics safely if it has Rigidbody2D
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;

                rb.position = originalPositions[obj];
                rb.rotation = originalRotations[obj].eulerAngles.z;
            }
            else
            {
                // Non-physics object
                obj.transform.position = originalPositions[obj];
                obj.transform.rotation = originalRotations[obj];
            }
        }

        Debug.Log("All objects reset.");
    }
}