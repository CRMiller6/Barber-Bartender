using UnityEngine;

public class GhostTriggerActivator : MonoBehaviour
{
    public GameObject targetObject; // Assign in Inspector
    public GameObject targetObject2; // Assign in Inspector


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ghost"))
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                targetObject2.SetActive(true);
            }
        }
    }
}