using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WaterTriggerCounter : MonoBehaviour
{
    public int requiredCount = 20;          // How many water objects needed
    public string LayerName = "Water"; // Name of the layer to check

    private int waterLayer;
    private HashSet<GameObject> currentWaterObjects = new HashSet<GameObject>();

    public Button nextButton;
    public Button backButton;

    private bool activated = false;         // Prevent multiple triggers

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
        if (!activated && currentWaterObjects.Count >= requiredCount)
        {
            activated = false;

            Debug.Log("Reached required water count!");

            if (nextButton != null)
                nextButton.gameObject.SetActive(true);

            if (backButton != null)
                backButton.gameObject.SetActive(true);
        }
    }
}