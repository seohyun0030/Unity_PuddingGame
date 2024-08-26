using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rasberry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            SlotManager.i.AddTopping("Rasberry");
        }
    }
}
