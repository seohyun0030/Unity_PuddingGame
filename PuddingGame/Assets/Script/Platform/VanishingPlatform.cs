using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour // 사라지는 발판
{
    [SerializeField] float Time = 5f;

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Invoke("ActiveFalse", Time);
        }
    }

    void ActiveFalse()
    {
       gameObject.SetActive(false);
    }
}
