using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Option_control : MonoBehaviour
{
    public Toggle knockbackToggle;

    private void Start()
    {
        knockbackToggle.SetIsOnWithoutNotify(Data.hardcoreMode);
    }

    public int VolumeScene;
    public void volume()
    {
        SceneManager.LoadScene(VolumeScene);
    }
    public void reset()
    {
        Data.ResetHighScores();
    }
    public void resetT()
    {
        Data.passTutorial = false;
    }
    public void back()
    {
        SceneManager.LoadScene(0);
    }

    public void OnToggle()
    {
        Data.hardcoreMode = knockbackToggle.isOn;
    }
}
