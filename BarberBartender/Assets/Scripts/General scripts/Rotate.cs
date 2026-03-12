using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class ThrowAndRotate : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;

    private Vector2 lastMousePosition;
    private Vector2 throwVelocity;
    private bool isDragging;

    [SerializeField] private float throwMultiplier = 4f;
    [SerializeField] private float rotateSpeed = 180f;

    [Header("Rotation Lock")]
    [SerializeField] private bool lockZRotation = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        lastMousePosition = GetMouseWorldPosition();
        throwVelocity = Vector2.zero;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        rb.linearVelocity = throwVelocity * throwMultiplier;
    }

    private void Update()
    {
        if (!isDragging) return;

        Vector2 mousePos = GetMouseWorldPosition();
        throwVelocity = (mousePos - lastMousePosition) / Time.deltaTime;
        lastMousePosition = mousePos;
    }

    private void FixedUpdate()
    {
        if (!isDragging) return;

        if (!lockZRotation)
        {
            float rotationInput = GetRotationInput();
            if (rotationInput != 0f)
            {
                rb.MoveRotation(rb.rotation + rotationInput * rotateSpeed * Time.fixedDeltaTime);
            }
        }

        // Lock Z rotation in Rigidbody if needed
        rb.freezeRotation = lockZRotation;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(pos.x, pos.y);
    }

    private static float GetRotationInput()
    {
        if (Input.GetKey(KeyCode.Q)) return 1f;
        if (Input.GetKey(KeyCode.E)) return -1f;
        return 0f;
    }
}