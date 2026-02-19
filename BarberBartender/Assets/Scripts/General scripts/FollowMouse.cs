using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float edgeThreshold = 50f; // Pixels from edge to trigger movement
    public float moveSpeed = 5f;      // Units per second
    
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // Check if mouse is near any of the four edges
        bool isNearEdge = mousePos.x < edgeThreshold || 
                          mousePos.x > Screen.width - edgeThreshold || 
                          mousePos.y < edgeThreshold || 
                          mousePos.y > Screen.height - edgeThreshold;

        if (isNearEdge)
        {
            // Calculate target world position
            mousePos.z = Mathf.Abs(_mainCamera.transform.position.z);
            Vector3 targetWorldPos = _mainCamera.ScreenToWorldPoint(mousePos);
            targetWorldPos.z = 0; // Lock to 2D plane

            // Move towards target smoothly at a constant speed
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetWorldPos, 
                moveSpeed * Time.deltaTime
            );
        }
    }
}