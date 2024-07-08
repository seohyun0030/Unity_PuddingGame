using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startpos; //시작 지점
    public Transform endPos; //종료 지점
    private Transform desPos;
    public float speed; //속도
    public float stopTime; //멈추는 시간

    private void OnEnable()
    {
        transform.position = startpos.position;
        desPos = endPos;
        StartCoroutine(nameof(Moving));
    }

    private void OnCollisionEnter2D(Collision2D col) //자식오브젝트로 만들어서 플랫폼과 같이 움직이게 만든다.
    {
        if (col.transform.CompareTag("Player"))
        {
            if (transform.position.y < col.transform.position.y)
                col.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    private IEnumerator Moving() //이동 함수
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * speed);

            if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
            {
                if (stopTime > 0f)
                    yield return new WaitForSeconds(stopTime); //멈추는 시간

                if (desPos == endPos) 
                    desPos = startpos;
                else 
                    desPos = endPos;
            }

            yield return null;
        }
    }
}
