using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour
{
    public static EndTrigger i;
    [SerializeField] GameObject cart; // 카트
    [SerializeField] GameObject background; // 카트
    [SerializeField] float size = 1.5f; // 배경 사이즈

    public float moveSpeed = 2f; // 이동 속도
    public Vector2 targetPosition; // 목표 위치
    public bool isTriggered = false;


    public void Awake()
    {
        i = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            CameraController.i.ZoomOut();
            StartCoroutine(MoveCart());
            Vector2 scale = background.transform.localScale;
            scale.x *= size;
            scale.y *= size;
            background.transform.localScale = scale;
        }
    }

    IEnumerator MoveCart()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f && Vector2.Distance(cart.transform.position, targetPosition) > 0.1f)
        {
            cart.transform.position = Vector2.MoveTowards(cart.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed *= 2f; 
        while (Vector2.Distance(cart.transform.position, targetPosition) > 0.1f)
        {
            cart.transform.position = Vector2.MoveTowards(cart.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            yield return null;
        }

        isTriggered = false;
    }
}
