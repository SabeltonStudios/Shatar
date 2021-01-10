using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    private static bool musicMuted = false;
    private static bool soundEffectsMuted = false;

    private static int gems = 0;
    private static int stars = 0;

    public static int playingLevel = 0;

    public static bool backFromLevel = false;

    private static int nivelActual = 0; //nivel desbloqueado
    private static int level0MejorPuntuacion = 0;
    private static int level0Estrellas = 0;
    private static int level1MejorPuntuacion = 0;
    private static int level1Estrellas = 0;
    private static int level2MejorPuntuacion = 0;
    private static int level2Estrellas = 0;

    #region Sound Related
    public static bool MusicMuted
    {
        get
        {
            if (PlayerPrefs.GetInt("musicMuted", 0) == 0)
                musicMuted = false;
            else
                musicMuted = true;
            return musicMuted;
        }
        set
        {
            musicMuted = value;
            if (musicMuted) 
                PlayerPrefs.SetInt("musicMuted", 1);
            else
                PlayerPrefs.SetInt("musicMuted", 0);
        }
    }

    public static bool SoundEffectsMuted
    {
        get
        {
            if (PlayerPrefs.GetInt("soundEffectsMuted", 0) == 0)
                soundEffectsMuted = false;
            else
                soundEffectsMuted = true;
            return soundEffectsMuted;
        }
        set
        {
            soundEffectsMuted = value;
            if (soundEffectsMuted)
                PlayerPrefs.SetInt("soundEffectsMuted", 1);
            else
                PlayerPrefs.SetInt("soundEffectsMuted", 0);
        }
    }
    #endregion

    #region Gems and Stars Related
    public static int Gems
    {
        get
        {
            gems = PlayerPrefs.GetInt("gems", 0);
            return gems;
        }
        set
        {
            gems = value;
            PlayerPrefs.SetInt("gems", gems);
        }
    }

    public static int Stars
    {
        get
        {
            stars = PlayerPrefs.GetInt("stars", 0);
            return stars;
        }
        set
        {
            stars = value;
            PlayerPrefs.SetInt("stars", stars);
        }
    }
    #endregion

    #region Levels Related
    public static int NivelActual
    {
        get
        {
            nivelActual = PlayerPrefs.GetInt("nivelActual", 0);
            return nivelActual;
        }
        set
        {
            nivelActual = value;
            PlayerPrefs.SetInt("nivelActual", nivelActual);
        }
    }

    /// <summary>
    /// Tutorial
    /// </summary>
    public static int Level0MejorPuntuacion
    {
        get
        {
            level0MejorPuntuacion = PlayerPrefs.GetInt("level0MejorPuntuacion", 0);
            return level0MejorPuntuacion;
        }
        set
        {
            level0MejorPuntuacion = value;
            PlayerPrefs.SetInt("level0MejorPuntuacion", level0MejorPuntuacion);
        }
    }

    public static int Level0Estrellas
    {
        get
        {
            level0Estrellas = PlayerPrefs.GetInt("level0Estrellas", 0);
            return level0Estrellas;
        }
        set
        {
            level0Estrellas = value;
            PlayerPrefs.SetInt("level0Estrellas", level0Estrellas);
        }
    }

    /// <summary>
    /// Level 1
    /// </summary>
    public static int Level1MejorPuntuacion
    {
        get
        {
            level1MejorPuntuacion = PlayerPrefs.GetInt("level1MejorPuntuacion", 0);
            return level1MejorPuntuacion;
        }
        set
        {
            level1MejorPuntuacion = value;
            PlayerPrefs.SetInt("level1MejorPuntuacion", level1MejorPuntuacion);
        }
    }

    public static int Level1Estrellas
    {
        get
        {
            level1Estrellas = PlayerPrefs.GetInt("level1Estrellas", 0);
            return level1Estrellas;
        }
        set
        {
            level1Estrellas = value;
            PlayerPrefs.SetInt("level1Estrellas", level1Estrellas);
        }
    }

    /// <summary>
    /// Level2
    /// </summary>
    public static int Level2MejorPuntuacion
    {
        get
        {
            level2MejorPuntuacion = PlayerPrefs.GetInt("level2MejorPuntuacion", 0);
            return level2MejorPuntuacion;
        }
        set
        {
            level2MejorPuntuacion = value;
            PlayerPrefs.SetInt("level2MejorPuntuacion", level2MejorPuntuacion);
        }
    }

    public static int Level2Estrellas
    {
        get
        {
            level2Estrellas = PlayerPrefs.GetInt("level2Estrellas", 0);
            return level2Estrellas;
        }
        set
        {
            level2Estrellas = value;
            PlayerPrefs.SetInt("level2Estrellas", level2Estrellas);
        }
    }
    #endregion
}
