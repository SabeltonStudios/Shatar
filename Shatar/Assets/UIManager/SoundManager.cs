using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource click_button = null;

    [Header("Music")]
    [SerializeField] private AudioSource song_menu = null;

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
            case "song_menu":
                song_menu.Play();
                break;
        }
    }

    public void Stop_Music(string name)
    {
        switch (name)
        {
            case "song_menu":
                song_menu.Stop();
                break;
        }
    }

    public void Mute_Music(string name, bool state)
    {
        switch (name)
        {
            case "song_menu":
                song_menu.mute = state;
                break;
        }
    }

    public IEnumerator SoundFadeOut(string audioSourceName, float FadeTime)
    {
        AudioSource audioSource = null;
        switch (audioSourceName)
        {
            case "song_menu":
                audioSource = song_menu;
                break;
        }

        float startVolume = audioSource.volume;

        /*
        for (float t = 0.1f; t < FadeTime; t += Time.deltaTime)
        {
            float porcentaje = t / FadeTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, porcentaje);
            yield return null;
        }
        */
        
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
