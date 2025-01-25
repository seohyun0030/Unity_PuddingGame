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
            SfxManager.i.PlaySound("IcicleFalling");
            if (trigger != null) trigger.enabled = false;
            if (physical != null) physical.enabled = true;
            isDropped = true;
            
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(isDropped.ToString());
       
        if (col.gameObject.CompareTag("Player") && isDropped)
        {
            PlayerMoveControl.i.ShowDeath(col.transform.position);      //���� �̹��� ����

            col.gameObject.SetActive(false);
            SfxManager.i.PlaySound("Death");
        }
        if (col.gameObject.CompareTag("Platform") && isDropped)
        {
            SfxManager.i.PlaySound("IcicleDropped");
            isDropped = false;
        }

    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            
            isDropped = true;
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
    public IEnumerator coRespawn()
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;

        // ������Ʈ ���� ����
        transform.position = initialPosition; // ��ġ �ʱ�ȭ
        rb.gravityScale = 0f; // �߷� �ʱ�ȭ
       
        if (trigger != null) trigger.enabled = true;
        if (physical != null) physical.enabled = false;

        yield return null;
   
        rb.simulated = true;
        isDropped = false; // ��� ���� �ʱ�ȭ
        gameObject.SetActive(true); // ������Ʈ Ȱ��ȭ
    }

}
