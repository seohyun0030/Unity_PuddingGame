using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public static CursorController i;
    public Texture2D originalCursor;    //���� Ŀ�� �̹���
    Texture2D transparentCursor;        //����ȭ�� Ŀ�� �̹���
    [SerializeField] Transform currentCursor;   // Ŀ���� ���� ��ġ
    [SerializeField] GameObject Player;
    [SerializeField] Image arrow;           //ȭ��ǥ
    [SerializeField] Image arrowHead;       //ȭ��ǥ�� �Ӹ��κ�
    [SerializeField] Transform arrowHeadPos;    //�ﰢ�� ��ġ
    Vector3 screenPosition;
    Vector3 startPos;
    Vector3 myPos;
    public bool isLong;    //���콺 �巡�װ� �Ѱ輱�� �Ѿ����� Ȯ��
    [SerializeField] float MinLength;   //���콺�� ��� �� ���� ������ �ּ� ����
    [SerializeField] float MaxLength;   //�ִ� ����
    
    private void Awake()
    {
        i = this;
        GameManager.isPause = false;
    }
    private void Start()
    {
        Cursor.SetCursor(originalCursor, Vector2.zero, CursorMode.Auto);
        transparentCursor = CreateTransparentTexture(originalCursor, 0.5f);
    }
    void Update()
    {
        if (DialogueUI.i == null || CameraController.i == null)
            return;

        if (!DialogueUI.i.dialogueText.IsActive() && !CameraController.i.isAnimation && !GameManager.isPause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPosition = Camera.main.WorldToScreenPoint(Player.transform.position);

                arrow.gameObject.SetActive(true);
                arrowHead.gameObject.SetActive(true);
                arrow.transform.position = screenPosition;      //ȭ��ǥ�� ��ġ�� �÷��̾� ������Ʈ ��ġ�� ����
                startPos = Input.mousePosition;

                SfxManager.i.PlaySound("ButtonTouch");     //ȿ���� ���
            }

            if (Input.GetMouseButton(0))
            {
                myPos = Input.mousePosition;

                arrow.transform.localScale = new Vector2(Vector3.Distance(myPos, startPos), 1); //���콺�� ���� ��ŭ ������ ����
                arrow.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos) + 180);  //���콺�� ���� �������� ȸ��
                arrowHead.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(startPos, myPos) + 180);

                if (arrow.transform.localScale.x >= MaxLength)    //���̰� �Ѱ����� �Ѿ��
                {
                    arrow.transform.localScale = new Vector2(MaxLength, 1);     //���� ����
                    arrowHead.transform.position = arrowHeadPos.position;       //�ﰢ�� ��ġ�� ������ ��ġ�� ���� ����
                }
                else
                {
                    arrowHead.transform.position = arrowHeadPos.position;       //�ﰢ�� ��ġ ����
                }
                if (arrow.transform.localScale.x <= MinLength)     //���̰� �ּ������� ������
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
                //�ִ� ���̿� ȭ��ǥ�� ���̸� ������ 0.2, 1�� ��������
            }

            if (Input.GetMouseButtonUp(0))
            {
                changeAlpha(false);
                arrow.gameObject.SetActive(false);
                arrowHead.gameObject.SetActive(false);
            }
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

        if (angle > -15)      //���콺�� ��ܿ� ���� ���
        {
            changeAlpha(true);
            isLong = false;

            flip(1);
            PlayerManager.i.idleAnim();
        }

        else if (angle > -40)   //������ 3��
        {
            PlayerManager.i.char3Anim();
            flip(1);
        }

        else if (angle > -65)   //������ 2��
        {
            PlayerManager.i.char2Anim();
            flip(1);
        }

        else if (angle > -90)   //������ 1��
        {
            PlayerManager.i.char1Anim();
            flip(1);
        }

        else if (angle > -115)   //���� 1��
        {
            PlayerManager.i.char1Anim();
            flip(-1);
        }

        else if (angle > -140)   //���� 2��
        {
            PlayerManager.i.char2Anim();
            flip(-1);
        }

        else if (angle > -165)   //���� 3��
        {
            PlayerManager.i.char3Anim();
            flip(-1);
        }

        else if (angle < -165)                    //���� �Ұ� ����
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
        GameObject player = GameObject.FindWithTag("Player");   //�÷��̾� ã��
        Vector3 scale = player.transform.localScale;
        scale.x = isFlip;
        player.transform.localScale = scale;
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
        if (!isAlpha)
        {
            Cursor.SetCursor(originalCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(transparentCursor, Vector2.zero, CursorMode.Auto);
        }
    }
    Texture2D CreateTransparentTexture(Texture2D original, float alpha)
    {
        Texture2D newTexture = new Texture2D(original.width, original.height);
        Color[] pixels = original.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].a *= alpha; // ���� ���� ����
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
    }
}
