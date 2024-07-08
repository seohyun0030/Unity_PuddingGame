using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed -= 2f;
            PlayerManager.i.Friction += 0.3f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed += 2f;
            PlayerManager.i.Friction -= 0.3f;
        }
    }
}
