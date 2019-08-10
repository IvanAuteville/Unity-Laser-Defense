using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreDisplay = null;
    private int totalLength = 0;

    private void Awake()
    {
        GameSession.instance.SetScoreDisplay(this);

        scoreDisplay = GetComponent<TextMeshProUGUI>();
        totalLength = scoreDisplay.GetParsedText().Length;
    }

    public void SetScore(string score)
    {
        int scoreLength = score.Length;

        int zeroesToAdd = totalLength - scoreLength;
        string aux = "";

        for (int i = 0; i < zeroesToAdd; i++)
        {
            aux += "0";
        }

        scoreDisplay.SetText(aux + score);
    }
}