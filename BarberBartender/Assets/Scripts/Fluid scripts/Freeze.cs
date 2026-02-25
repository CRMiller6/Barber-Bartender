using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FreezeInZone : MonoBehaviour
{
    // Tracks frozen cups in this zone
    private HashSet<Rigidbody2D> frozenCups = new HashSet<Rigidbody2D>();

    // Tracks drinks frozen along with cups
    private HashSet<Rigidbody2D> frozenDrinks = new HashSet<Rigidbody2D>();

    [Header("Indicator to show when a cup is frozen")]
    public GameObject frozenIndicator;

    private void UpdateIndicator()
    {
        if (frozenIndicator != null)
        {
            frozenIndicator.SetActive(frozenCups.Count > 0);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Handle cups
        if (other.CompareTag("cup"))
        {
            Drag dragScript = other.GetComponent<Drag>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (dragScript != null && !dragScript.IsDragging)
                {
                    // Freeze the cup
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;

                    // Add to frozen cups set if not already
                    if (frozenCups.Add(rb))
                    {
                        // Freeze drinks that are currently inside the zone at the moment of freezing
                        Collider2D[] overlapping = Physics2D.OverlapBoxAll(
                            transform.position, 
                            GetComponent<Collider2D>().bounds.size, 
                            0f
                        );

                        foreach (var col in overlapping)
                        {
                            if (col.CompareTag("Drink"))
                            {
                                Rigidbody2D drinkRb = col.GetComponent<Rigidbody2D>();
                                if (drinkRb != null)
                                {
                                    drinkRb.constraints = RigidbodyConstraints2D.FreezeAll;
                                    frozenDrinks.Add(drinkRb);
                                }
                            }
                        }
                    }

                    UpdateIndicator(); // Turn on indicator
                }
                else
                {
                    // Unfreeze cup if being dragged
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.freezeRotation = false;
                    frozenCups.Remove(rb);

                    // Unfreeze any drinks that were linked to this cup
                    foreach (var drinkRb in frozenDrinks)
                    {
                        if (drinkRb != null)
                        {
                            drinkRb.constraints = RigidbodyConstraints2D.None;
                            drinkRb.freezeRotation = false;
                        }
                    }
                    frozenDrinks.Clear();

                    UpdateIndicator(); // Turn off indicator if no frozen cups
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Reset constraints for cups and drinks leaving the zone
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.freezeRotation = false;
        }

        if (other.CompareTag("cup"))
        {
            frozenCups.Remove(rb);

            // Unfreeze linked drinks when cup leaves
            foreach (var drinkRb in frozenDrinks)
            {
                if (drinkRb != null)
                {
                    drinkRb.constraints = RigidbodyConstraints2D.None;
                    drinkRb.freezeRotation = false;
                }
            }
            frozenDrinks.Clear();

            UpdateIndicator(); // Turn off indicator if no frozen cups
        }
        else if (other.CompareTag("Drink"))
        {
            frozenDrinks.Remove(rb);
        }
    }
}