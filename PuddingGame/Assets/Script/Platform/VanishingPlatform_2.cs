using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform_2 : MonoBehaviour
{
    [SerializeField] float Time = 2f;
    BoxCollider2D boxCollider2D;
    private bool state = true;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(state == false)
        {
            Invoke("ActiveTrue", Time);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Invoke("ActiveFalse", Time);
        }
    }

    void ActiveFalse()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        state = false;
        boxCollider2D.isTrigger = true;
    }

    void ActiveTrue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        state = true;
        boxCollider2D.isTrigger = false;
    }
}
