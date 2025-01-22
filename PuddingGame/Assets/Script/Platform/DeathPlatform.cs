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
            PlayerMoveControl.i.ShowDeath(col.gameObject.transform.position);        //죽음 이미지 보여주기
            col.gameObject.SetActive(false);

            SfxManager.i.PlaySound("Death");        //죽음 효과음 재생

            //PlayerMoveControl.i.ShowDeath(col.gameObject.transform.position);        //죽음 이미지 보여주기
        }
    }
}
