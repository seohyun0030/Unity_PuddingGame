using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
    
    [SerializeField] public float floorMaxRay;  //바닥 감지용 RayCast
    [SerializeField] public float rightMaxRay;   //오른쪽 벽 감지용 RayCast
    [SerializeField] public float leftMaxRay;   //왼쪽 벽 감지용 RayCast
    public float rotationSpeed = 10f;
    private bool isGravityReserved = false;
    bool isLeftMoving = false;      //왼쪽으로 이동하고 있는지 확인
    public bool isJumping = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canJump = PlayerManager.i.CanJump;
        jumpPower = PlayerManager.i.JumpPower;
        maxSpeed = PlayerManager.i.MaxSpeed;
        jumpGauge = PlayerManager.i.JumpGauge;
        bouncePower = PlayerManager.i.BouncePower;
    }
    private void Update()
    {
        jumpPower = PlayerManager.i.JumpPower;
        bouncePower = PlayerManager.i.BouncePower;

        if (Cannon.i.isAttached)
        {
            transform.rotation = Cannon.i.transform.rotation;
            rb.gravityScale = 0f;
            HandleCannon();
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

        RayCastControl();
    }
    void FixedUpdate()
    {
        if (!Cannon.i.isAttached && !Cannon.i.isFire)
        {
            if(isJumping)
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
        
    }
    void Move()    //움직임 구현
    {
        //방향키 동시에 입력받을 때 최초로 눌려진 키에만 반응
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Vector2.left, ForceMode2D.Impulse);
            isLeftMoving = true;
        }
        else if(Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(Vector2.right, ForceMode2D.Impulse);
            isLeftMoving = false;
        }
        else if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))    //두 방향키가 다 눌려진 상태일 때
        {
            if(isLeftMoving)        //왼쪽으로 이동중인 상태였을 때
                rb.AddForce(Vector2.left, ForceMode2D.Impulse);
            else                    //오른쪽으로 이동중인 상태였을 때
                rb.AddForce(Vector2.right, ForceMode2D.Impulse);
        }

        //최대 속도 넘지 않도록 설정
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
        else 
        {
            rb.AddForce(Vector3.down * PlayerManager.i.JumpGauge * jumpPower, ForceMode2D.Impulse);
        }
        StartCoroutine(CheckJumping());
    }
    void RayCastControl()  //레이 캐스트 구현
    {
        //아래로 레이 쏘기
        //레이어 마스크로 Platform인 레이어에만 레이 쏘기
        //transform.TransformDirection(Vector2.down) <-- 오브젝트 회전에 맞게 레이도 회전
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * floorMaxRay, Color.red, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if (hit.collider != null)
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
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * rightMaxRay, Color.green, 0.3f);    //레이 그리기
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
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.left) * leftMaxRay, Color.blue, 0.3f);    //레이 그리기
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
    /*void Rotate()
    {
        //경사로 보면 회전하기
        //Ray길이 20으로
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 20, LayerMask.GetMask("Platform"));
        angle = Vector2.Angle(hit3.normal, Vector2.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), 0.5f);
    }*/
    void resetRotation() //회전 상태 초기화
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Cannon.i.transform.rotation = Quaternion.identity;
        Cannon.i.isFire = false;
    }
    void Bounce(bool isLeft)   //벽에 닿으면 튕기기
    {
        if(isLeft)      //만약 왼쪽 벽에 닿았다면
            rb.AddForce(Vector3.right * bouncePower, ForceMode2D.Impulse);  //오른쪽으로 튕기기
        else            //만약 오른쪽 벽에 닿았다면
            rb.AddForce(Vector3.left * bouncePower, ForceMode2D.Impulse);  //왼쪽으로 튕기기
    }
    public void SetGravityReversed(bool reserved) //중력 반전
    {
        isGravityReserved = reserved;
    }
    IEnumerator CheckJumping()      //점프하고 2초후에는 falling상태로 변함
    {
        isJumping = true;
        yield return new WaitForSeconds(2f);
        isJumping = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //플랫폼과 닿아있으면 점핑상태가 아니므로 움직일 수 없음
        {
            isJumping = false;
            rb.gravityScale = 1f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //플랫폼과 닿아있지 않으면 점핑상태 이므로 움직일 수 있음
        {
            isJumping = true;
        }
    }
    private void HandleCannon()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!Cannon.i.IsWall(Vector2.right))
            {
                Cannon.i.RotateCannon(-90f, rb);
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // 캐논을 원래 방향으로 회전
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!Cannon.i.IsWall(Vector2.left))
            {
                Cannon.i.RotateCannon(90f, rb);
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // 캐논을 원래 방향으로 회전
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!Cannon.i.IsWall(Vector2.down))
            {
                Cannon.i.RotateCannon(180f, rb); // 아래 방향으로 180도 회전
                
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // 캐논을 원래 방향으로 회전
                return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (!Cannon.i.IsWall(Vector2.down))
            {
                Cannon.i.fire(rb);
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // 캐논을 원래 방향으로 회전
                return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow)) // 위 방향 발사 추가
        {
            if (!Cannon.i.IsWall(Vector2.up)) // 벽 감지
            {
                Cannon.i.fire(rb); // 발사 메서드 호출
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // 캐논을 원래 방향으로 회전
                return;
            }
        }
        if (Input.GetButtonUp("Horizontal"))
        {
                Cannon.i.fire(rb);
        }
        
    }

} 
