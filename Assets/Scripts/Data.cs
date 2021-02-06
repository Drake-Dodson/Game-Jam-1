using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static bool passTutorial
    {
        get => PlayerPrefs.GetInt("Tutorial", 0) == 1;
        set => PlayerPrefs.SetInt("Tutorial", value == true ? 1 : 0);
    }

    public static int difficulty
    {
        get => PlayerPrefs.GetInt("Difficulty", 1);
        set => PlayerPrefs.SetInt("Difficulty", value);
    }

    public static bool hardcoreMode
    {
        get => PlayerPrefs.GetInt("Hardcore", 0) == 1;
        set => PlayerPrefs.SetInt("Hardcore", value == true ? 1 : 0);
    }

    public static float volume
    {
        get => PlayerPrefs.GetFloat("Volume", 1);
        set
        {
            PlayerPrefs.SetFloat("Volume", value);
            AudioListener.volume = value;
        }
    }

    public static float sfxVolume
    {
        get => PlayerPrefs.GetFloat("SFX Volume", 1);
        set
        {
            PlayerPrefs.SetFloat("SFX Volume", value);
        }
    }

    public static float musicVolume
    {
        get => PlayerPrefs.GetFloat("Music Volume", 1);
        set
        {
            PlayerPrefs.SetFloat("Music Volume", value);
        }
    }

    public static int score;
    public static int highScore
    {
        //saves high scores for each type of difficulty
        get => PlayerPrefs.GetInt($"High Score {difficulty} {hardcoreMode.ToString()}", 0);
        set => PlayerPrefs.SetInt($"High Score {difficulty} {hardcoreMode.ToString()}", value);
    }

    //resets all high scores for all difficulties
    public static void ResetHighScores()
    {
        for (int i = 1; i <= 3; i++) {
         PlayerPrefs.SetInt("High Score " + i, 0);
        }
    }
}
