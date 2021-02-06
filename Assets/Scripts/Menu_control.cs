using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_control : MonoBehaviour
{
    public int startScene;
    public int optionScene;
    public int difficultyScene;
    public int creditScene;

    public void ButtonStart()
    {
        if (Data.passTutorial)
            SceneManager.LoadScene(startScene);
        else
            SceneManager.LoadScene(6);
    }
    public void ButtonDifficulty()
    {
        SceneManager.LoadScene(difficultyScene);
    }
    public void ButtonOption()
    {
        SceneManager.LoadScene(optionScene);
    }
    public void credit()
    {
        SceneManager.LoadScene(creditScene);
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
}
