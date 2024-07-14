using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour //�����̴� ����
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed -= 2f;
            PlayerManager.i.Friction += 0.3f; //������ ����
            PlayerManager.i.BouncePower -= 0.3f; //ź�� ����
            PlayerManager.i.JumpPower -= 2f; //���� �Ŀ� ����
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed += 2f;
            PlayerManager.i.Friction -= 0.3f;
            PlayerManager.i.BouncePower += 0.3f;
            PlayerManager.i.JumpPower += 2f;
        }
    }
}
