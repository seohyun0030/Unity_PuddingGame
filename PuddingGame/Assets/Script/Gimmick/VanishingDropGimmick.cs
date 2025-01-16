using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingDropGimmick : MonoBehaviour, IResettable
{
    private Rigidbody2D rb;
    private Collider2D trigger;
    private Collider2D physical;
    private bool isActive = true;
    public GameObject player;
    private Vector2 initialPosition;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        initialPosition = transform.position;
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
            SfxManager.i.PlaySound("IcicleFalling");
            if (trigger != null) trigger.enabled = false;
            if (physical != null) physical.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.SetActive(false);
            isActive = false;
            SfxManager.i.PlaySound("Death");
        }
        if (col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Player"))
        {
            SfxManager.i.PlaySound("IcicleDropped");
            gameObject.SetActive(false);
            isActive = false;
        }

    }
    public void Respawn()
    {
        transform.position = initialPosition;
        rb.gravityScale = 0f;
        gameObject.SetActive(true);
        if (trigger != null) trigger.enabled = true;
        if (physical != null) physical.enabled = false;
        isActive = true;
    }
    
}
