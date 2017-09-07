using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game
{
    public static int gamesCount;

    public static bool isGameStarted;

    public static bool sounds = true;

    public static bool music = true;

    public static void PlaySound(AudioSource source, AudioClip clip, float volume = 1, float pitch = 1)
    {
        if (!sounds)
            return;
        source.volume = volume;
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
    }

    public static void SetBool(string name, bool booleanValue)
    {
        PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
    }
    public static bool GetBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 1 ? true : false;
    }
}