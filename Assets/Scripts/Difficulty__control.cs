using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Difficulty__control : MonoBehaviour
{
    public void Easy()
    {
        Data.difficulty = 1;
        SceneManager.LoadScene(0);
    }
    public void Medium()
    {
        Data.difficulty = 2;
        SceneManager.LoadScene(0);
    }
    public void Hard()
    {
        Data.difficulty = 3;
        SceneManager.LoadScene(0);
    }
}
