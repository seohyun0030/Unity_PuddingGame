using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public static CursorController i;
    Image cursor;
    [SerializeField] Transform currentCursor;   // 커서의 현재 위치
    [SerializeField] GameObject Player;
    [SerializeField] Image arrow;           //화살표
    [SerializeField] Image arrowHead;       //화살표의 머리부분
    [SerializeField] Transform arrowHeadPos;    //삼각형 위치
    Vector3 screenPosition;
    Vector3 startPos;
    Vector3 myPos;
    public bool isLong;    //마우스 드래그가 한계선을 넘었는지 확인
    [SerializeField] float MinLength;   //마우스를 당길 때 점프 가능한 최소 길이
    [SerializeField] float MaxLength;   //최대 길이
    
    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        cursor = GetComponent<Image>();
    }
    void Update()
    {
        CursorMoving();

        if (!DialogueUI.i.dialogueText.IsActive() && !CameraController.i.isAnimation)
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPosition = Camera.main.WorldToScreenPoint(Player.transform.position);

                arrow.gameObject.SetActive(true);
                arrowHead.gameObject.SetActive(true);
                arrow.transform.position = screenPosition;      //화살표의 위치를 플레이어 오브젝트 위치로 설정
                startPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                myPos = Input.mousePosition;

                arrow.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1); //마우스를 당기는 만큼 사이즈 변경
                arrow.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos) + 180);  //마우스가 당기는 방향으로 회전
                arrowHead.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos) + 180);

                if (arrow.transform.localScale.x >= MaxLength)    //길이가 한계점을 넘어가면
                {
                    arrow.transform.localScale = new Vector2(MaxLength, 1);     //길이 고정
                    arrowHead.transform.position = arrowHeadPos.position;       //삼각형 위치도 고정된 위치에 따라 설정
                }
                else
                {
                    arrowHead.transform.position = arrowHeadPos.position;       //삼각형 위치 설정
                }
                if (arrow.transform.localScale.x <= MinLength)     //길이가 최소점보다 작으면
                {
                    changeAlpha(true);
                    isLong = false;
                    PlayerManager.i.idleAnim();
                }
                else
                {
                    changeAlpha(false);
                    isLong = true;

                    findAngle();
                }


                PlayerManager.i.JumpGauge = Mathf.Lerp(0.2f, 1, arrow.transform.localScale.x / MaxLength);
                //최대 길이와 화살표의 길이를 나눠서 0.2, 1로 선형보간
            }

            if (Input.GetMouseButtonUp(0))
            {
                changeAlpha(false);
                arrow.gameObject.SetActive(false);
                arrowHead.gameObject.SetActive(false);
            }
        }
    }
    public Vector2 GetDirection()       //점프 방향 구하기
    {
        Vector2 direction = (startPos - myPos).normalized;
        return direction;
    }
    void findAngle()
    {
        float angle = AngleInDeg(startPos, myPos);

        if (angle > -15)      //마우스를 상단에 놓는 경우
        {
            changeAlpha(true);
            isLong = false;

            flip(1);
            PlayerManager.i.idleAnim();
        }

        else if (angle > -40)   //오른쪽 3번
        {
            PlayerManager.i.char3Anim();
            flip(1);
        }

        else if (angle > -65)   //오른쪽 2번
        {
            PlayerManager.i.char2Anim();
            flip(1);
        }

        else if (angle > -90)   //오른쪽 1번
        {
            PlayerManager.i.char1Anim();
            flip(1);
        }

        else if (angle > -115)   //왼쪽 1번
        {
            PlayerManager.i.char1Anim();
            flip(-1);
        }

        else if (angle > -140)   //왼쪽 2번
        {
            PlayerManager.i.char2Anim();
            flip(-1);
        }

        else if (angle > -165)   //왼쪽 3번
        {
            PlayerManager.i.char3Anim();
            flip(-1);
        }

        else if (angle < -165)                    //왼쪽 불가 영역
        {
            changeAlpha(true);
            isLong = false;

            flip(1);
            PlayerManager.i.idleAnim();
        }

        if (angle > -90 && angle < 0)
            PlayerMoveControl.i.isLeftMoving = true;
        else if(angle > -180)
            PlayerMoveControl.i.isLeftMoving = false;
    }
    void flip(int isFlip)
    {
        GameObject player = GameObject.FindWithTag("Player");   //플레이어 찾기
        Vector3 scale = player.transform.localScale;
        scale.x = isFlip;
        player.transform.localScale = scale;
    }
    void CursorMoving()
    {
        currentCursor.position = Input.mousePosition;
        
        //Cursor.visible = false;   마우스 안보이기
        // 마우스 이동
        /*float x = Input.mousePosition.x - (Screen.width / 2);
        float y = Input.mousePosition.y - (Screen.height / 2);
        currentCursor.localPosition = Input.mousePosition;

        // 마우스 가두기 (범위 지정)
        float tmp_cursorPosX = currentCursor.localPosition.x;
        float tmp_cursorPosY = currentCursor.localPosition.y;

        float min_width = -Screen.width / 2;
        float max_width = Screen.width / 2;
        float min_height = -Screen.height / 2;
        float max_height = Screen.height / 2;
        int padding = 20;	// 값은 자유

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);

        currentCursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);*/
    }
    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }

    void changeAlpha(bool isAlpha)
    {
        Color color = cursor.color;

        color.a = isAlpha ? 0.5f : 1f;
        //isAlpha가 참이면 0.5, 거짓이면 1
        cursor.color = color;
    }
}
