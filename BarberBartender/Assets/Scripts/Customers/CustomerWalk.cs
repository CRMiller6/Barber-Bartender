using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomerBehavior : MonoBehaviour
{
    public enum CustomerType { DrinkCustomer } // kept flexible for future extension

    [Header("References")]
    public DrinkOrderManager drinkManager; // set by spawner when instantiating

    private Vector3 spawnPosition;
    private Vector3 stopPosition;
    private Vector3 exitPosition;

    [Header("Movement")]
    public float walkSpeed = 2f;

    [Header("Bounce")]
    public float bounceHeight = 0.15f;
    public float bounceSpeed = 8f;

    [Header("Scaling")]
    public float startScale = 0.5f;
    public float endScale = 1f;

    private float bounceTimer;
    private bool walkingIn = true;
    private bool walkingOut = false;
    private bool notifiedManager = false;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Vector3 spawn, Vector3 stop, Vector3 exit, DrinkOrderManager manager)
    {
        spawnPosition = spawn;
        stopPosition = stop;
        exitPosition = exit;
        drinkManager = manager;

        transform.position = spawnPosition;
        transform.localScale = Vector3.one * startScale;

        walkingIn = true;
        walkingOut = false;
        notifiedManager = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateFlip(); // flip sprite to face movement direction
    }

    void Update()
    {
        if (walkingIn)
            WalkToStop();
        else if (walkingOut)
            WalkAway();
        // else idle at stop, waiting for spawner to call Leave()
    }

    void WalkToStop()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;

        transform.position = Vector3.MoveTowards(transform.position, stopPosition, walkSpeed * Time.deltaTime);

        // Bounce (affects only Y)
        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(bounceTimer) * bounceHeight;
        transform.position = pos;

        // Scale based ONLY on X movement (no Y influence)
        float total = Mathf.Abs(stopPosition.x - spawnPosition.x);
        // protect divide-by-zero
        float t = 0f;
        if (total > 0.0001f)
        {
            float current = Mathf.Abs(stopPosition.x - transform.position.x);
            t = 1f - (current / total);
            t = Mathf.Clamp01(t);
        }

        float scale = Mathf.Lerp(startScale, endScale, t);
        transform.localScale = Vector3.one * scale;

        UpdateFlip();

        if (Mathf.Abs(transform.position.x - stopPosition.x) < 0.05f)
        {
            walkingIn = false;

            // Notify drink manager we arrived (only once)
            if (!notifiedManager && drinkManager != null)
            {
                notifiedManager = true;
                drinkManager.CustomerArrivedAtCounter();
            }
        }
    }

    public void Leave()
    {
        // Determine facing based on exit direction
        walkingOut = true;
        UpdateFlip();
    }

    void WalkAway()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;

        transform.position = Vector3.MoveTowards(transform.position, exitPosition, walkSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(bounceTimer) * bounceHeight;
        transform.position = pos;

        // Scale based ONLY on X movement from stop to exit
        float total = Mathf.Abs(exitPosition.x - stopPosition.x);
        float t = 0f;
        if (total > 0.0001f)
        {
            float current = Mathf.Abs(exitPosition.x - transform.position.x);
            t = 1f - (current / total);
            t = Mathf.Clamp01(t);
        }

        float scale = Mathf.Lerp(endScale, startScale, t);
        transform.localScale = Vector3.one * scale;

        UpdateFlip();

        if (Mathf.Abs(transform.position.x - exitPosition.x) < 0.05f)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateFlip()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // Face right if moving to greater X, left otherwise
        float targetX = walkingOut ? exitPosition.x : (walkingIn ? stopPosition.x : transform.position.x);
        if (Mathf.Abs(targetX - transform.position.x) > 0.01f)
        {
            bool facingRight = (targetX - transform.position.x) > 0f;
            spriteRenderer.flipX = !facingRight; // flipX semantics depend on your art; adjust as needed
        }
    }
}