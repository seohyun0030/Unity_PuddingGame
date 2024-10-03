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
        Debug.Log(isDropped.ToString());
       
        if (col.gameObject.CompareTag("Player") && isDropped)
        {
            col.gameObject.SetActive(false);
        }
        if (col.gameObject.CompareTag("Platform") && isDropped)
        {
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
        // 오브젝트가 파괴될 때 매니저에서 자신을 제거
        if (gimmickManager != null)
        {
            gimmickManager.UnregisterResettable(this);
        }
    }
    public void Respawn()
    {
        // 오브젝트 상태 리셋
        transform.position = initialPosition; // 위치 초기화
        rb.gravityScale = 0f; // 중력 초기화
        isDropped = false; // 드롭 상태 초기화
        if (trigger != null) trigger.enabled = true;
        if (physical != null) physical.enabled = false;
        gameObject.SetActive(true); // 오브젝트 활성화

    }
}
