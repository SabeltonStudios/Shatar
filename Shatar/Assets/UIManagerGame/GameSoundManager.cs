using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource click_button = null;

    [Header("Music")]
    [SerializeField] private AudioSource song_game = null;

    public void Play_SoundEffect(string name)
    {
        switch (name)
        {
            case "click_button":
                click_button.Play();
                break;
        }
    }

    public void Play_Music(string name)
    {
        switch (name)
        {
            case "song_game":
                song_game.Play();
                break;
        }
    }

    public void Stop_Music(string name)
    {
        switch (name)
        {
            case "song_game":
                song_game.Stop();
                break;
        }
    }

    public void Mute_Music(string name, bool state)
    {
        switch (name)
        {
            case "song_game":
                song_game.mute = state;
                break;
        }
    }

    public IEnumerator SoundFadeOut(string audioSourceName, float FadeTime)
    {
        AudioSource audioSource = null;
        switch (audioSourceName)
        {
            case "song_game":
                audioSource = song_game;
                break;
        }

        float startVolume = audioSource.volume;

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
