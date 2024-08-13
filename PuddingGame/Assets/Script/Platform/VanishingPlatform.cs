using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour // 사라지는 발판
{

    [SerializeField] float Time = 2f;
    private bool state = true;


    private void Update()
    {
        if (state == false)
        {
            Invoke("ActiveTrue", Time);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Invoke("ActiveFalse", Time);
        }
    }

    void ActiveFalse()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        state = false;
    }

    void ActiveTrue()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        state = true;
    }
}
