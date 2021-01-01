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
}
