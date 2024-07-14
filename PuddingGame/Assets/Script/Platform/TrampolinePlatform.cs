using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlatform : MonoBehaviour //트램펄린 발판
{
    public float speed; //플레이어 속도

    private void OnCollisionEnter2D(Collision2D collision)
    {
        speed = PlayerManager.i.rigidbody.velocity.magnitude;

        ContactPoint2D contactPoint = collision.contacts[0];
        Vector2 pos = contactPoint.point; // 충돌 지점
        Vector2 normal = contactPoint.normal; // 법선 벡터
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-normal.normalized * speed, ForceMode2D.Impulse);
        }
    }
}
