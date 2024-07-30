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
        if (collision.CompareTag("Player"))     //�÷��̾ ���̺� ����Ʈ ������ ����
        {
            savePoint = transform.position;     //���̺� ����Ʈ ��ǥ �����ϱ�
            cd.enabled = false;                 //������ �������� ���ϵ��� �ݶ��̴� ����
        }
    }
}
