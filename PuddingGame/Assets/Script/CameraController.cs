using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera camera;
    public float cameraSpeed;
    private PlayerMoveControl playerMoveControl;
    public GameObject player;

    public Vector2 minPos;
    public Vector2 maxPos;

    public float playerXPos;
    public float playerYPos;

    public float zoomOut;
    public float zoomSpeed;
    public float animMoveSpeed;
    public Vector3 dest_1;
    public Vector3 dest_2;
    public Vector3 dest_3;
    public bool isAnimation;
    private void Start()
    {
        camera = GetComponent<Camera>();
        StartCoroutine(Stage1Animation());
    }
    private void FixedUpdate()
    {
        if(!isAnimation)
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

            CameraMove();
            LimitPositionPlayer();
        }
    }
    void CameraMove()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveHorizontal * cameraSpeed * Time.deltaTime, moveVertical * cameraSpeed * Time.deltaTime, 0.0f);

        this.transform.Translate(move);
        
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
    public IEnumerator Stage1Animation()
    {
        //������ �������� ��Ÿ��
        isAnimation = true;
        float elapsedTime = 0f;
        float currentSize = camera.orthographicSize;
        float zoomOutTime = Mathf.Abs(zoomOut - currentSize) / zoomSpeed;
        float distance;
        //�� �ƿ��� �� �� ������ �� �ƿ��� �Ѵ�
        while (elapsedTime < zoomOutTime)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomOut, Time.deltaTime * zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        MoveAnimation(dest_1);
        /*//�̵��ϴ� �ð����� dest_1���� �̵��� �Ѵ�
        float moveTime = Vector3.Distance(transform.position, dest_1) / animMoveSpeed;
        elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            distance = Vector3.Distance(transform.position, dest_1);  //�Ÿ� ���ϱ�
            if (distance < 0.2f)        //���� ��ġ�� Ÿ�� ��ġ�� �Ÿ��� 0.2���� ������ �̵� ���߱�
            {
                break;
            }
            transform.position = Vector3.Lerp(transform.position, dest_1, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = dest_1;

        //�̵��ϴ� �ð����� dest_2���� �̵��� �Ѵ�
        moveTime = Vector3.Distance(transform.position, dest_2) / animMoveSpeed;
        elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            distance = Vector3.Distance(transform.position, dest_2);  //�Ÿ� ���ϱ�
            if (distance < 0.2f)        //���� ��ġ�� Ÿ�� ��ġ�� �Ÿ��� 0.2���� ������ �̵� ���߱�
            {
                break;
            }
            transform.position = Vector3.Lerp(transform.position, dest_2, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = dest_2;

        //�̵��ϴ� �ð����� dest_3���� �̵��� �Ѵ�
        moveTime = Vector3.Distance(transform.position, dest_3) / animMoveSpeed;
        elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            distance = Vector3.Distance(transform.position, dest_3);  //�Ÿ� ���ϱ�
            if (distance < 0.2f)        //���� ��ġ�� Ÿ�� ��ġ�� �Ÿ��� 0.2���� ������ �̵� ���߱�
            {
                break;
            }
            transform.position = Vector3.Lerp(transform.position, dest_3, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = dest_3;*/

        //������ ������ �˸�
        isAnimation = false;
    }
    void MoveAnimation(Vector3 dest)
    {
        float moveTime = Vector3.Distance(transform.position, dest) / animMoveSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            float distance = Vector3.Distance(transform.position, dest);  //�Ÿ� ���ϱ�
            if (distance < 0.2f)        //���� ��ġ�� Ÿ�� ��ġ�� �Ÿ��� 0.2���� ������ �̵� ���߱�
            {
                break;
            }
            transform.position = Vector3.Lerp(transform.position, dest, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
        }
    }
}
