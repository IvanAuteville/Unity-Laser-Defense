using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    private TextMeshProUGUI scoreDisplay = null;

    private void Awake()
    {
        scoreDisplay = GetComponent<TextMeshProUGUI>();

        int score = FindObjectOfType<GameSession>().GetScore();

        scoreDisplay.SetText(score.ToString());
    }
}
