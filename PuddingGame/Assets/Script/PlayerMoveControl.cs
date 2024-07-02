using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed;
    bool canJump;
    float jumpPower;
    float maxSpeed;
    float jumpGauge;
    [SerializeField] public float floorMaxRay;  //�ٴ� ������ RayCast
    [SerializeField] public float rightMaxRay;   //������ �� ������ RayCast
    [SerializeField] public float leftMaxRay;   //���� �� ������ RayCast

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = PlayerManager.i.MoveSpeed;  //�÷��̾� �Ŵ��� ��ũ��Ʈ���� ������
        canJump = PlayerManager.i.CanJump;
        jumpPower = PlayerManager.i.JumpPower;
        maxSpeed = PlayerManager.i.MaxSpeed;
        jumpGauge = PlayerManager.i.JumpGauge;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))    //ZŰ�� ������ ���������� ����
        {
            if (canJump)    //���� ���� ������ ��
            {
                PlayerManager.i.plusJumpGauge();
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))  //ZŰ�� �� �� ����, ���� ���°� �ƴ� ��
        {
            Debug.Log("jump");
            Jump();
            PlayerManager.i.JumpGauge = 0.2f;  //���� ������ �ʱ�ȭ
            PlayerManager.i.time = 0f;         //�ð� �ʱ�ȭ
        }
        if (Input.GetButtonUp("Horizontal"))    //�¿� �̵��ϴٰ� ����Ű�� �� ���� �ӵ�
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * moveSpeed, rb.velocity.y);
        }
    }
    void FixedUpdate()
    {
        Move();
        RayCastControl();
    }
    void Move()         //������ ����
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < maxSpeed * (-1))
        {
            rb.velocity = new Vector2(maxSpeed * (-1), rb.velocity.y);
        }
    }
    void Jump()
    {
        rb.AddForce(Vector3.up * PlayerManager.i.JumpGauge * jumpPower, ForceMode2D.Impulse);
    }
    void RayCastControl()  //���� ĳ��Ʈ ����
    {
        //�Ʒ��� ���� ���
        //���̾� ����ũ�� Platform�� ���̾�� ���� ���
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * floorMaxRay, Color.red, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }

        //���������� ���� ���
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right, rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.right * rightMaxRay, Color.green, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
        if (hit1.collider != null)
        {
            if (hit1.collider.tag == "Platform")
            {
                Debug.Log("�����ʺ� �浹");
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }

        //�������� ���� ���
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.left * leftMaxRay, Color.blue, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Platform")
            {
                Debug.Log("���ʺ� �浹");
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
    }
}
