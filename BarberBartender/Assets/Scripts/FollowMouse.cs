using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        // 1. Get mouse position in screen pixels
        Vector3 mousePos = Input.mousePosition;

        // 2. Convert to World Space using the Main Camera
        // Note: Z must be set to the distance from the camera (usually its Z depth)
        mousePos.z = -Camera.main.transform.position.z; 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 3. Apply position (keeping Z at 0 for 2D)
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);
    }
}
