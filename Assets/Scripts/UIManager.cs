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
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Button saveGameButton;
    public Button exitGameButton;

    public AudioMixer audioMixer;

    private void OnEnable()
    {
        ScoreManager.onScoreUpdated += UpdateScore;
        ScoreManager.onComboUpdated += UpdateCombo;
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        saveGameButton.onClick.AddListener(GameManager.Instance.SaveGame);
        exitGameButton.onClick.AddListener(GameManager.Instance.QuitGame);
        GameManager.onGameSaved += SaveGame;
        GameManager.onGameLoaded += LoadGame;

        UpdateScore(0);
        UpdateCombo(1);
    }

    private void OnDisable()
    {
        ScoreManager.onScoreUpdated -= UpdateScore;
        ScoreManager.onComboUpdated -= UpdateCombo;
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        saveGameButton.onClick.RemoveListener(GameManager.Instance.SaveGame);
        exitGameButton.onClick.RemoveListener(GameManager.Instance.QuitGame);
        GameManager.onGameSaved -= SaveGame;
        GameManager.onGameLoaded -= LoadGame;
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("MusicVolume", audioMixer.GetFloat("MusicVolume", out float musicVolume) ? (int)musicVolume : 0);
        PlayerPrefs.SetInt("SFXVolume", audioMixer.GetFloat("SFXVolume", out float sfxVolume) ? (int)sfxVolume : 0);
    }

    private void LoadGame()
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
}
