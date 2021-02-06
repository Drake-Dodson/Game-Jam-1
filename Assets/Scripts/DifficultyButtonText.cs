using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonText : MonoBehaviour
{
    [SerializeField] int difficulty = 0;
    [SerializeField] string text = "";

    Text t;

    private void Awake()
    {
        t = GetComponent<Text>();
    }

    private void Start()
    {
        if(t != null)
        {
            if(difficulty == Data.difficulty)
            {
                t.text = $"> {text} <";
            } else
            {
                t.text = text;
            }
        }
    }
}
