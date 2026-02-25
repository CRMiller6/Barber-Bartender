using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Drag : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 dragOffset; // Offset between mouse and object center
    public bool IsDragging => isDragging;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        // Stop any current movement
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.freezeRotation = true;

        // Calculate offset
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        dragOffset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);

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
        Vector3 targetPos = new Vector3(mousePos.x, mousePos.y, transform.position.z) + dragOffset;
        rb.MovePosition(targetPos);
    }
}
