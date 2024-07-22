using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour //끈적이는 발판
{
    public float friction = 20f; //마찰력
    public float elasticity = 0.3f; // 탄력
    public float jump = 2f; // 점프 파워

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed -= 2f;
            PlayerManager.i.Friction += friction; //마찰력 증가
            PlayerManager.i.BouncePower -= elasticity; //탄력 감소
            PlayerManager.i.JumpPower -= jump; //점프 파워 감소
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed += 2f;
            PlayerManager.i.Friction -= friction;
            PlayerManager.i.BouncePower += elasticity;
            PlayerManager.i.JumpPower += jump;
        }
    }
}
