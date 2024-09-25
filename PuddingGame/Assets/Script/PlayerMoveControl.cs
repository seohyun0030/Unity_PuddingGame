using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

public class PlayerMoveControl : MonoBehaviour
{
    public static PlayerMoveControl i;
    Rigidbody2D rb;
    public bool canJump;
    float jumpPower;
    public float angle;    //회전한 값
    public bool matcha = false; //녹차잎 사용
    bool rasberry = false; //라즈베리 사용
    bool chocolate = false; //초콜릿 사용
    [SerializeField] float fallingSpeed = -5f; // 떨어지는 속도
    [SerializeField] float flyingSpeed = 10f; // 날아가는 속도
    [SerializeField] float limitDistance = 10f; // 떨어지기 시작하는 거리
    [SerializeField] float clingJumpForce = .5f;
    [SerializeField] float clingRay = 1f;
    [SerializeField] float rayOffset = .1f;
    public GameObject particlePrefab;

    [SerializeField] public float floorMaxRay;  //바닥 감지용 RayCast
    [SerializeField] public float rightMaxRay;   //오른쪽 벽 감지용 RayCast
    [SerializeField] public float leftMaxRay;   //왼쪽 벽 감지용 RayCast
    public float rotationSpeed = 10f;
    private bool isGravityReserved = false;
    public bool isLeftMoving;      //왼쪽으로 이동하고 있는지 확인
    public bool isJumping = false;  //jumping상태
    public bool isFalling = false;  //falling상태
    public bool isGrounded = true;     //땅에 있는지 확인
    public bool jumpPlatform = false;
    public bool isLong;
    private bool canEmitParticles = true;
    public float particleCoolDownTime = 1f;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canJump = PlayerManager.i.CanJump;
        jumpPower = PlayerManager.i.JumpPower;
    }
    private void Update()
    {
        if (!DialogueUI.i.dialogueText.IsActive())
        {
            Time.timeScale = 1f;
            jumpPower = PlayerManager.i.JumpPower;


            RayCastControl();

            isLong = CursorController.i.isLong;

            if (Input.GetMouseButtonUp(0) && canJump && isGrounded && isLong || (jumpPlatform && Input.GetMouseButtonUp(0)))     //마우스를 뗐을 때 점프가능 상태이고 땅에 있으면 점프 가능
            {

                Move();
                PlayerManager.i.JumpGauge = 0.2f;  //점프 게이지 초기화
            }
            if (chocolate && Input.GetMouseButtonUp(0))
            {
                chocolate = false;
                isRotated = false;
                Move();
                transform.rotation = Quaternion.Euler(0, 0, 0);
                PlayerManager.i.JumpGauge = 0.2f;

            }
        }
        
    }
    void FixedUpdate()
    {

            if (isGravityReserved)
            {
                rb.gravityScale = -1f;
            }
            else
            {
                rb.gravityScale = 1f;
            }
            if (matcha)
            {
                rb.velocity = new Vector2(rb.velocity.x, fallingSpeed);
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2(-2f, rb.velocity.y);
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2(2f, rb.velocity.y);
                }
            }
            if (rasberry)
            {
                rb.gravityScale = 0f;
            }
            if (chocolate)
            {
                Chocolate();
            }
        
        
    }
    public void Move()     //움직임 구현
    {
        rb.velocity = Vector2.zero;  // 현재의 수직 속도 초기화

        Vector2 MoveDirection = CursorController.i.GetDirection();
        //점프 높이 일정하도록
        float jumpForce = Mathf.Sqrt(2 * rb.mass * Physics2D.gravity.magnitude * jumpPower * PlayerManager.i.JumpGauge);

        if (MoveDirection.y <= 0)       //마우스가 플레이어 상단으로 드래그되면 (y값이 0보다 작아지면)
            MoveDirection.y = 0;        //x좌표로만 이동하도록 y를 0으로

        rb.AddForce(MoveDirection * jumpForce, ForceMode2D.Impulse);

        StartCoroutine(CheckJumping());
        StartCoroutine(JumpDelay());
        jumpPlatform = false;

        PlayerManager.i.Animation("jump");

    }
    
    void RayCastControl()  //레이 캐스트 구현
    {
        Vector2 origin = GetComponent<Collider2D>().bounds.center;
        //아래로 레이 쏘기
        //레이어 마스크로 Platform인 레이어에만 레이 쏘기
        //transform.TransformDirection(Vector2.down) <-- 오브젝트 회전에 맞게 레이도 회전
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.TransformDirection(Vector2.down), floorMaxRay, LayerMask.GetMask("Platform"));
        //isGrounded = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, transform.TransformDirection(Vector2.down) * floorMaxRay, Color.red, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if (hit.collider != null)
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
        /*else
        {
            canJump = false;
        }*/

        //오른쪽으로 레이 쏘기
        RaycastHit2D hit1 = Physics2D.Raycast(origin, transform.TransformDirection(Vector2.right), rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, transform.TransformDirection(Vector2.right) * rightMaxRay, Color.green, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if (hit1.collider != null)
        {
            if (hit1.collider.tag == "Platform")
            {
                canJump = true;

                //Bounce(false);
            }
            else
            {
                canJump = false;
            }
        }

        //왼쪽으로 레이 쏘기
        RaycastHit2D hit2 = Physics2D.Raycast(origin, transform.TransformDirection(Vector2.left), leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, transform.TransformDirection(Vector2.left) * leftMaxRay, Color.blue, 0.3f);    //레이 그리기
        //레이에 맞은 바닥의 태그가 Platform일 때만 점프 가능
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Platform")
            {
                canJump = true;

                //Bounce(true);
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
    public void SetGravityReversed(bool reserved) //중력 반전
    {
        isGravityReserved = reserved;
        rb.gravityScale = reserved ? -1f : 1f;
    }
    IEnumerator JumpDelay()     //한번 점프하고 1초 뒤에 점프가능
    {
        yield return new WaitForSeconds(1f);
        canJump = true;
    }
    IEnumerator CheckJumping()      //점프하고 2초후에는 falling상태로 변함
    {
        isJumping = true;
        yield return new WaitForSeconds(2f);
        isJumping = false;
        isFalling = true;
    }
    IEnumerator ParticleCooldown()
    {
        canEmitParticles = false;
        yield return new WaitForSeconds(particleCoolDownTime);
        Debug.Log("co");
        canEmitParticles = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //플랫폼과 닿아있으면 점핑상태가 아니므로 움직일 수 없음
        {
            isJumping = false;
            isFalling = false;
            isGrounded = true;

            Falling(collision);

            if (canEmitParticles)
            {
                Vector2 contactPoint = collision.GetContact(0).point;
                GameObject particle = Instantiate(particlePrefab, contactPoint, Quaternion.identity);
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    //Destroy(particle, ps.main.duration);
                }
                StartCoroutine(ParticleCooldown());
                
            }
            Debug.Log(canEmitParticles.ToString());
            if (gameObject.activeSelf)
                StartCoroutine(isStopMoving());

            if (matcha)
            {
                rb.velocity = Vector3.zero;
                matcha = false;
            }
            if (rasberry)
            {
                rb.velocity = Vector3.zero;
                rasberry = false;
            }
            PlayerManager.i.Animation("idle");
        }
    }
    public bool playerActive = true;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isFalling = false;      //플랫폼에 닿아있는 내내 falling상태 아님

            //Test용     움직이는 플랫폼으로 이동하다가 다른 플랫폼에 닿으면 점프안됨
            //canJump = true;
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //플랫폼과 닿아있지 않으면 점핑상태 이므로 움직일 수 있음
        {
            //isJumping = true;         한번 충돌하고 나서 움직이지 못하도록
            isGrounded = false;
        }
    }
    public void Falling(Collision2D collision)   //낙사 구현
    {
        //임계점보다 속도가 더 빨라지면 추락

        Vector2 impactVelocity = collision.relativeVelocity;

        if (impactVelocity.y > PlayerManager.i.fallingSpeed)
        {
            gameObject.SetActive(false);
            playerActive = false;
        }
    }
    IEnumerator isStopMoving()
    {
        /*float xPos = transform.position.x;
        float yPos = transform.position.y;
        
        yield return new WaitForSeconds(1f);

        Debug.Log(xPos+" "+yPos);
        Debug.Log(transform.position.x + " " + transform.position.y);
        //1초동안 좌표가 변하지 않았으면 점프 가능
        if (xPos == transform.position.x && (transform.position.y >= yPos - 0.1f || transform.position.y <= yPos + 0.1f))
        {
            canJump = true;
        }*/

        yield return new WaitForSeconds(1f);        //1초 기다리기

        //임계치보다 속도가 낮으면 점프 가능
        //canJump = rb.velocity.magnitude < threshold && Mathf.Abs(rb.angularVelocity) < threshold;
        canJump = rb.velocity.magnitude < 0.01f;
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
    public void ToppingJump(int i)   //토핑을 쓰면 나타나는 점프 구현
    {
        StopCoroutine(CheckJumping());
        StartCoroutine(CheckJumping());

        float jumpForce = Mathf.Sqrt(2 * rb.mass * Physics2D.gravity.magnitude * jumpPower);

        if (i == 0) //레몬
        {
            if (rb.gravityScale > 0f)
            {
                //rb.velocity = Vector2.zero;
                rb.velocity = new Vector2(rb.velocity.x, 0);  // 현재의 수직 속도 초기화
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);  // 현재의 수직 속도 초기화
                rb.AddForce(Vector3.down * jumpForce, ForceMode2D.Impulse);
            }
        }
        else if (i == 1)    //체리
        {
            Vector3 jumpDirection;

            if(isLeftMoving)
                jumpDirection = new Vector3(-1, 1, 0).normalized;
            else
                jumpDirection = new Vector3(1, 1, 0).normalized;

            rb.velocity = Vector2.zero;
            //rb.velocity = new Vector2(rb.velocity.x, 0);  // 현재의 수직 속도 초기화
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        }
        else if(i == 2) // 초콜렛
        {
            if(isJumping || isFalling)
            {
                chocolate = true;
            }
        }
        else if(i == 3) // 라즈베리
        {
            if(isJumping || isFalling)
            {
                rasberry = true;
                rb.velocity = Vector2.zero;
                StartCoroutine(Rasberry_Co());
                
            }
        }
        else if(i == 4) // 녹차
        {
            
            if (!isFalling)
            {
                matcha = true;
            }
            
        }
        
    }
    Vector2 flyDirection;
    private float flyingDistance = 0f; // 날아간 거리

    IEnumerator Rasberry_Co()
    {
        //Vector2 startPosition = transform.position;
        float inputTime = 0f;
        flyDirection = Vector2.zero;
        flyingDistance = 0f;
        
        while (rasberry && inputTime < 2f)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                flyDirection = Vector2.up;
                break;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                flyDirection = Vector2.down;
                break;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                flyDirection = Vector2.left;
                break;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                flyDirection = Vector2.right;
                break;
            }
            inputTime += Time.deltaTime;
            yield return null;
        }
        if (flyDirection != Vector2.zero)
        {
            rb.gravityScale = 0f;
            rb.velocity = flyDirection.normalized * flyingSpeed;
            while (rasberry && flyingDistance < limitDistance)
            {
                
                flyingDistance += rb.velocity.magnitude * Time.deltaTime;
                yield return null;
            }
            rasberry = false;
            rb.gravityScale = 1f;
        }
        else
        {
            rb.velocity = Vector2.zero;
            rasberry = false;
            rb.gravityScale = 1f;
        }
        rasberry = false;
    }
    private bool isRotated = false; // 회전
    private void Chocolate()
    {
        float colliderHalfWidth = GetComponent<Collider2D>().bounds.extents.x;
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, clingRay, LayerMask.GetMask("Platform"));
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, clingRay, LayerMask.GetMask("Platform"));
        if (left.collider != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            transform.position = new Vector2(left.point.x  - colliderHalfWidth - rayOffset, transform.position.y);
            if (!isRotated)
            {
                transform.Rotate(0, 0, -90);
                isRotated = true;
            }


        }
        else if (right.collider != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            transform.position = new Vector2(right.point.x + colliderHalfWidth + rayOffset, transform.position.y);
            if (!isRotated)
            {
                transform.Rotate(0, 0, 90);
                isRotated = true;
            }
        }

    }
}
public static class Vector2Extensions
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }
}
