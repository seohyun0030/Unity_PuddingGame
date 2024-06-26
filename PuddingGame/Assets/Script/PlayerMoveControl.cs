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
    [SerializeField] public float floorMaxRay;  //바닥 감지용 RayCast

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = PlayerManager.i.MoveSpeed;  //플레이어 매니저 스크립트에서 가져옴
        canJump = PlayerManager.i.CanJump;
        jumpPower = PlayerManager.i.JumpPower;
        maxSpeed = PlayerManager.i.MaxSpeed;
        jumpGauge = PlayerManager.i.JumpGauge;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))    //Z키를 눌러서 점프게이지 충전
        {
            if (canJump)    //점프 가능 상태일 때
            {
                PlayerManager.i.plusJumpGauge();
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))  //Z키를 뗄 때 점프, 점프 상태가 아닐 때
        {
            Debug.Log("jump");
            Jump();
            PlayerManager.i.JumpGauge = 0.2f;  //점프 게이지 초기화
            PlayerManager.i.time = 0f;         //시간 초기화
        }
        if (Input.GetButtonUp("Horizontal"))    //좌우 이동하다가 방향키를 뗄 때의 속도
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * moveSpeed, rb.velocity.y);
        }
    }
    void FixedUpdate()
    {
        Move();
        RayCastControl();

        
    }
    void Move()         //움직임 구현
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
    void RayCastControl()  //레이 캐스트 구현
    {
        //레이어 마스크로 Platform인 레이어에만 레이 쏘기
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * floorMaxRay, Color.blue, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
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
    }
}
