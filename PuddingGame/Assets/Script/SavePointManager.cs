using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    public static SavePointManager i;
    public GameObject Player;

    private void Awake()
    {
        i = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!Player.activeSelf)      //플레이어가 비활성화 상태라면
                Player.SetActive(true);

            PlayerManager.i.GoToSavePoint();

            //SlotManager.i.RespawnTopping();
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))     //플레이어가 세이브 포인트 지점에 오면
        {
            savePoint = transform.position;     //세이브 포인트 좌표 저장하기
            cd.enabled = false;                 //다음에 저장하지 못하도록 콜라이더 끄기
        }
    }*/
}
