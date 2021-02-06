using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void Click()
    {
        source.Play();
    }
}
