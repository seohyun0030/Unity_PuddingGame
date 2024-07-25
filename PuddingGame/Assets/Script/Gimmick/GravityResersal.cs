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
            collision.transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMoveControl>().SetGravityReversed(false);
            other.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
