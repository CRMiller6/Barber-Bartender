using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit Game"); // Shows in editor
        Application.Quit();
    }
}
