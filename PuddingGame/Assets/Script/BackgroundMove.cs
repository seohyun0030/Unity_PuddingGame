using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BackgroundMove : MonoBehaviour
{
    public Transform player;      // 플레이어 Transform
    public Transform outline;     // 아웃라인 Transform

    private Vector3 outlineMin;   // 아웃라인의 최소 좌표
    private Vector3 outlineMax;   // 아웃라인의 최대 좌표
    private Vector3 bgMin;        // 배경의 최소 좌표
    private Vector3 bgMax;        // 배경의 최대 좌표
    private Vector3 bgSize;

    float clampedX;
    float clampedY;

    void Start()
    {
        // 아웃라인 경계 계산
        Renderer outlineRenderer = outline.GetComponent<Renderer>();
        outlineMin = outlineRenderer.bounds.min;
        outlineMax = outlineRenderer.bounds.max;

        // 배경 경계 계산
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
