using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BackgroundMove : MonoBehaviour
{
    // ��� ���̾��� �̵� �ӵ� ���� (0�� ��� �̵����� ����)
    public Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);  // X�� Y�� ���� ������ ����
    // ī�޶��� ��ġ
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private Vector3 deltaMovement;

    void Start()
    {
        // ī�޶� Ʈ�������� ������
        cameraTransform = Camera.main.transform;
        // ���� ī�޶� ��ġ ����
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // ī�޶��� �̵� �Ÿ� ���
        deltaMovement = cameraTransform.position - lastCameraPosition;

        // ����� �̵� (X, Y ��ǥ ��� �з����� ȿ���� ����)
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y, 0);

        // ���� ī�޶� ��ġ ����
        lastCameraPosition = cameraTransform.position;
    }
    /*public Transform player;      // �÷��̾� Transform
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
    */
}
