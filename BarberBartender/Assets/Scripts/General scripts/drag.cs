using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Drag : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 dragOffset;
    public bool IsDragging => isDragging;

    private enum DragMode
    {
        Snap,
        SmoothDrag, // uses dragStrength (Lerp)
        MaxSpeed    // uses speed cap
    }

    [Header("Drag Settings")]
    [SerializeField] private DragMode dragMode = DragMode.SmoothDrag;

    [Header("Smooth Drag")]
    [SerializeField] private float dragStrength = 10f;

    [Header("Max Speed Drag")]
    [SerializeField] private float maxSpeed = 15f;

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

        switch (dragMode)
        {
            case DragMode.Snap:
                rb.MovePosition(targetPos);
                break;

            case DragMode.SmoothDrag:
                Vector2 smoothPos = Vector2.Lerp(rb.position, targetPos, dragStrength * Time.fixedDeltaTime);
                rb.MovePosition(smoothPos);
                break;

            case DragMode.MaxSpeed:
                Vector2 direction = targetPos - (Vector3)rb.position;
                Vector2 move = Vector2.ClampMagnitude(direction, maxSpeed * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + move);
                break;
        }
    }
}