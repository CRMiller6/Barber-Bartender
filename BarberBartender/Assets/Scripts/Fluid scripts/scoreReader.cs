using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Reference to the scoring button")]
    public FlavorPoints drinkButton;

    private TMP_Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (drinkButton != null)
        {
            scoreText.text = "Drink score: " + drinkButton.CurrentScore.ToString();
        }
    }
}