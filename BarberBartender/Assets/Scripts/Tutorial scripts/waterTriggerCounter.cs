using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WaterTriggerCounter : MonoBehaviour
{
    public int requiredCount = 20;              // How many water objects needed
    public string LayerName = "Water";          // Name of the layer to check

    private int waterLayer;
    private HashSet<GameObject> currentWaterObjects = new HashSet<GameObject>();

    public Button nextButton;
    public Button backButton;

    public DialogueManager dialogueManager;     // Reference to DialogueManager

    private bool activated = false;             // Prevent multiple triggers

    private void Awake()
    {
        waterLayer = LayerMask.NameToLayer(LayerName);

        if (waterLayer == -1)
        {
            Debug.LogError("Layer '" + LayerName + "' does not exist!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            currentWaterObjects.Add(other.gameObject);
            CheckCount();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == waterLayer)
        {
            currentWaterObjects.Remove(other.gameObject);
        }
    }

    private void CheckCount()
    {
        // Stop if already triggered
        if (activated) return;

        // Make sure DialogueManager is assigned
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not assigned!");
            return;
        }

        // Get current dialogue line
        int index = dialogueManager.CurrentIndex;

        if (index < 0 || index >= dialogueManager.dialogueLines.Count)
            return;

        DialogueLine currentLine = dialogueManager.dialogueLines[index];

        if (currentLine.hasRemovedButtons && currentWaterObjects.Count >= requiredCount)
        {
            activated = true;

            Debug.Log("Water requirement met AFTER buttons were removed!");

            if (nextButton != null)
                nextButton.gameObject.SetActive(true);

            if (backButton != null)
                backButton.gameObject.SetActive(true);
        }
    }
}