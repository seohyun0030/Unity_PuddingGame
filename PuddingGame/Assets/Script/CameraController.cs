using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f;

    public GameObject player;

    private void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;

        //������ ���� ī�޶� ����
        //Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);

        //������ �� ī�޶� �ȵ���
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, 0.0f, 0.0f);
        this.transform.Translate(moveVector);
    }
}