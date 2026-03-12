using UnityEngine;
using UnityEngine.SceneManagement;


public class StoryExplained : MonoBehaviour
{
    public float timeToRead = 10f;

    public float timer;
    public string sceneToLoad = "Main Scene";

    public GameObject dadExplained;
    public bool finishedReadingDad = false;

    public GameObject uncleExplained;
    public bool playerClick = false;

    public GameObject nextStoryButton;
    public GameObject tutorial;
   
    void Start()
    {
        uncleExplained.SetActive(false);
        dadExplained.SetActive(true);
        nextStoryButton.SetActive(false);
        tutorial.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToRead && finishedReadingDad == false)
        {
            finishedReadingDad = true;
            //timer = 0;
            nextStoryButton.SetActive(true);
        }
        if (timer >= timeToRead && finishedReadingDad == true && playerClick == true)
        {
            tutorial.SetActive(true);
            nextStoryButton.SetActive(false);
            //finishedReadingUncle = true;
        }
    }

    public void Transfer()
    {
        if (finishedReadingDad == true)
        {
            dadExplained.SetActive(false);
            uncleExplained.SetActive(true);
            timer = 0;
            playerClick = true;
        }
    }


    
}
