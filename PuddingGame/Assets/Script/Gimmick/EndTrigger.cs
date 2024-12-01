using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour
{
    public static EndTrigger i;
    [SerializeField] GameObject cart; // īƮ
    [SerializeField] GameObject background; // īƮ
    [SerializeField] float size = 1.5f; // ��� ������

    public float moveSpeed = 2f; // �̵� �ӵ�
    public Vector2 targetPosition; // ��ǥ ��ġ
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

        while (Vector2.Distance(cart.transform.position, targetPosition) > 0.1f)
        {
            cart.transform.position = Vector2.MoveTowards(cart.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }

        isTriggered = false;
    }
}
