using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Scissors : MonoBehaviour
{
    public GameObject startingPoint;

    void Start()
    {
        
    }

    void Update()
    {
        PickedUp();
    }

    public void PickedUp()
    {
        if (Input.GetMouseButton(0)) 
        {
            Debug.Log("Left mouse button is being held!");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            transform.position = mousePos;
        }
        else if (Input.GetMouseButtonUP(0))
        {
            Vector3 goHome = startingPoint;
            transform.position = goHome;
        }
    }
}
