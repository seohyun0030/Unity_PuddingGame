using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 3.0f;

    public GameObject player;

    private void FixedUpdate()
    {
        Vector3 dir = player.transform.position - this.transform.position;

        //점프할 때도 카메라 따라감
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            this.transform.Translate(moveVector);

        //점프할 때 카메라 안따라감
        /*Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, 0.0f, 0.0f);
        this.transform.Translate(moveVector);*/

        CameraMove();
    }
    void CameraMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveHorizontal * cameraSpeed * Time.deltaTime, moveVertical * cameraSpeed * Time.deltaTime, 0.0f);
        this.transform.Translate(move);
    }
}
