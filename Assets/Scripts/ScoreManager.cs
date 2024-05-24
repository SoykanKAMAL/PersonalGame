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
        GameManager.onGameStart += StartGame;
        GameManager.onGameSaved += SaveGame;
        GameManager.onGameLoaded += LoadGame;
    }

    private void OnDisable()
    {
        GameManager.onCardsMatched -= IncreaseScore;
        GameManager.onCardsMatched -= IncreaseCombo;
        GameManager.onCardsMismatched -= ResetCombo;
        GameManager.onGameStart -= StartGame;
        GameManager.onGameSaved -= SaveGame;
        GameManager.onGameLoaded -= LoadGame;
    }

    private void StartGame(uint seed)
    {
        ResetScore();
        ResetCombo();
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.SetInt("Combo", _combo);
    }

    private void LoadGame(uint seed)
    {
        _score = PlayerPrefs.GetInt("Score");
        _combo = PlayerPrefs.GetInt("Combo");
        onScoreUpdated?.Invoke(_score);
        onComboUpdated?.Invoke(_combo);
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
