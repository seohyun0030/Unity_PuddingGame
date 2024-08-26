using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlatform : MonoBehaviour //트램펄린 발판
{
    public float bounce = 1f; // 튀어오르는 힘

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(PlayerManager.i.speed);
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 pos = contactPoint.point; // 충돌 지점
        Vector2 normal = contactPoint.normal; // 법선 벡터
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.i.Physics.bounciness = 0;
            PlayerMoveControl.i.jumpPlatform = true;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-normal.normalized * PlayerManager.i.speed * bounce, ForceMode2D.Impulse);
        }
    }
}
