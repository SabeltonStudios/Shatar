using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource click_button = null;
    [SerializeField] private AudioSource comprar = null;
    [SerializeField] private AudioSource fichaArrastrandose = null;
    [SerializeField] private AudioSource ficha_comida1 = null;
    [SerializeField] private AudioSource ficha_comida2 = null;
    [SerializeField] private AudioSource fichas1 = null;
    [SerializeField] private AudioSource fichas2 = null;
    [SerializeField] private AudioSource fichas3 = null;
    [SerializeField] private AudioSource derrota = null;
    [SerializeField] private AudioSource victoria = null;
    [SerializeField] private AudioSource teleport = null;
    [SerializeField] private AudioSource vallas1 = null;
    [SerializeField] private AudioSource vallas2 = null;
    [SerializeField] private AudioSource camaraRotation = null;
    [SerializeField] private AudioSource goal = null;
    [SerializeField] private AudioSource vallas = null;

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
                case "comprar":
                    comprar.Play();
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
                case "teleport":
                    teleport.Play();
                    break;
                case "vallas1":
                    vallas1.Play();
                    break;
                case "vallas2":
                    vallas2.Play();
                    break;
                case "camaraRotation":
                    camaraRotation.Play();
                    break;
                case "goal":
                    goal.Play();
                    break;
                case "vallas":
                    vallas.Play();
                    break;
            }
        }
    }

    public void RandomChessPiecePlaceSound()
    {
        int eleccionAleatoria = Random.Range(1, 3);
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
