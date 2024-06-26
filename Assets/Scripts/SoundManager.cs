using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip cardHoverSound;
    public AudioClip cardFlipSound;
    public AudioClip cardMatchSound;
    public AudioClip cardMismatchSound;
    public AudioClip gameOverSound;
    public AudioClip backgroundMusic;

    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private void OnEnable()
    {
        Card.onCardHovered += PlayHoverSound;
        Card.onCardFlipped += PlayFlipSound;
        GameManager.onCardsMatched += PlayMatchSound;
        GameManager.onCardsMismatched += PlayMismatchSound;
        GameManager.onGameEnd += PlayGameOverSound;

        PlayBackgroundMusic();
    }

    private void OnDisable()
    {
        Card.onCardHovered -= PlayHoverSound;
        Card.onCardFlipped -= PlayFlipSound;
        GameManager.onCardsMatched -= PlayMatchSound;
        GameManager.onCardsMismatched -= PlayMismatchSound;
        GameManager.onGameEnd -= PlayGameOverSound;
    }

    private void PlayHoverSound()
    {
        PlaySfxSound(cardHoverSound);
    }

    private void PlayFlipSound()
    {
        PlaySfxSound(cardFlipSound);
    }

    private void PlayMatchSound()
    {
        PlaySfxSound(cardMatchSound);
    }

    private void PlayMismatchSound()
    {
        PlaySfxSound(cardMismatchSound);
    }

    private void PlayGameOverSound()
    {
        PlaySfxSound(gameOverSound);
    }

    private void PlaySfxSound(AudioClip clip)
    {
        // Randomize pitch to make the sound less repetitive
        sfxAudioSource.pitch = Random.Range(0.95f, 1.05f);
        sfxAudioSource.PlayOneShot(clip);
    }

    private void PlayBackgroundMusic()
    {
        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }
}
