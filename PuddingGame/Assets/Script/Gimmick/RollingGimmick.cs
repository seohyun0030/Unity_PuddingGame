using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class RollingGimmick : MonoBehaviour
{
    public Vector2 startPosition; // 시작 위치
    public Collider2D endPosition; // 사라지는 위치
    public float resetTime = 3f;


    private Rigidbody2D rb;
    private bool isRolling = false;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        //StartCoroutine(StartRolling());
    }
    
    IEnumerator StartRolling()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(resetTime);
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            isRolling = true;
           
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == endPosition)
        {
            isRolling = false;
            gameObject.SetActive(false);
            ResetObject();
        }
    }
    void ResetObject()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
        rb.isKinematic = true;
        StartCoroutine(StartRolling());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Destroy(collision.gameObject);
        }
    }

}
