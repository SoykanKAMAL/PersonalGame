using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int matchScoreReward = 100;
    private int _score = 0;
    private int _combo = 1;

    public static Action<int> onScoreUpdated;
    public static Action<int> onComboUpdated;

    private void OnEnable()
    {
        GameManager.onCardsMatched += IncreaseScore;
        GameManager.onCardsMatched += IncreaseCombo;
        GameManager.onCardsMismatched += ResetCombo;
    }

    private void IncreaseScore()
    {
        _score += matchScoreReward * _combo;
        onScoreUpdated?.Invoke(_score);
    }

    private void ResetScore()
    {
        _score = 0;
        onScoreUpdated?.Invoke(_score);
    }

    private void IncreaseCombo()
    {
        _combo++;
        onComboUpdated?.Invoke(_combo);
    }

    private void ResetCombo()
    {
        _combo = 1;
        onComboUpdated?.Invoke(_combo);
    }
}
