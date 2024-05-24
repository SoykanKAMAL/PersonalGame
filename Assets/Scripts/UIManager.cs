using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    private void OnEnable()
    {
        ScoreManager.onScoreUpdated += UpdateScore;
        ScoreManager.onComboUpdated += UpdateCombo;

        UpdateScore(0);
        UpdateCombo(1);
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    private void UpdateCombo(int combo)
    {
        if(combo > 1)
        {
            comboText.text = $"Combo: {combo}x";
        }
        else
        {
            comboText.text = string.Empty;
        }
    }
}
