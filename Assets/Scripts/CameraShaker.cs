using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;

    private Vector3 origin;

    float intensity = 0f;
    const float smoothRate = 0.05f;

    private void Awake()
    {
        instance = this;

        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(intensity > 0f)
        {
            transform.position = origin + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0f);

            intensity -= smoothRate;
        }
    }

    public void Shake(float intense)
    {
        intensity = intense;
    }
}
