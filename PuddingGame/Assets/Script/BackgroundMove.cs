using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BackgroundMove : MonoBehaviour
{
    public Transform player;      // �÷��̾� Transform
    public Transform outline;     // �ƿ����� Transform

    private Vector3 outlineMin;   // �ƿ������� �ּ� ��ǥ
    private Vector3 outlineMax;   // �ƿ������� �ִ� ��ǥ
    private Vector3 bgMin;        // ����� �ּ� ��ǥ
    private Vector3 bgMax;        // ����� �ִ� ��ǥ
    private Vector3 bgSize;

    float clampedX;
    float clampedY;

    void Start()
    {
        // �ƿ����� ��� ���
        Renderer outlineRenderer = outline.GetComponent<Renderer>();
        outlineMin = outlineRenderer.bounds.min;
        outlineMax = outlineRenderer.bounds.max;

        // ��� ��� ���
        Renderer bgRenderer = GetComponent<Renderer>();
        bgMin = bgRenderer.bounds.min;
        bgMax = bgRenderer.bounds.max;
        bgSize = bgMax - bgMin;
    }

    void Update()
    {
        float normalizedX = Mathf.InverseLerp(outlineMin.x, outlineMax.x, player.position.x);
        float normalizedY = Mathf.InverseLerp(outlineMin.y, outlineMax.y, player.position.y);

        float bgX = Mathf.Lerp(bgMin.x, bgMax.x, normalizedX);
        float bgY = Mathf.Lerp(bgMin.y, bgMax.y, normalizedY);

        //transform.position = new Vector3(bgX, bgY);

        transform.position = Vector3.Lerp(transform.position, new Vector3(bgX, bgY), 0.02f);
    }
    /*void LimitPosition()
    {
        clampedX = Mathf.Clamp(clampedX, outlineMin.x, outlineMax.x);
        clampedY = Mathf.Clamp(clampedY, outlineMin.y, outlineMax.y);

        transform.position = new Vector3(clampedX, clampedY);
    }*/
}
