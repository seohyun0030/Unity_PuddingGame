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
            if(!Player.activeSelf)      //�÷��̾ ��Ȱ��ȭ ���¶��
                Player.SetActive(true);

            PlayerManager.i.GoToSavePoint();

            SlotManager.i.RespawnTopping();
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))     //�÷��̾ ���̺� ����Ʈ ������ ����
        {
            savePoint = transform.position;     //���̺� ����Ʈ ��ǥ �����ϱ�
            cd.enabled = false;                 //������ �������� ���ϵ��� �ݶ��̴� ����
        }
    }*/
}
