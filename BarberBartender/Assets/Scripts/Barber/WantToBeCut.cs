using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class WantToBeCut : MonoBehaviour
{
    public bool wantCutting;
    private SpriteRenderer self;
    [SerializeField] private float duration = 30;

    void Update()
    {
        self = GetComponent<SpriteRenderer>();
        if (self != null)
        {
            StartCoroutine(TransitionToRed());
        }
    }

    IEnumerator TransitionToRed()
    {
        float elapsed = 0f;
        Color startColor = self.color;
        Color targetColor = Color.red;

        while (elapsed < duration)
        {
            if (wantCutting == true)
            {
                elapsed += Time.deltaTime;
                self.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            }
            yield return null;
        }
        self.color = targetColor;
    }
}
