using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Volume_control : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundEffectsSlider;

    public int option;

    private void Start()
    {
        masterSlider.value = Data.volume;
        musicSlider.value = Data.musicVolume;
        soundEffectsSlider.value = Data.sfxVolume;
    }

    public void ButtonBack()
    {
        SceneManager.LoadScene(option);
    }

    void Update()
    {
        mixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        mixer.SetFloat("SoundEffects", Mathf.Log10(soundEffectsSlider.value) * 20);
        mixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);

        Data.volume = masterSlider.value;
        Data.sfxVolume = soundEffectsSlider.value;
        Data.musicVolume = musicSlider.value;
    }
}
