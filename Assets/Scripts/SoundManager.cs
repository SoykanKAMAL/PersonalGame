using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip cardFlipSound;
    public AudioClip cardMatchSound;
    public AudioClip cardMismatchSound;
    public AudioClip gameOverSound;

    private AudioSource audioSource => GetComponent<AudioSource>();

    private void OnEnable()
    {
        Card.onCardFlipped += PlayFlipSound;
        GameManager.onCardsMatched += PlayMatchSound;
        GameManager.onCardsMismatched += PlayMismatchSound;
        GameManager.onGameEnd += PlayGameOverSound;
    }

    private void OnDisable()
    {
        Card.onCardFlipped -= PlayFlipSound;
        GameManager.onCardsMatched -= PlayMatchSound;
        GameManager.onCardsMismatched -= PlayMismatchSound;
        GameManager.onGameEnd -= PlayGameOverSound;
    }

    private void PlayFlipSound()
    {
        PlaySound(cardFlipSound);
    }

    private void PlayMatchSound()
    {
        PlaySound(cardMatchSound);
    }

    private void PlayMismatchSound()
    {
        PlaySound(cardMismatchSound);
    }

    private void PlayGameOverSound()
    {
        PlaySound(gameOverSound);
    }

    private void PlaySound(AudioClip clip)
    {
        // Randomize pitch to make the sound less repetitive
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}