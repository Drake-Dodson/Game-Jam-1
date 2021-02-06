using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHitBox : MonoBehaviour
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
        if (TutorialGameManager.instance.isPaused) return;

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
        }
        else
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
                if (pair.Value.isBomb)
                    TutorialGameManager.instance.setInProgress(false);
                return !pair.Value.isBomb;
            }
        }

        return false;
    }

    private void GoodHit(int key)
    {
        TutorialGameManager.instance.Good();

        DestroyArrow(key);
    }

    private void BadHit(int key)
    {
        //if(!TutorialGameManager.instance.getLastStage())
          //  TutorialGameManager.instance.lastStage();
        TutorialGameManager.instance.Bad();
        DestroyArrow(key);
    }

    private void DestroyArrow(int key)
    {
        if (!touching.ContainsKey(key)) return;

        Arrow a = touching[key];

        touching.Remove(key);

        a.Destroy();
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

                if (!a.isBomb)
                {
                    TutorialGameManager.instance.MissedArrow();
                }
                else
                {
                    TutorialGameManager.instance.addstage();
                }

                touching.Remove(a.rotationID);

                Destroy(a.gameObject);
            }
        }
    }
}
