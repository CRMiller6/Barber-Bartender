using UnityEngine;

public class DryerTool : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<HairData>(out var hair))
        {
            // Only dry if the hair is currently wet
            if (hair.isWet)
            {
                hair.SetDry();
            }
        }
    }
}