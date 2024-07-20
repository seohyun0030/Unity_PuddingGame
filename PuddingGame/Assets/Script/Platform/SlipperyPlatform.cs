using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyPlatform : MonoBehaviour //¹Ì²ö°Å¸®´Â ¹ßÆÇ
{
    public float friction = 0.3f; //¸¶Âû·Â

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            PlayerManager.i.Friction -= friction; //¸¶Âû·Â °¨¼Ò
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            PlayerManager.i.Friction += friction;
        }
    }
}
