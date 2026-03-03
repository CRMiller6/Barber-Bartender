using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DrinkSubmissionHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("Zone to Check")]
    public Collider2D zoneCollider;

    [Header("References")]
    public DrinkOrderManager orderManager;
    public FlavorPoints scoreManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!orderManager || !orderManager.IsDrinkActive)
        {
            // No active drink, can't submit
            Debug.Log("No active drink to submit.");
            return;
        }

        if (zoneCollider == null)
        {
            Debug.LogWarning("Zone Collider not assigned!");
            return;
        }

        HandleSubmission();

        // End the current drink early so the delay timer starts
        orderManager.EndDrinkEarly();
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
            if (submittedDrinkID == orderManager.CurrentTargetDrinkID)
            {
                scoreManager.AddCorrect();
            }
            else
            {
                scoreManager.AddWrong();
            }
        }
        else
        {
            Debug.Log("No drink submitted.");
        }

        // Destroy all drinks in zone
        foreach (var col in overlapping)
        {
            if (col.CompareTag("Drink"))
            {
                Destroy(col.gameObject);
            }
        }
    }
}