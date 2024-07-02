using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour //Ƣ������� ����
{
    public float bounce = 5f; // Ƣ������� ��

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 pos = contactPoint.point; // �浹 ����
        Vector2 normal = contactPoint.normal; // ���� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-normal.normalized * bounce, ForceMode2D.Impulse);
        }
    }

}
