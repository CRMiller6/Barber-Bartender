using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DrinkDeleteButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Zone to check for drinks")]
    public Collider2D zoneCollider;

    [Header("Drink Definitions")]
    public DrinkDefinition[] possibleDrinks;

    [Header("Scoring")]
    public int correctPoints = 2;
    public int wrongPoints = -1;

    public int currentScore = 0;

    private string currentTargetDrinkID;
    private SpriteRenderer spriteRenderer;

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

        Debug.Log($"Current Score: {currentScore}");

        // Delete all drinks
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

        // Change sprite color
        spriteRenderer.color = possibleDrinks[randomIndex].displayColor;

        Debug.Log($"New Order: {currentTargetDrinkID}");
    }
}