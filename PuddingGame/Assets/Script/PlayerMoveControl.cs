using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMoveControl : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed;
    bool canJump;
    float jumpPower;
    float maxSpeed;
    float jumpGauge;
    public float angle;    //회전한 값
    float bouncePower;
    bool isSliding;        //미끄러지고 있는지 확인
    float friction;
    
    [SerializeField] public float floorMaxRay;  //바닥 감지용 RayCast
    [SerializeField] public float rightMaxRay;   //오른쪽 벽 감지용 RayCast
    [SerializeField] public float leftMaxRay;   //왼쪽 벽 감지용 RayCast
    public float rotationSpeed = 10f;
    private bool isGravityReserved = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canJump = PlayerManager.i.CanJump;
        jumpPower = PlayerManager.i.JumpPower;
        maxSpeed = PlayerManager.i.MaxSpeed;
        jumpGauge = PlayerManager.i.JumpGauge;
        bouncePower = PlayerManager.i.BouncePower;
        friction = PlayerManager.i.Friction;
    }
    private void Update()
    {   
        
        if (Cannon.i.isAttached)
        {
            float rotate = Input.GetAxis("Horizontal");
            Vector3 rotation = new Vector3(0, 0, -rotate);
            transform.Rotate(rotation * Cannon.i.roatationSpeed * Time.deltaTime);
            if (Input.GetButtonUp("Horizontal"))
            {
                Cannon.i.fire(rb);
                
            }
        }

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

        /*if (Input.GetButtonUp("Horizontal"))    //좌우 이동하다가 방향키를 뗄 때의 속도
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * moveSpeed, rb.velocity.y);
        }*/

        RayCastControl();
    }
    void FixedUpdate()
    {
        if (!Cannon.i.isAttached && !Cannon.i.isFire)
        {
            Move();
            
            if (isGravityReserved)
            {
                rb.gravityScale = -1f;
            }
            else
            {
                rb.gravityScale = 1f;
            }
            
        }
        //Slide();
        
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
        if (rb.gravityScale > 0f)
        {
            rb.AddForce(Vector3.up * PlayerManager.i.JumpGauge * jumpPower, ForceMode2D.Impulse);
        }
        else {
            rb.AddForce(Vector3.down * PlayerManager.i.JumpGauge * jumpPower, ForceMode2D.Impulse);
        }
    }
    void RayCastControl()  //레이 캐스트 구현
    {
        //아래로 레이 쏘기
        //레이어 마스크로 Platform인 레이어에만 레이 쏘기
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.down * floorMaxRay, Color.red, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                canJump = true;
                if (Cannon.i.isFire)
                {
                    resetRotation();
                }
            }
            else
            {
                canJump = false;
            }
        }
        else
        {
            canJump = true;
        }

        //오른쪽으로 레이 쏘기
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right, rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.right * rightMaxRay, Color.green, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if (hit1.collider != null)
        {
            if (hit1.collider.tag == "Platform")
            {
                canJump = true;
                if (Cannon.i.isFire)
                {
                    resetRotation();
                }
                Bounce(false);
            }
            else
            {
                canJump = false;
                
            }
        }

        //왼쪽으로 레이 쏘기
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.left * leftMaxRay, Color.blue, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Platform")
            {
                canJump = true;
                if (Cannon.i.isFire)
                {
                    resetRotation();
                }
                Bounce(true);
            }
            else
            {
                canJump = false;
            }
        }
    }
    void Rotate()
    {
        //경사로 보면 회전하기
        //Ray길이 20으로
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 20, LayerMask.GetMask("Platform"));
        angle = Vector2.Angle(hit3.normal, Vector2.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), 0.5f);
    }
    void resetRotation()
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Cannon.i.isFire = false;
    }
    void Bounce(bool isLeft)   //벽에 닿으면 튕기기
    {
        if(isLeft)      //만약 왼쪽 벽에 닿았다면
            rb.AddForce(Vector3.right * bouncePower, ForceMode2D.Impulse);  //오른쪽으로 튕기기
        else            //만약 오른쪽 벽에 닿았다면
            rb.AddForce(Vector3.left * bouncePower, ForceMode2D.Impulse);  //왼쪽으로 튕기기
    }
    /*void Slide()
    {
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position, Vector2.down, floorMaxRay, LayerMask.GetMask("Platform"));

        if (Vector2.Angle(hit4.normal, Vector2.up) > 0)
        {
            Vector2 velocity = rb.velocity;

            if (velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            //Vector2 frictionForce = Vector2.ProjectOnPlane(velocity, slopeNormal) * friction;
            Vector2 frictionForce = new Vector2(rb.velocity.normalized.x * friction, rb.velocity.y);

            rb.velocity += frictionForce * Time.fixedDeltaTime;
        }
    }*/
    public void SetGravityReversed(bool reserved)
    {
        isGravityReserved = reserved;
    }
}
