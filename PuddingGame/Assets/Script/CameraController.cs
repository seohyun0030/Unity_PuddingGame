using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    private PlayerMoveControl playerMoveControl;
    public GameObject player;

    public Vector2 minPos;
    public Vector2 maxPos;

    public float playerXPos;
    public float playerYPos;
    private void FixedUpdate()
    {
        playerMoveControl = player.GetComponent<PlayerMoveControl>();
        Vector3 dir = player.transform.position - this.transform.position;


        if (!playerMoveControl.matcha)
        {
            //������ ���� ī�޶� ����
            Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                this.transform.Translate(moveVector);
        }
        else
        {
            Vector3 matchaFollowVector = new Vector3(dir.x, dir.y, 0.0f);
            this.transform.Translate(matchaFollowVector * cameraSpeed * Time.deltaTime);
        }
        //������ �� ī�޶� �ȵ���
        /*Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, 0.0f, 0.0f);
        this.transform.Translate(moveVector);*/

        CameraMove();
        //LimitPosition();
        LimitPositionPlayer();
    }
    void CameraMove()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveHorizontal * cameraSpeed * Time.deltaTime, moveVertical * cameraSpeed * Time.deltaTime, 0.0f);

        this.transform.Translate(move);
        
    }
    void LimitPosition()    //ī�޶� �̵� ����
    {
        Vector3 position = transform.position;

        // ������ ����� �ʵ��� ����
        position.x = Mathf.Clamp(position.x, minPos.x, maxPos.x);
        position.y = Mathf.Clamp(position.y, minPos.y, maxPos.y);

        // ���ѵ� ��ġ�� ������Ʈ
        transform.position = position;
    }
    void LimitPositionPlayer()  //�÷��̾��� ��ġ�� ���� ī�޶� �̵� ����
    {
        Vector3 position = transform.position;

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if(p != null)
        {
            Vector2 playerPos = p.transform.position;

            // ������ ����� �ʵ��� ����
            position.x = Mathf.Clamp(position.x, playerPos.x - playerXPos, playerPos.x + playerXPos);
            position.y = Mathf.Clamp(position.y, playerPos.y - playerYPos, playerPos.y + playerYPos);
        }
        // ������ ����� �ʵ��� ����
        position.x = Mathf.Clamp(position.x, minPos.x, maxPos.x);
        position.y = Mathf.Clamp(position.y, minPos.y, maxPos.y);

        // ���ѵ� ��ġ�� ������Ʈ
        transform.position = position;
    }
}
