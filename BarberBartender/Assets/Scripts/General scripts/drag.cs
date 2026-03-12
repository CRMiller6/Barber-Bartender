using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Drag : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 dragOffset; // Offset between mouse and object center
    public bool IsDragging => isDragging;

    [Header("Drag Settings")]
    [SerializeField] private bool useDrag = true; // Enable drag effect
    [SerializeField] private float dragStrength = 10f; // How strongly it follows the mouse

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

        if (useDrag)
        {
            // Move smoothly toward the target position
            Vector2 newPos = Vector2.Lerp(rb.position, targetPos, dragStrength * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else
        {
            // Snap directly
            rb.MovePosition(targetPos);
        }
    }
}