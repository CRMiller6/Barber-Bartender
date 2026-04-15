using UnityEngine;

public class TargetTheScript : MonoBehaviour
{
    public System.Action OnTargetDestroyed;

    private void OnDestroy()
    {
        OnTargetDestroyed?.Invoke();
    }
}
