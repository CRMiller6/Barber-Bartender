using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WaterTriggerCounter : MonoBehaviour
{
    public int requiredCount = 20;
    public string LayerName = "Water";
    public string PurpleLayerName = "purple";

    private int waterLayer;
    private int purpleLayer;

    private HashSet<GameObject> currentWaterObjects = new HashSet<GameObject>();
    private HashSet<GameObject> currentPurpleObjects = new HashSet<GameObject>();

    public Button nextButton;
    public Button backButton;

    public DialogueManager dialogueManager;

    // Track which dialogue index has already triggered
    private int lastTriggeredIndex = -1;

    private void Awake()
    {
        waterLayer = LayerMask.NameToLayer(LayerName);
        purpleLayer = LayerMask.NameToLayer(PurpleLayerName);

        if (waterLayer == -1)
            Debug.LogError("Layer '" + LayerName + "' does not exist!");

        if (purpleLayer == -1)
            Debug.LogError("Layer '" + PurpleLayerName + "' does not exist!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            currentWaterObjects.Add(other.gameObject);
        }

        if (other.gameObject.layer == purpleLayer)
        {
            currentPurpleObjects.Add(other.gameObject);
        }

        CheckCount();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            currentWaterObjects.Remove(other.gameObject);
        }

        if (other.gameObject.layer == purpleLayer)
        {
            currentPurpleObjects.Remove(other.gameObject);
        }
    }

    private void CheckCount()
    {
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not assigned!");
            return;
        }

        int index = dialogueManager.CurrentIndex;

        if (index < 0 || index >= dialogueManager.dialogueLines.Count)
            return;

        // Prevent retriggering for same dialogue line
        if (lastTriggeredIndex == index)
            return;

        DialogueLine currentLine = dialogueManager.dialogueLines[index];

        // WATER CHECK
        if (currentLine.waterTrigger && currentWaterObjects.Count >= requiredCount)
        {
            Debug.Log("Water requirement met!");

            currentLine.waterTrigger = false; // turn off
            lastTriggeredIndex = index;

            EnableButtons();
            return;
        }

        // PURPLE CHECK
        if (currentLine.purpleTrigger && currentPurpleObjects.Count >= requiredCount)
        {
            Debug.Log("Purple requirement met!");

            currentLine.purpleTrigger = false; // turn off
            lastTriggeredIndex = index;

            EnableButtons();
        }
    }

    private void EnableButtons()
    {
        if (nextButton != null)
            nextButton.gameObject.SetActive(true);

        if (backButton != null)
            backButton.gameObject.SetActive(true);
    }
}