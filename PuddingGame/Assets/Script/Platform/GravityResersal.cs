using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityResersal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMoveControl>().SetGravityReversed(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMoveControl>().SetGravityReversed(false);
        }
    }
}
