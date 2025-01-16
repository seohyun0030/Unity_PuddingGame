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
            if(!Player.activeSelf)      //�÷��̾ ��Ȱ��ȭ ���¶��
            {
                Player.SetActive(true);
            }

            SlotManager.i.DeleteTopping();      //������ �ִ� ���� ����
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
