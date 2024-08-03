using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Portal : MonoBehaviour
{
    private bool isActive = true;
    public float cooldownTime = 1f;
    public GameObject outPortal;
    private CircleCollider2D col;
    public float pauseDuration = .05f;

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            
            StartCoroutine(PortalPause(collision));
        }
    }
    private IEnumerator Cooldown()
    {
        isActive = false;
        col.enabled = false;
        yield return new WaitForSeconds(cooldownTime);
        isActive = true;
        col.enabled = true;
    }
    private IEnumerator PortalPause(Collision2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(pauseDuration);

        rb.transform.position = outPortal.transform.position;
        if(rb != null)
        {
            rb.isKinematic = false;
        }

        StartCoroutine(Cooldown());

    }
}
