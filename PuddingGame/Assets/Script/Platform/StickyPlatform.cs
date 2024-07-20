using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour //�����̴� ����
{
    public float friction = 0.3f; //������
    public float elasticity = 0.3f; // ź��
    public float jump = 2f; // ���� �Ŀ�

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            //PlayerManager.i.MoveSpeed -= 2f;
            PlayerManager.i.Friction += friction; //������ ����
            PlayerManager.i.BouncePower -= elasticity; //ź�� ����
            PlayerManager.i.JumpPower -= jump; //���� �Ŀ� ����
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
