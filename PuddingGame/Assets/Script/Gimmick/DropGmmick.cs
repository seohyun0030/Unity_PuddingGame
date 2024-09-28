using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DropGmmick : MonoBehaviour, IResettable
{
    private Rigidbody2D rb;
    private Collider2D trigger;
    private Collider2D physical;
    public bool isDropped = false;
    private Vector2 initialPosition;
    private GimmickManager gimmickManager;

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
            if (trigger != null) trigger.enabled = false;
            if (physical != null) physical.enabled = true;
            isDropped = true;
            
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && isDropped)
        {
            col.gameObject.SetActive(false);
        }
        else
        {
            isDropped = false;
        }

    }
    private void OnDestroy()
    {
        // ������Ʈ�� �ı��� �� �Ŵ������� �ڽ��� ����
        if (gimmickManager != null)
        {
            gimmickManager.UnregisterResettable(this);
        }
    }
    public void Respawn()
    {
        // ������Ʈ ���� ����
        transform.position = initialPosition; // ��ġ �ʱ�ȭ
        rb.gravityScale = 0f; // �߷� �ʱ�ȭ
        isDropped = false; // ��� ���� �ʱ�ȭ
        if (trigger != null) trigger.enabled = true;
        if (physical != null) physical.enabled = false;
        gameObject.SetActive(true); // ������Ʈ Ȱ��ȭ

    }
}
