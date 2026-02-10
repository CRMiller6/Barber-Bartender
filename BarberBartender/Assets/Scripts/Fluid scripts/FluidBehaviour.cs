using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public sealed class FluidBehavior : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 4f;

    [Header("Fluid Settings")]
    [SerializeField] private float stickDistance = 0.3f;
    [SerializeField] private float stickStrength = 5f;
    [SerializeField] private float maxVelocity = 5f;
    [SerializeField] private int minConnections = 4;

    private Rigidbody2D rb;
    private Collider2D col;
    private int instanceId;
    private int updateOffset;

    private float lifeTimer;
    private bool lifetimePaused;

    private static readonly Collider2D[] overlapBuffer = new Collider2D[12];
    private static ContactFilter2D contactFilter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        instanceId = GetInstanceID();

        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;

        updateOffset = instanceId & 1;

        contactFilter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = true
        };
    }

    private void Update()
    {
        if (lifetimePaused)
            return;

        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifetime)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if ((Time.frameCount & 1) != updateOffset)
            return;

        int hitCount = Physics2D.OverlapCircle(
            rb.position,
            stickDistance,
            contactFilter,
            overlapBuffer
        );

        if (hitCount == 0)
            return;

        float stickDistSqr = stickDistance * stickDistance;
        Vector2 velocity = rb.linearVelocity;

        int connections = 0;
        Vector2 cohesionSum = Vector2.zero;

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D otherCol = overlapBuffer[i];
            if (otherCol == col) continue;

            if (!otherCol.TryGetComponent(out FluidBehavior other))
                continue;

            if (other.instanceId <= instanceId)
                continue;

            Vector2 dir = other.rb.position - rb.position;
            float distSqr = dir.sqrMagnitude;

            if (distSqr <= 0f || distSqr >= stickDistSqr)
                continue;

            connections++;
            cohesionSum += dir;

            float dist = Mathf.Sqrt(distSqr);
            float forceScale = (stickDistance - dist) * stickStrength / dist;
            Vector2 force = dir * forceScale * Time.fixedDeltaTime;

            velocity += force;
            other.rb.linearVelocity -= force;
        }

        if (connections < minConnections && cohesionSum != Vector2.zero)
        {
            velocity +=
                cohesionSum.normalized *
                stickStrength *
                0.4f *
                Time.fixedDeltaTime;
        }

        rb.linearVelocity = Vector2.ClampMagnitude(velocity, maxVelocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("cup"))
        {
            lifetimePaused = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("cup"))
        {
            lifetimePaused = false;
        }
    }
}
