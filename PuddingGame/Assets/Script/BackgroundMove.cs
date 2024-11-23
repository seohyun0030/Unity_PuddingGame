using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float backSpd;       //배경이 움직이는 속도
    Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void FixedUpdate()
    {
        BackGroundMove();
    }
    void BackGroundMove()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = -player.transform.position + transform.position;
        Vector3 moveVector = initialPos + new Vector3(dir.x * backSpd * Time.deltaTime, dir.y * backSpd * Time.deltaTime, 0.0f);
        transform.position = moveVector;
    }
}
