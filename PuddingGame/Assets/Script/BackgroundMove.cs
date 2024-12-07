using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMove : MonoBehaviour
{
    public Vector2 parallaxFactor = new Vector2(0.5f, 0.5f);      //얼마나 빨리 움직일지 결정

    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    private Vector3 movement;      //카메라 움직임

    void Start()
    {
        cameraTransform = Camera.main.transform;

        lastCameraPos = cameraTransform.position;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Stage2")
        {
            // Stage2에서만 isEffectRunning이 false일 때 작동
            if (!Stage2Camera.i.isEffectRunning)
            {
                movement = cameraTransform.position - lastCameraPos;

                transform.position += new Vector3(movement.x * parallaxFactor.x, movement.y * parallaxFactor.y, 0);

                lastCameraPos = cameraTransform.position;
            }
        }
        else
        {
            // 다른 씬에서는 항상 작동
            movement = cameraTransform.position - lastCameraPos;

            transform.position += new Vector3(movement.x * parallaxFactor.x, movement.y * parallaxFactor.y, 0);

            lastCameraPos = cameraTransform.position;
        }
    }
}
