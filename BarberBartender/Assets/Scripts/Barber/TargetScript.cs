using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public System.Action OnTargetDestroyed;

    private void OnDestroy()
    {
        OnTargetDestroyed?.Invoke();
    }
}
