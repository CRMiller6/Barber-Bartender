using UnityEngine;

public class Mixable : MonoBehaviour
{
    [Header("Unique ID for Mixing")]
    public string objectID; // e.g., "Apple", "Banana"

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the Mixable component from the object we collided with
        Mixable otherMixable = collision.gameObject.GetComponent<Mixable>();
        if (otherMixable == null) return;

        // Attempt to mix
        MixManager.TryMix(this, otherMixable);
    }
}
