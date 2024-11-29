using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);      //�󸶳� ���� �������� ����

    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    private Vector3 movement;      //ī�޶� ������

    void Start()
    {
        cameraTransform = Camera.main.transform;

        lastCameraPos = cameraTransform.position;
    }

    void Update()
    {
        movement = cameraTransform.position - lastCameraPos;

        transform.position += new Vector3(movement.x * parallaxFactor.x, movement.y * parallaxFactor.y, 0);

        lastCameraPos = cameraTransform.position;
    }
}
