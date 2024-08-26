using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float xPos;
    float yPos;

    public GameObject StartPoint;
    private void Start()
    {
        SetPosition();
    }
    void SetPosition()      //플레이어 위치 조정
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        xPos = PlayerPrefs.GetFloat("SavePosX", StartPoint.transform.position.x);
        yPos = PlayerPrefs.GetFloat("SavePosY", StartPoint.transform.position.y);

        if(player != null)
            player.transform.position = new Vector2(xPos, yPos);
    }
}
