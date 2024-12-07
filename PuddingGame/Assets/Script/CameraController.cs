using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController i;
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
    public GameObject LeftDoor;
    public GameObject RightDoor;
    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        // ���� "Stage2"�� ���� isEffectRunning�� �˻�
        if (!isAnimation)
        {
            if (SceneManager.GetActiveScene().name == "Stage2")
            {
                if (Stage2Camera.i.isEffectRunning)
                {
                    return; 
                }
            }

            playerMoveControl = player.GetComponent<PlayerMoveControl>();
            Vector3 dir = player.transform.position - this.transform.position;

            if (!playerMoveControl.matcha)
            {
                // ������ ���� ī�޶� ����
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

    private void Update()
    {
        if (PlayerMoveControl.i.isPlayerFixed)
        {
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

        if (p != null)
        {
            Vector2 playerPos = p.transform.position;
            if (PlayerMoveControl.i.isPlayerFixed)
            {
                position = Vector3.Lerp(transform.position, new Vector3(playerPos.x, playerPos.y + 5f, transform.position.z), cameraSpeed);

            }
            else
            {
                // ������ ����� �ʵ��� ����
                position.x = Mathf.Clamp(position.x, playerPos.x - playerXPos, playerPos.x + playerXPos);
                position.y = Mathf.Clamp(position.y, playerPos.y - playerYPos, playerPos.y + playerYPos);
            }
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

        //�̵��ϴ� �ð����� dest_1���� �̵��� �Ѵ�
        while (true)
        {
            distance = Vector3.Distance(transform.position, dest_1);  //�Ÿ� ���ϱ�

            if (transform.position == dest_1)       //Ÿ�� ��ġ�� �����ϸ� �ݺ��� ����������
                break;

            transform.position = Vector3.MoveTowards(transform.position, dest_1, animMoveSpeed);
            yield return null;
        }
        transform.position = dest_1;

        //�̵��ϴ� �ð����� dest_2���� �̵��� �Ѵ�
        while (true)
        {
            distance = Vector3.Distance(transform.position, dest_2);  //�Ÿ� ���ϱ�

            if (transform.position == dest_2)       //Ÿ�� ��ġ�� �����ϸ� �ݺ��� ����������
                break;

            transform.position = Vector3.MoveTowards(transform.position, dest_2, animMoveSpeed);
            yield return null;
        }
        transform.position = dest_2;

        //�̵��ϴ� �ð����� dest_3���� �̵��� �Ѵ�
        while (true)
        {
            distance = Vector3.Distance(transform.position, dest_3);  //�Ÿ� ���ϱ�

            if (transform.position == dest_3)       //Ÿ�� ��ġ�� �����ϸ� �ݺ��� ����������
                break;

            transform.position = Vector3.MoveTowards(transform.position, dest_3, animMoveSpeed);
            yield return null;
        }
        transform.position = dest_3;

        StartCoroutine(OpenDoor());
    }
    IEnumerator OpenDoor()
    {
        Vector3 leftPos = LeftDoor.transform.position;
        Vector3 rightPos = RightDoor.transform.position;
        while (true)
        {
            float dist = Vector3.Distance(LeftDoor.transform.position, leftPos - new Vector3(20, 0, 0));  //�Ÿ� ���ϱ�
            if (dist < 0.1f)        //���� ��ġ�� Ÿ�� ��ġ�� �Ÿ��� 0.2���� ������ �̵� ���߱�
            {
                break;
            }

            LeftDoor.transform.position = Vector3.MoveTowards(LeftDoor.transform.position, leftPos - new Vector3(20, 0, 0), 0.05f);
            RightDoor.transform.position = Vector3.MoveTowards(RightDoor.transform.position, rightPos + new Vector3(20, 0, 0), 0.05f);

            yield return null;
        }

        StartCoroutine(ZoomIn());
    }
    IEnumerator ZoomIn()
    {
        float elapsedTime = 0f;
        float currentSize = camera.orthographicSize;
        float zoomOutTime = Mathf.Abs(5 - currentSize) / zoomSpeed;

        while (elapsedTime < zoomOutTime)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 5, Time.deltaTime * zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //������ ������ �˸�
        isAnimation = false;
    }

    public IEnumerator ZoomOutCoroutine()
    {
        float elapsedTime = 0f;
        float currentSize = camera.orthographicSize;
        float zoomOutTime = Mathf.Abs(zoomOut - currentSize) / zoomSpeed;

        while (elapsedTime < zoomOutTime)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomOut, Time.deltaTime * zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        camera.orthographicSize = zoomOut; // ���������� ��Ȯ�� �ܾƿ� ũ��� ����
    }
    public void ZoomOut()
    {
        StartCoroutine(ZoomOutCoroutine());
    }
}
