using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Button newGameButton;
    public Button saveGameButton;
    public Button loadGameButton;
    public Button exitGameButton;

    public AudioMixer audioMixer;

    private void OnEnable()
    {
        ScoreManager.onScoreUpdated += UpdateScore;
        ScoreManager.onComboUpdated += UpdateCombo;
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        newGameButton.onClick.AddListener(GameManager.Instance.StartGame);
        saveGameButton.onClick.AddListener(GameManager.Instance.SaveGame);
        loadGameButton.onClick.AddListener(GameManager.Instance.LoadGame);
        exitGameButton.onClick.AddListener(GameManager.Instance.QuitGame);
        GameManager.onGameStart += StartGame;
        GameManager.onGameSaved += SaveGame;
        GameManager.onGameLoaded += LoadGame;
        GameManager.onGameEnd += GameOver;

        UpdateScore(0);
        UpdateCombo(1);
    }

    private void OnDisable()
    {
        ScoreManager.onScoreUpdated -= UpdateScore;
        ScoreManager.onComboUpdated -= UpdateCombo;
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        newGameButton.onClick.RemoveListener(GameManager.Instance.StartGame);
        saveGameButton.onClick.RemoveListener(GameManager.Instance.SaveGame);
        loadGameButton.onClick.RemoveListener(GameManager.Instance.LoadGame);
        exitGameButton.onClick.RemoveListener(GameManager.Instance.QuitGame);
        GameManager.onGameStart -= StartGame;
        GameManager.onGameSaved -= SaveGame;
        GameManager.onGameLoaded -= LoadGame;
        GameManager.onGameEnd -= GameOver;
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("MusicVolume", audioMixer.GetFloat("MusicVolume", out float musicVolume) ? (int)musicVolume : 0);
        PlayerPrefs.SetInt("SFXVolume", audioMixer.GetFloat("SFXVolume", out float sfxVolume) ? (int)sfxVolume : 0);
    }

    private void LoadGame(uint seed)
    {
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetInt("MusicVolume"));
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetInt("SFXVolume"));
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

    private void TogglePauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        sfxVolumeSlider.value = audioMixer.GetFloat("SFXVolume", out float sfxVolume) ? sfxVolume : 0;
        musicVolumeSlider.value = audioMixer.GetFloat("MusicVolume", out float musicVolume) ? musicVolume : 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu(!pauseMenu.activeSelf);
        }
    }

    private void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    private void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    private void StartGame(uint seed)
    {
        TogglePauseMenu(false);
        gameOverMenu.SetActive(false);
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
}
