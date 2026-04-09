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
    public GameObject tutorialButton;


    public void OnPointerClick(PointerEventData eventData)
    {

        
        if (!orderManager || !orderManager.IsDrinkActive) return;
        if (zoneCollider == null) return;

        HandleSubmission();
        orderManager.EndDrinkEarly();

        if (orderManager.tutorial) 
        {
            tutorialButton.gameObject.SetActive(false); // Disable submission during tutorial
        }
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
                    break;
                }
            }
        }

        if (submittedDrinkID != null)
        {
            if (submittedDrinkID == orderManager.CurrentTargetDrinkID)
            {
                int points = orderManager.GetCurrentDrinkPoints();
                scoreManager.AddPoints(points);
            }
            else
            {
                scoreManager.AddWrong();
            }
        }

        // Destroy all drinks in zone
        foreach (var col in overlapping)
        {
            if (col.CompareTag("Drink")) Destroy(col.gameObject);
        }
    }
}