using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SprayTool : MonoBehaviour
{
    public bool isSpraying = true;
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Touching: " + other.name); 
        
        if (other.TryGetComponent<HairData>(out var hair))
        {
            if (!hair.isWet)
            {
                hair.SetWet();
            }            
        }
    }
}