using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Drag : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isDragging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.freezeRotation = true;

        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        rb.freezeRotation = false;
    }

    private void FixedUpdate()
    {
        if (!isDragging) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(new Vector2(mousePos.x, mousePos.y));
    }
}
