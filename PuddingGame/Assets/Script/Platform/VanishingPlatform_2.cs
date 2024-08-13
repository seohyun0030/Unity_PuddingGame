using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform_2 : MonoBehaviour // 점프하면 사라지는 발판
{
    [SerializeField] float Time = 2f;


    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Invoke("ActiveFalse", Time);
        }
    }
    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
