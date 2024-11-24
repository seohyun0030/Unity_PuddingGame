using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BackgroundMove : MonoBehaviour
{
    // 배경 레이어의 이동 속도 조정 (0일 경우 이동하지 않음)
    public Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);  // X와 Y에 대한 별도의 조정
    // 카메라의 위치
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private Vector3 deltaMovement;

    void Start()
    {
        // 카메라 트랜스폼을 가져옴
        cameraTransform = Camera.main.transform;
        // 이전 카메라 위치 저장
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // 카메라의 이동 거리 계산
        deltaMovement = cameraTransform.position - lastCameraPosition;

        // 배경을 이동 (X, Y 좌표 모두 패럴랙스 효과를 적용)
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y, 0);

        // 이전 카메라 위치 갱신
        lastCameraPosition = cameraTransform.position;
    }
    /*public Transform player;      // 플레이어 Transform
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
    */
}
