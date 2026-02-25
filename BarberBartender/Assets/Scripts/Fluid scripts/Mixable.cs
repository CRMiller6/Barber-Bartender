using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Mixable : MonoBehaviour
{
    public string objectID;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Mixable other = collision.gameObject.GetComponent<Mixable>();
        if (other == null) return;

        // Only one object per pair triggers the mix
        if (GetInstanceID() > other.GetInstanceID())
            return;

        MixManager.Instance.TryMix(this, other);
    }
}