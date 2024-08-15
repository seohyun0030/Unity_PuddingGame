using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;

public class CursorController : MonoBehaviour
{
    public static CursorController i;
    [SerializeField] Transform currentCursor;   // Ŀ���� ���� ��ġ
    RectTransform rectTransform;
    [SerializeField] GameObject Player;
    [SerializeField] Image mouseCircle;     //���콺 ��ġ�� ����� ��
    [SerializeField] Image arrow;           //ȭ��ǥ
    Vector3 screenPosition;
    Vector3 startPos;
    Vector3 myPos;
    public bool isLong;    //���콺 �巡�װ� �Ѱ輱�� �Ѿ����� Ȯ��
    [SerializeField] float MinLength;   //���콺�� ��� �� ���� ������ �ּ� ����
    [SerializeField] float MaxLength;   //�ִ� ����

    public Sprite[] changeSprite;
    
    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        CursorMoving();

        if (Input.GetMouseButtonDown(0))
        {
            screenPosition = Camera.main.WorldToScreenPoint(Player.transform.position);

            mouseCircle.gameObject.SetActive(true);
            arrow.gameObject.SetActive(true);
            arrow.transform.position = screenPosition;      //ȭ��ǥ�� ��ġ�� �÷��̾� ������Ʈ ��ġ�� ����
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            myPos = Input.mousePosition;

            mouseCircle.transform.localPosition = currentCursor.localPosition;      //���콺 �� ��ġ

            arrow.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1); //���콺�� ���� ��ŭ ������ ����
            arrow.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos) + 180);  //���콺�� ���� �������� ȸ��

            if (arrow.transform.localScale.x >= MaxLength)    //���̰� �Ѱ����� �Ѿ��
            {
                arrow.transform.localScale = new Vector2(MaxLength, 1);     //���� ����
            }
            if(arrow.transform.localScale.x <= MinLength)     //���̰� �ּ������� ������
            {
                changeAlpha(true);
                isLong = false;
            }
            else
            {
                changeAlpha(false);
                isLong = true;
            }

            findAngle();

            PlayerManager.i.JumpGauge = Mathf.Lerp(0.2f, 1, arrow.transform.localScale.x / MaxLength);
            //�ִ� ���̿� ȭ��ǥ�� ���̸� ������ 0.2, 1�� ��������
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseCircle.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);

            changeImage(changeSprite[2], false);

            Debug.Log(AngleInDeg(startPos, myPos));
        }
    }
    public Vector2 GetDirection()       //���� ���� ���ϱ�
    {
        Vector2 direction = (startPos - myPos).normalized;
        return direction;
    }
    void findAngle()
    {
        float angle = AngleInDeg(startPos, myPos);

        if (angle > 0)      //���콺�� ��ܿ� ���� ���
            changeImage(changeSprite[2], false);
        else if (angle > -30)   //������ 1��
            changeImage(changeSprite[0], false);

        else if (angle > -60)   //������ 2��
            changeImage(changeSprite[1], false);
        
        else if(angle > -120)   //3��
            changeImage(changeSprite[2], false);

        else if(angle > -150)   //���� 2��
            changeImage(changeSprite[1], true);

        else if (angle > -180)   //���� 1��
            changeImage(changeSprite[0], true);
    }
    public void changeImage(Sprite newSprite, bool isFlip)
    {
        GameObject player = GameObject.FindWithTag("Player");   //�÷��̾� ã��
        player.GetComponent<SpriteRenderer>().sprite = newSprite;

        //�¿���� ����
        player.GetComponent<SpriteRenderer>().flipX = isFlip;
    }
    void CursorMoving()
    {
        //Cursor.visible = false;   ���콺 �Ⱥ��̱�
        // ���콺 �̵�
        float x = Input.mousePosition.x - (Screen.width / 2);
        float y = Input.mousePosition.y - (Screen.height / 2);
        currentCursor.localPosition = new Vector2(x, y);

        // ���콺 ���α� (���� ����)
        float tmp_cursorPosX = currentCursor.localPosition.x;
        float tmp_cursorPosY = currentCursor.localPosition.y;

        float min_width = -Screen.width / 2;
        float max_width = Screen.width / 2;
        float min_height = -Screen.height / 2;
        float max_height = Screen.height / 2;
        int padding = 20;	// ���� ����

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);

        currentCursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
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
        Color color = mouseCircle.color;

        color.a = isAlpha ? 0.5f : 1f;
        //isAlpha�� ���̸� 0.5, �����̸� 1
        mouseCircle.color = color;
    }
}

/*
using System.Runtime.InteropServices;
using UnityEngine;

public static class CursorController
{
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int x, y; }

    /// <summary>
    /// Sets the cursor to a specific position relative to the upper-left corner of the main monitor screen
    /// </summary>
    /// <param name="position">Position to set, in pixels</param>
    public static void SetPosition(Vector2 position)
    {
        SetPosition((int)position.x, (int)position.y);
    }

    /// <summary>
    /// Sets the cursor to a specific position relative to the upper-left corner of the main monitor screen
    /// </summary>
    /// <param name="x">The X coordinate, in pixels</param>
    /// <param name="y">The Y coordinate, in pixels</param>
    public static void SetPosition(float x, float y)
    {
        SetPosition((int)x, (int)y);
    }

    /// <summary>
    /// Sets the cursor to a specific position relative to the upper-left corner of the main monitor screen
    /// </summary>
    /// <param name="x">The X coordinate, in pixels</param>
    /// <param name="y">The Y coordinate, in pixels</param>
    public static void SetPosition(int x, int y)
    {
        if (!SetCursorPos(x, y))
            Debug.LogError("Unknown Exception. Failed to move cursor.");
    }

    /// <summary>
    /// Returns the cursor position relative to the upper-left corner of the main monitor screen, in pixels
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetPosition()
    {
        var point = new POINT();
        if (!GetCursorPos(out point))
        {
            Debug.LogError("Unknown Exception. Failed to get cursor position, Vector2.Zero returned");
            return Vector2.zero;
        }
        return new Vector2(point.x, point.y);
    }
}*/
