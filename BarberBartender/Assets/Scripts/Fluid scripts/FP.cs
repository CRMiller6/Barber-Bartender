using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DrinkDeleteButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Zone to check for drinks")]
    public Collider2D zoneCollider; // Assign the FreezeInZone collider

    [Header("Points per drink ID")]
    public int defaultPoints = 1; // Points if no specific mapping
    public DrinkPointMapping[] drinkPoints; // Assign specific points per drink ID

    [Header("Score tracking")]
    public int currentScore = 0; // Total score tracked locally

    [System.Serializable]
    public struct DrinkPointMapping
    {
        public string objectID; // From Mixable
        public int points;      // Points to award for any drink in the zone
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (zoneCollider == null)
        {
            Debug.LogWarning("Zone Collider not assigned!");
            return;
        }

        DeleteDrinksAndAwardPoints();
    }

    private void DeleteDrinksAndAwardPoints()
    {
        Collider2D[] overlapping = Physics2D.OverlapBoxAll(
            zoneCollider.bounds.center,
            zoneCollider.bounds.size,
            0f
        );

        int pointsToAdd = defaultPoints; // Default if no drinks matched

        // Look for the first drink in the zone to determine points
        foreach (var col in overlapping)
        {
            if (col.CompareTag("Drink"))
            {
                Mixable mixable = col.GetComponent<Mixable>();
                if (mixable != null)
                {
                    // Check if there is a specific point mapping
                    foreach (var mapping in drinkPoints)
                    {
                        if (mapping.objectID == mixable.objectID)
                        {
                            pointsToAdd = mapping.points;
                            break;
                        }
                    }
                }
                // We only care about the first drink for points
                break;
            }
        }

        // Delete all drinks in the zone
        foreach (var col in overlapping)
        {
            if (col.CompareTag("Drink"))
            {
                Destroy(col.gameObject);
            }
        }

        // Award points once per button press
        currentScore += pointsToAdd;
        Debug.Log($"Button pressed! Awarded {pointsToAdd} points. Current score: {currentScore}");
    }
}