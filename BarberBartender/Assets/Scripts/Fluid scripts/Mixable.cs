using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Mixable : MonoBehaviour
{
    public string objectID;

    private int cupContacts = 0;

    public bool IsInCup => cupContacts > 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("cup"))
        {
            cupContacts++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("cup"))
        {
            cupContacts--;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Mixable other = collision.gameObject.GetComponent<Mixable>();
        if (other == null) return;

        // Only mix if both are inside a cup
        if (!IsInCup || !other.IsInCup)
            return;

        // Only one object per pair triggers the mix
        if (GetInstanceID() > other.GetInstanceID())
            return;

        MixManager.Instance.TryMix(this, other);
    }
}