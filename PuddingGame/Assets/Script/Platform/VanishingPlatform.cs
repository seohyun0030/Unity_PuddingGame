using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour // 사라지는 발판
{
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
