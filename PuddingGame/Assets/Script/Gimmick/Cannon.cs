using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float yOffset = 1f;
    public float roatationSpeed = 10f;
    public bool isAttached = false;
    public bool isActive = true;
    public float fireForce = 1f;
    public bool isFire = false;
    public float cooldownTime = 1f;
    public static Cannon i;
    private CircleCollider2D cannon;
    public void Awake()
    {
        i = this;
        cannon = GetComponent<CircleCollider2D>();
    }
    public void fire(Rigidbody2D col)
    {
        if (!isActive) return;

        Vector3 fireDirection = col.transform.up;
        col.AddForce(fireDirection * fireForce);
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
}
