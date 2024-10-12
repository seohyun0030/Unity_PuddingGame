using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;

public class Cannon : MonoBehaviour
{
    public float yOffset = 1f;
    public bool isAttached = false;
    public bool isActive = true;
    public float fireForce = 1f;
    public bool isFire = false;
    public float cooldownTime = 1f;
    public static Cannon i;
    private CircleCollider2D cannon;
    public float rayDistance = 10f;
    public void Awake()
    {
        i = this;
        cannon = GetComponent<CircleCollider2D>();
    }

    public void fire(Rigidbody2D col)
    {
        if (!isActive) return;

        //Vector3 fireDirection = transform.up;
        col.transform.SetParent(null);
        col.AddForce(transform.up * fireForce);
        isAttached = false;
        isFire = true;
        StartCoroutine(Cooldown());


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            Vector3 pos = transform.position;
            collision.transform.position = new Vector3(pos.x, pos.y + yOffset, pos.z);
            collision.transform.SetParent(transform);
            PlayerManager.i.CanJump = false;
            isAttached = true;

        }
    }
    private IEnumerator Cooldown()
    {
        isActive = false;
        cannon.enabled = false;
        yield return new WaitForSeconds(cooldownTime);
        isActive = true;
        cannon.enabled = true;
    }
    public void RotateCannon(float angle, Rigidbody2D col)
    {
        transform.Rotate(0, 0, angle);
        col.transform.Rotate(0, 0, angle);



    }
    public bool IsWall(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, LayerMask.GetMask("Platform"));
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        // Raycast 시각화 (디버그용)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)transform.up * rayDistance);
    }
}
#endif