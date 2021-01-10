using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource click_button = null;
    [SerializeField] private AudioSource fichaArrastrandose = null;
    [SerializeField] private AudioSource ficha_comida1 = null;
    [SerializeField] private AudioSource ficha_comida2 = null;
    [SerializeField] private AudioSource fichas1 = null;
    [SerializeField] private AudioSource fichas2 = null;
    [SerializeField] private AudioSource fichas3 = null;
    [SerializeField] private AudioSource derrota = null;
    [SerializeField] private AudioSource victoria = null;

    [Header("Music")]
    [SerializeField] private AudioSource song_menu = null;

    public void Play_SoundEffect(string name)
    {
        if (!PlayerData.SoundEffectsMuted)
        {
            switch (name)
            {
                case "click_button":
                    click_button.Play();
                    break;
                case "fichaArrastrandose":
                    fichaArrastrandose.Play();
                    break;
                case "ficha_comida1":
                    ficha_comida1.Play();
                    break;
                case "ficha_comida2":
                    ficha_comida2.Play();
                    break;
                case "fichas1":
                    fichas1.Play();
                    break;
                case "fichas2":
                    fichas2.Play();
                    break;
                case "fichas3":
                    fichas3.Play();
                    break;
                case "derrota":
                    derrota.Play();
                    break;
                case "victoria":
                    victoria.Play();
                    break;
            }
        }
    }

    public void RandomChessPiecePlaceSound()
    {
        int eleccionAleatoria = Random.Range(1, 3);
        Debug.Log("eleccionAleatoria = " + eleccionAleatoria);
        switch (eleccionAleatoria)
        {
            case 1:
                fichas1.Play();
                break;
            case 2:
                fichas2.Play();
                break;
            case 3:
                fichas3.Play();
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
        
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
