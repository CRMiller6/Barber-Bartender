using UnityEngine;

public class MoveToZero : MonoBehaviour
{
    //I know we already have a reset object script but also i am not using a rigidbody for this
    //also i know it is called move to 0 but it doesnt do that lol it moves it to whatever the script is on

    public GameObject targetGameObject;

    public void OnClick()
    {
        targetGameObject.transform.position = transform.position;
    }



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
