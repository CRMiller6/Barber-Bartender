using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void MainSceneButton(string MainScene)
    {
        SceneManager.LoadScene(MainScene);
    }

    public void StorySceneButton(string StoryIntro)
    {
        SceneManager.LoadScene(StoryIntro);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
