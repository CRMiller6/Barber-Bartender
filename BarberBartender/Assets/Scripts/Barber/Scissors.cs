using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Scissors : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Something hit: " + other.name);
        // Check if the object we hit has the "Cutable" tag
        if (other.CompareTag("Cutable"))
        {
            // Destroy the object
            Destroy(other.gameObject);
        }
    }
}