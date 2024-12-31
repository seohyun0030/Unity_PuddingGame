using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startpos; //���� ����
    public Transform endPos; //���� ����
    private Transform desPos;
    public float speed; //�ӵ�
    public float stopTime; //���ߴ� �ð�
    public Transform trans;

    private void OnEnable()
    {
        transform.position = startpos.position;
        desPos = endPos;
        StartCoroutine(nameof(Moving));
    }

    private void OnCollisionEnter2D(Collision2D col) //�ڽĿ�����Ʈ�� ���� �÷����� ���� �����̰� �����.
    {
        if (col.transform.CompareTag("Player"))
        {
            if (transform.position.y < col.transform.position.y)
                col.transform.SetParent(trans);

        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.SetParent(null);
        }
    }

    private IEnumerator Moving() //�̵� �Լ�
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * speed);

            if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
            {
                if (stopTime > 0f)
                    yield return new WaitForSeconds(stopTime); //���ߴ� �ð�

                if (desPos == endPos) 
                    desPos = startpos;
                else 
                    desPos = endPos;
            }

            yield return null;
        }
    }
}
