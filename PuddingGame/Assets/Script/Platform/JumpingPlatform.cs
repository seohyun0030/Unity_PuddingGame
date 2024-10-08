using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour //Ƣ������� ����
{
    public float bounce = 5f; // Ƣ������� ��
    float originalBounce;     //���� ź����

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 pos = contactPoint.point; // �浹 ����
        Vector2 normal = contactPoint.normal; // ���� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            originalBounce = PlayerManager.i.Physics.bounciness;
            PlayerManager.i.Physics.bounciness = 0;
            PlayerMoveControl.i.jumpPlatform = true;

            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-normal.normalized * bounce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.i.Physics.bounciness = originalBounce;
        }
    }
}
