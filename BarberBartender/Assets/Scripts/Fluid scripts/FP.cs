using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DrinkDeleteButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Zone to check for drinks")]
    public Collider2D zoneCollider;

    [Header("Cup Settings")]
    public string cupTag = "cup";

    [Header("Drink Definitions")]
    public DrinkDefinition[] possibleDrinks;

    [Header("Scoring")]
    public int correctPoints = 2;
    public int wrongPoints = -1;

    [Header("Reset Other Object")]
    public GameObject objectToReset; // Public object to teleport back
    private Vector3 objectStartPosition;
    private Quaternion objectStartRotation;

    public int currentScore = 0;

    private string currentTargetDrinkID;
    private SpriteRenderer spriteRenderer;

    private Vector3 cupStartPosition;
    private Quaternion cupStartRotation;
    private Rigidbody2D cupRb;

    [System.Serializable]
    public struct DrinkDefinition
    {
        public string objectID;
        public Color displayColor;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Cache cup starting transform
        GameObject cup = GameObject.FindGameObjectWithTag(cupTag);
        if (cup != null)
        {
            cupStartPosition = cup.transform.position;
            cupStartRotation = cup.transform.rotation;
            cupRb = cup.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("No object with tag '" + cupTag + "' found in scene.");
        }

        // Cache starting transform of the other object
        if (objectToReset != null)
        {
            objectStartPosition = objectToReset.transform.position;
            objectStartRotation = objectToReset.transform.rotation;
        }

        PickNewTargetDrink();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zoneCollider == null)
        {
            Debug.LogWarning("Zone Collider not assigned!");
            return;
        }

        HandleSubmission();
        ResetCup();
        ResetOtherObject(); // Reset the public object
        PickNewTargetDrink();
    }

    private void HandleSubmission()
    {
        Collider2D[] overlapping = Physics2D.OverlapBoxAll(
            zoneCollider.bounds.center,
            zoneCollider.bounds.size,
            0f
        );

        string submittedDrinkID = null;

        // Find first drink in zone
        foreach (var col in overlapping)
        {
            if (col.CompareTag("Drink"))
            {
                Mixable mixable = col.GetComponent<Mixable>();
                if (mixable != null)
                {
                    submittedDrinkID = mixable.objectID;
                }
                break;
            }
        }

        // Score logic
        if (submittedDrinkID != null)
        {
            if (submittedDrinkID == currentTargetDrinkID)
            {
                currentScore += correctPoints;
                Debug.Log($"Correct! +{correctPoints} points.");
            }
            else
            {
                currentScore += wrongPoints;
                Debug.Log($"Wrong drink! {wrongPoints} points.");
            }
        }
        else
        {
            Debug.Log("No drink submitted.");
        }

        Debug.Log("Current Score: " + currentScore);

        // Delete all drinks in zone
        foreach (var col in overlapping)
        {
            if (col.CompareTag("Drink"))
            {
                Destroy(col.gameObject);
            }
        }
    }

    private void PickNewTargetDrink()
    {
        if (possibleDrinks == null || possibleDrinks.Length == 0)
        {
            Debug.LogWarning("No drinks assigned!");
            return;
        }

        int randomIndex = Random.Range(0, possibleDrinks.Length);
        currentTargetDrinkID = possibleDrinks[randomIndex].objectID;

        // Change color of root and all children
        SetColorRecursively(spriteRenderer.gameObject, possibleDrinks[randomIndex].displayColor);

        Debug.Log("New Order: " + currentTargetDrinkID);
    }

    private void SetColorRecursively(GameObject obj, Color color)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color newColor = color;
            newColor.a = sr.color.a; // Preserve original alpha
            sr.color = newColor;
        }

        foreach (Transform child in obj.transform)
        {
            SetColorRecursively(child.gameObject, color);
        }
    }

    private void ResetCup()
    {
        if (cupRb == null)
            return;

        // Stop all physics motion
        cupRb.linearVelocity = Vector2.zero;
        cupRb.angularVelocity = 0f;

        // Reset position & rotation
        cupRb.position = cupStartPosition;
        cupRb.rotation = cupStartRotation.eulerAngles.z;

        Debug.Log("Cup reset to original position and rotation.");
    }

    private void ResetOtherObject()
    {
        if (objectToReset == null)
            return;

        objectToReset.transform.position = objectStartPosition;
        objectToReset.transform.rotation = objectStartRotation;

        // Reset Rigidbody2D if it exists
        Rigidbody2D rb = objectToReset.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        Debug.Log("Other object reset to original position and rotation.");
    }
}