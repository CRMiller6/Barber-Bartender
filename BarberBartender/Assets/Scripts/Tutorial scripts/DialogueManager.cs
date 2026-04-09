using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
    public string name;
    [TextArea]
    public string text;
    public bool removeButtonsOnStart;
    [HideInInspector]
    public bool hasRemovedButtons = false;
    public bool SpawnRed;
    [HideInInspector]
    public bool hasSpawnedRed = false;

    public bool SpawnDad;
    [HideInInspector]
    public bool hasSpawnedDad = false;

    public bool SpawnSubmit;
    [HideInInspector]
    public bool hasSpawnedSubmit = false;
}

public class DialogueManager : MonoBehaviour
{
    public int CurrentIndex => currentIndex;
    private TMP_Text textComponent;             // The TextMeshPro component on this object
    public List<DialogueLine> dialogueLines;    // List of dialogue lines
    public Button nextButton;                    // Next button reference
    public Button backButton;                    // Back button reference
    public float typingDelay = 0.05f;

    public GameObject redBottle;
    public GameObject dad;
    public GameObject dissapearingDad;
    public GameObject submitButton;
    public DrinkOrderManager orderManager; // Reference to DrinkOrderManager for tutorial integration

    private int currentIndex = 0;               // Current dialogue index
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        textComponent.text = "";
    }

    private void Start()
    {
        if (dialogueLines.Count > 0)
        {
            StartTypingCurrentLine();
        }
    }

    public void ShowNextDialogue()
    {
        if (isTyping)
        {
            FinishCurrentLine();
        }
        else
        {
            currentIndex++;
            if (currentIndex >= dialogueLines.Count)
            {
                Debug.Log("Dialogue finished!");
                currentIndex = dialogueLines.Count - 1;
                return;
            }

            StartTypingCurrentLine();
        }
    }

    public void ShowPreviousDialogue()
    {
        if (currentIndex > 0)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            currentIndex--;
            StartTypingCurrentLine();
        }
        else
        {
            Debug.Log("Already at first dialogue, cannot go back.");
        }
    }

    private void StartTypingCurrentLine()
    {
        var line = dialogueLines[currentIndex];

        // Remove buttons immediately if toggle is set and hasn't happened yet
        if (line.removeButtonsOnStart && !line.hasRemovedButtons)
        {
            if (nextButton != null) nextButton.gameObject.SetActive(false);
            if (backButton != null) backButton.gameObject.SetActive(false);
            line.hasRemovedButtons = true; // Only remove once
        }
        else
        {
            // Re-enable buttons if we previously removed them and returned
            if (line.hasRemovedButtons)
            {
                if (nextButton != null) nextButton.gameObject.SetActive(true);
                if (backButton != null) backButton.gameObject.SetActive(true);
            }
        }

        if (line.SpawnRed && !line.hasSpawnedRed)
        {
            if (redBottle != null) redBottle.gameObject.SetActive(true);
            line.hasSpawnedRed = true; // Only spawn once
        }

        if (line.SpawnDad && !line.hasSpawnedDad)
        {
            if (dad != null) dad.gameObject.SetActive(true);
            if (dissapearingDad != null) dissapearingDad.gameObject.SetActive(false);

            line.hasSpawnedDad = true;
        }

        if (line.SpawnSubmit && !line.hasSpawnedSubmit)
        {
            if (submitButton != null) submitButton.gameObject.SetActive(true);

            line.hasSpawnedSubmit = true;
            orderManager.tutorialStart();
        }



        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(line));
    }

    private void FinishCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        textComponent.text = dialogueLines[currentIndex].text;
        isTyping = false;
    }

    private IEnumerator TypeText(DialogueLine line)
    {
        isTyping = true;
        textComponent.text = "";
        foreach (char c in line.text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingDelay);
        }
        isTyping = false;
        typingCoroutine = null;
    }

    // Call externally to bring buttons back
    public void EnableButtons()
    {
        if (nextButton != null) nextButton.gameObject.SetActive(true);
        if (backButton != null) backButton.gameObject.SetActive(true);
    }



    // Reset dialogue
    public void ResetDialogue()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        textComponent.text = "";
        currentIndex = 0;

        foreach (var line in dialogueLines)
        {
            line.hasRemovedButtons = false;
        }

        EnableButtons();
        StartTypingCurrentLine();
    }
}