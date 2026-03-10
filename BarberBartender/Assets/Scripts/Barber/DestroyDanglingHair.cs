using UnityEngine;

public class DestroyDanglingHair : MonoBehaviour
{
    public TargetScript target;

    void OnEnable()
    {
        if (target != null)
        {
            // Subscribe to your existing event
            target.OnTargetDestroyed += HandleDeletion;
        }
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks if this object is disabled first
        if (target != null)
        {
            target.OnTargetDestroyed -= HandleDeletion;
        }
    }

    private void HandleDeletion()
    {
        // Delete this dependent object
        Destroy(gameObject);
    }
}
