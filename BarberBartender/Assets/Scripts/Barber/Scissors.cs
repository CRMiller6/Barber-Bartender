using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Scissors : MonoBehaviour
{
    private void OnCollisionEnter2D (Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Cutable"))
        {
            Destroy(collision2D.gameObject);
            Debug.Log("Object Cut!");
        }
    }
}
