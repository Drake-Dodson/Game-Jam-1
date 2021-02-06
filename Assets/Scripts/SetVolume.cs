using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("Master", Mathf.Log10(Data.volume) * 20);
        mixer.SetFloat("SoundEffects", Mathf.Log10(Data.sfxVolume) * 20);
        mixer.SetFloat("Music", Mathf.Log10(Data.musicVolume) * 20);
    }
}
