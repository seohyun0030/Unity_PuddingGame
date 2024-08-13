using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour //튀어오르는 발판
{
    public float bounce = 5f; // 튀어오르는 힘

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 pos = contactPoint.point; // 충돌 지점
        Vector2 normal = contactPoint.normal; // 법선 벡터
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.i.BouncePower = 0;
            PlayerMoveControl.i.jumpPlatform = true;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-normal.normalized * bounce, ForceMode2D.Impulse);
        }
    }
}
