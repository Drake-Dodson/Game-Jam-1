using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    Dictionary<int, Arrow> touching = new Dictionary<int, Arrow>();

    /* ROTATIONS:
     * 
     * 0 = up
     * 1 = left
     * 2 = down
     * 3 = right
     */

    private void Update()
    {
        if (GameManager.instance.isPaused) return;

        if (Input.GetButtonDown("Up"))
        {
            CheckHit(0);
        }

        if (Input.GetButtonDown("Left"))
        {
            CheckHit(1);
        }

        if (Input.GetButtonDown("Down"))
        {
            CheckHit(2);
        }

        if (Input.GetButtonDown("Right"))
        {
            CheckHit(3);
        }
    }

    private void CheckHit(int rotationKey)
    {
        if (CheckRotation(rotationKey))
        {
            GoodHit(rotationKey);
        } else
        {
            BadHit(rotationKey);
        }
    }

    private bool CheckRotation(int rotationKey)
    {
        foreach (var pair in touching)
        {
            if (pair.Key == rotationKey)
            {
                return !pair.Value.isBomb;
            }
        }

        return false;
    }

    private void GoodHit(int key)
    {
        GameManager.instance.Good();

        DestroyArrow(key);
    }

    private void BadHit(int key)
    {
        GameManager.instance.Bad();

        DestroyAllTouching();
    }

    private void DestroyArrow(int key)
    {
        if (!touching.ContainsKey(key)) return;

        Arrow a = touching[key];

        touching.Remove(key);

        a.Destroy();
    }

    private void DestroyAllTouching()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!touching.ContainsKey(i)) continue;

            Arrow a = touching[i];

            touching.Remove(i);

            if (a.isBomb)
            {
                a.Destroy();
            } else
            {
                Destroy(a.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            Arrow a = collision.GetComponent<Arrow>();

            if (a.isDestroying) return;

            if (!touching.ContainsKey(a.rotationID))
            {
                touching.Add(a.rotationID, a);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            Arrow a = collision.GetComponent<Arrow>();

            if (touching.ContainsKey(a.rotationID))
            {
                if (a.isDestroying) return;

                touching.Remove(a.rotationID);

                if (a.isBomb)
                {
                    GameManager.instance.Good();//missed a bomb
                } else
                {
                    GameManager.instance.Bad();//missed an arrow
                }

                a.isDestroying = true;
                Destroy(a.gameObject, 2f);
            }
        }
    }
}
