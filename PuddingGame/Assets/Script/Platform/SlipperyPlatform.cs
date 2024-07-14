using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyPlatform : MonoBehaviour //미끈거리는 발판
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            PlayerManager.i.Friction -= 0.3f; //마찰력 감소
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            PlayerManager.i.Friction += 0.3f;
        }
    }
}
