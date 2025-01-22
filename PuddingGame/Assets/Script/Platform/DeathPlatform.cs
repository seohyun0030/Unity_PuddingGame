using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log(col.gameObject.name);
            PlayerMoveControl.i.ShowDeath(col.gameObject.transform.position);        //���� �̹��� �����ֱ�
            col.gameObject.SetActive(false);

            SfxManager.i.PlaySound("Death");        //���� ȿ���� ���

            //PlayerMoveControl.i.ShowDeath(col.gameObject.transform.position);        //���� �̹��� �����ֱ�
        }
    }
}
