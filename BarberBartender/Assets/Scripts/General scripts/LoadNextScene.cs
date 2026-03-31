using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;   // Needed for SceneAsset
using System.Collections;

public class LoadNextScene : MonoBehaviour
{
    [Header("Optional Delay Before Loading")]
    public float delay = 0f;

    [Header("Scenes for Levels (Drag & Drop)")]
    public SceneAsset[] levelScenes;  // Drag scene assets here in inspector

    private static int currentLevelIndex = 0; // Start at first scene in array

    // Call this from the Button's OnClick()
    public void LoadNextLevel()
    {
        if (levelScenes == null || levelScenes.Length == 0)
        {
            Debug.LogError("No level scenes assigned!");
            return;
        }

        StartCoroutine(LoadLevelRoutine());
    }

    private IEnumerator LoadLevelRoutine()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        // Get the next scene name
        if (currentLevelIndex >= levelScenes.Length)
        {
            Debug.LogWarning("No more levels in the list. You can reset or load main menu.");
            yield break;
        }

        string sceneName = levelScenes[currentLevelIndex].name;
        Debug.Log("Loading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);

        // Increment for next button press
        currentLevelIndex++;
    }
}