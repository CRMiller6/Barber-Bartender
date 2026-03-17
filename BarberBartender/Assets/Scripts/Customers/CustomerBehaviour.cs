using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomerBehavior : MonoBehaviour
{
    public DrinkOrderManager drinkManager;

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

    [Header("Z-Position Bounds")]
    public float minZ = 0f;   // smallest scale = farthest back
    public float maxZ = -1f;  // largest scale = closest

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
        UpdateFlip();
    }

    void Update()
    {
        if (walkingIn) WalkToStop();
        else if (walkingOut) WalkAway();
    }

    void WalkToStop()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;
        transform.position = Vector3.MoveTowards(transform.position, stopPosition, walkSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(bounceTimer) * bounceHeight;
        transform.position = pos;

        float total = Mathf.Abs(stopPosition.x - spawnPosition.x);
        float t = total > 0.0001f ? 1f - Mathf.Abs(stopPosition.x - transform.position.x) / total : 0f;
        t = Mathf.Clamp01(t);

        float scale = Mathf.Lerp(startScale, endScale, t);
        transform.localScale = Vector3.one * scale;

        // Dynamic Z based on scale
        float zT = (scale - startScale) / (endScale - startScale);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(minZ, maxZ, zT));

        UpdateFlip();

        if (Mathf.Abs(transform.position.x - stopPosition.x) < 0.05f && !notifiedManager)
        {
            notifiedManager = true;
            walkingIn = false;
            drinkManager.CustomerArrivedAtCounter();
        }
    }

    public void Leave()
    {
        walkingOut = true;
        bounceTimer = 0f;
        UpdateFlip();
    }

    void WalkAway()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;
        transform.position = Vector3.MoveTowards(transform.position, exitPosition, walkSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(bounceTimer) * bounceHeight;
        transform.position = pos;

        float total = Mathf.Abs(exitPosition.x - stopPosition.x);
        float t = total > 0.0001f ? 1f - Mathf.Abs(exitPosition.x - transform.position.x) / total : 1f;
        t = Mathf.Clamp01(t);

        float scale = Mathf.Lerp(endScale, startScale, t);
        transform.localScale = Vector3.one * scale;

        // Dynamic Z based on scale
        float zT = (scale - startScale) / (endScale - startScale);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(minZ, maxZ, zT));

        UpdateFlip();

        if (Vector3.Distance(transform.position, exitPosition) < 0.05f)
            Destroy(gameObject);
    }

    private void UpdateFlip()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        float targetX = walkingOut ? exitPosition.x : (walkingIn ? stopPosition.x : transform.position.x);
        if (Mathf.Abs(targetX - transform.position.x) > 0.01f)
        {
            bool facingRight = (targetX - transform.position.x) > 0f;
            spriteRenderer.flipX = !facingRight;
        }
    }
}