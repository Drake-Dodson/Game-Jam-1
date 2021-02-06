using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 movement = Vector2.down;

    [HideInInspector]
    public int rotationID = -1;

    private float speed = 1;

    [HideInInspector]
    public bool isBomb = false;

    [SerializeField]
    private ParticleSystem explosionSystem;

    [SerializeField]
    new private SpriteRenderer renderer;

    [SerializeField]
    private AudioSource audioSource;

    public bool isDestroying = false;

    /* ROTATIONS:
     * 
     * 0 = up
     * 1 = left
     * 2 = down
     * 3 = right
     */

    [SerializeField] private Rigidbody2D body;

    public void SetSpeed(float speed)
    {
        body.velocity = movement * speed;
    }

    public void Destroy()
    {
        if (isDestroying) return;

        isDestroying = true;
        renderer.enabled = false;
        body.velocity = Vector2.zero;
        if(audioSource.clip != null)
        {
            audioSource.Play();
        }
        var main = explosionSystem.main;
        main.startColor = renderer.color;

        Destroy(gameObject, 1f);

        explosionSystem.Play();
    }

    public void SetColor(Color color)
    {
        if (isBomb) return;

        if(color.a == 0)
        {
            renderer.color = Color.white;
        } else
        {
            renderer.color = color;
        }
    }

    public void ScaleHitboxSize(float size)
    {
        size = Mathf.Max(size, 0.01f);

        GetComponent<BoxCollider2D>().size *= size;
    }
}
