using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform_2 : MonoBehaviour
{
    [SerializeField] float Time = 2f;
    //BoxCollider2D boxCollider2D;
    private bool state = true;


    private void Update()
    {
        if(state == false)
        {
            Invoke("ActiveTrue", Time);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Invoke("ActiveFalse", Time);
            Debug.Log("Collider2D");
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
