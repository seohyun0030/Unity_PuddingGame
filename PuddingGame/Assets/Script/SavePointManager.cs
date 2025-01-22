using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    public static SavePointManager i;
    public GameObject Player;
    public ParticleSystem particle;

    private void Awake()
    {
        i = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!Player.activeSelf)      //플레이어가 비활성화 상태라면
            {
                Player.SetActive(true);

                PlayerMoveControl.i.Rebirth();
            }

            SlotManager.i.DeleteTopping();      //가지고 있는 토핑 삭제
            PlayerManager.i.GoToSavePoint();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            particle.Play();
        }
    }
}
