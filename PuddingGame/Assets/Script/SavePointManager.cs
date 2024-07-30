using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    public static SavePointManager i;
    public Vector3 savePoint;
    BoxCollider2D cd;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        cd = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))     //플레이어가 세이브 포인트 지점에 오면
        {
            savePoint = transform.position;     //세이브 포인트 좌표 저장하기
            cd.enabled = false;                 //다음에 저장하지 못하도록 콜라이더 끄기
        }
    }
}
