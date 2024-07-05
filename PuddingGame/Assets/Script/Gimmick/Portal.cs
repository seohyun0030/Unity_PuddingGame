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

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            collision.transform.position = outPortal.transform.position;
            StartCoroutine(Cooldown());
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
}
