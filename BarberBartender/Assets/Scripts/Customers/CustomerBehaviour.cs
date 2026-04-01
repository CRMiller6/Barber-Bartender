using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomerBehavior : MonoBehaviour
{
    public DrinkOrderManager drinkManager;
    public GameObject submitZone; 
    private BoxCollider2D submitZoneCollider;

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
    public float minZ = 0f;  
    public float maxZ = -1f;  

    [Header("Stop Y Position")]
    public float stopY = -19f; 

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

        if (submitZone == null)
            submitZone = GameObject.FindWithTag("SubmitZone");

        if (submitZone != null)
        {
            submitZoneCollider = submitZone.GetComponent<BoxCollider2D>();
            if (submitZoneCollider != null)
                submitZoneCollider.enabled = false;
        }

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
        // Horizontal movement
        Vector3 basePos = Vector3.MoveTowards(transform.position, stopPosition, walkSpeed * Time.deltaTime);

        float totalX = Mathf.Abs(stopPosition.x - spawnPosition.x);
        float t = totalX > 0.0001f ? 1f - Mathf.Abs(stopPosition.x - basePos.x) / totalX : 0f;
        t = Mathf.Clamp01(t);

        float scale = Mathf.Lerp(startScale, endScale, t);

        Vector3 visualPos = basePos;

        // If reached stop X, lock Y exactly at stopY
        if (Mathf.Abs(basePos.x - stopPosition.x) < 0.05f)
        {
            if (!notifiedManager)
            {
                notifiedManager = true;
                walkingIn = false;
                drinkManager.CustomerArrivedAtCounter();

                if (submitZoneCollider != null)
                    submitZoneCollider.enabled = true;
            }

            visualPos.y = stopY; // <- lock Y exactly at -19
        }
        else
        {
            // Normal bounce while walking
            bounceTimer += Time.deltaTime * bounceSpeed;
            visualPos.y += Mathf.Sin(bounceTimer) * bounceHeight;
        }

        float zT = (scale - startScale) / (endScale - startScale);
        visualPos.z = Mathf.Lerp(minZ, maxZ, zT);

        transform.position = visualPos;
        transform.localScale = Vector3.one * scale;

        UpdateFlip();
    }

    public void Leave()
    {
        walkingOut = true;
        bounceTimer = 0f;
        UpdateFlip();

        if (submitZoneCollider != null)
            submitZoneCollider.enabled = false;
    }

    void WalkAway()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;

        Vector3 basePos = Vector3.MoveTowards(transform.position, exitPosition, walkSpeed * Time.deltaTime);

        float totalX = Mathf.Abs(exitPosition.x - stopPosition.x);
        float t = totalX > 0.0001f ? 1f - Mathf.Abs(exitPosition.x - basePos.x) / totalX : 1f;
        t = Mathf.Clamp01(t);

        float scale = Mathf.Lerp(endScale, startScale, t);

        Vector3 visualPos = basePos;
        // Normal bounce while leaving
        visualPos.y += Mathf.Sin(bounceTimer) * bounceHeight;

        float zT = (scale - startScale) / (endScale - startScale);
        visualPos.z = Mathf.Lerp(minZ, maxZ, zT);

        transform.position = visualPos;
        transform.localScale = Vector3.one * scale;

        UpdateFlip();

        if (Vector3.Distance(new Vector3(basePos.x, basePos.y, 0f),
                             new Vector3(exitPosition.x, exitPosition.y, 0f)) < 1.5f)
        {
            Destroy(gameObject);
        }
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