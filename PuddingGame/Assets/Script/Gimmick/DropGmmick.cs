using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGmmick : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D trigger;
    private Collider2D physical;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                trigger = collider;
            }
            else
            {
                physical = collider;
                physical.enabled = false;
               
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.gravityScale = 1f;
            if (trigger != null) trigger.enabled = false;
            if (physical != null) physical.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            Destroy(col.gameObject);
        }
    }
}
