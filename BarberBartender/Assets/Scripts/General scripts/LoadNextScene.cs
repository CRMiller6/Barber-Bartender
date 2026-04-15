using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevelButton : MonoBehaviour
{
    [Header("Optional Delay Before Loading")]
    public float delay = 0f;

    [Header("Scene Names (Must match Build Settings)")]
    public string[] levelScenes;

    private static int currentLevelIndex = 0;

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

        if (currentLevelIndex >= levelScenes.Length)
        {
            Debug.LogWarning("No more levels in the list.");
            yield break;
        }

        string sceneName = levelScenes[currentLevelIndex];
        Debug.Log("Loading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);

        currentLevelIndex++;
    }

    public void ResetLevels()
    {
        currentLevelIndex = 0;
    }
}