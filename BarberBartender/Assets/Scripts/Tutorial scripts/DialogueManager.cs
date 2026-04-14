using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueLine
{
    public string name;
    [TextArea]
    public string text;
    public bool uncleSpeaking;
    public bool dadSpeaking;
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
    public bool waterTrigger;
    public bool purpleTrigger;
    public bool MoveCamera;
    [HideInInspector] public bool hasMovedCamera = false;
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

    public RectTransform parentRect;

    public Transform uncleAnchor; // where uncle dialogue goes
    public Transform dadAnchor;   // where dad dialogue goes
    public Transform defaultAnchor; // fallback

    public float dadLerpDuration = 0.5f;
    private Coroutine dadMoveCoroutine; 

    public GameObject movingCamera;        // The object that moves (like disappearingDad)
    public GameObject targetCamera;        // The destination position

    public float cameraLerpDuration = 0.5f;

    private Coroutine cameraMoveCoroutine;

    [Header("Dialogue Colors")]
    public Color uncleColor = new Color32(0xB5, 0xEE, 0xEA, 255); // B5EEEA
    public Color dadColor   = new Color32(0xFF, 0xDD, 0xA7, 255); // FFDDA7

    public string nextSceneName;


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

                // Load next scene if name is set
                if (!string.IsNullOrEmpty(nextSceneName))
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                {
                    Debug.LogWarning("Next scene name not set!");
                }

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
        // Apply text color
        if (line.uncleSpeaking)
        {
            textComponent.color = uncleColor;
        }
        else if (line.dadSpeaking)
        {
            textComponent.color = dadColor;
        }
        else
        {
            textComponent.color = Color.white; // fallback
        }
        // Move dialogue position
        if (parentRect != null)
        {
            if (line.uncleSpeaking && uncleAnchor != null)
            {
                parentRect.position = uncleAnchor.position;
            }
            else if (line.dadSpeaking && dadAnchor != null)
            {
                parentRect.position = dadAnchor.position;
            }
            else if (defaultAnchor != null)
            {
                parentRect.position = defaultAnchor.position;
            }
        }

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
            if (dadMoveCoroutine != null)
                StopCoroutine(dadMoveCoroutine);

            dadMoveCoroutine = StartCoroutine(MoveDadTransition());

            line.hasSpawnedDad = true;
        }

        if (line.MoveCamera && !line.hasMovedCamera)
        {
            if (cameraMoveCoroutine != null)
                StopCoroutine(cameraMoveCoroutine);

            cameraMoveCoroutine = StartCoroutine(MoveCameraTransition());

            line.hasMovedCamera = true;
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

    private IEnumerator MoveDadTransition()
    {
        if (dissapearingDad == null || dad == null)
            yield break;

        dissapearingDad.SetActive(true);

        Vector3 startPos = dissapearingDad.transform.position;
        Vector3 targetPos = dad.transform.position;

        float time = 0f;

        while (time < dadLerpDuration)
        {
            time += Time.deltaTime;
            float t = time / dadLerpDuration;

            dissapearingDad.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Snap to exact position at end
        dissapearingDad.transform.position = targetPos;
    }

    private IEnumerator MoveCameraTransition()
    {
        if (movingCamera == null || targetCamera == null)
            yield break;

        movingCamera.SetActive(true);

        Vector3 startPos = movingCamera.transform.position;
        Vector3 targetPos = targetCamera.transform.position;

        float time = 0f;

        while (time < cameraLerpDuration)
        {
            time += Time.deltaTime;
            float t = time / cameraLerpDuration;

            movingCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Snap to final position
        movingCamera.transform.position = targetPos;
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