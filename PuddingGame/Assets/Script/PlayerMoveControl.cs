using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMoveControl : MonoBehaviour
{
    public static PlayerMoveControl i;
    Rigidbody2D rb;
    public bool canJump;
    float jumpPower;
    public float angle;    //ȸ���� ��
    public bool matcha = false; //������ ���
    bool rasberry = false; //����� ���
    bool chocolate = false; //���ݸ� ���
    [SerializeField] float fallingSpeed = -5f; // �������� �ӵ�
    [SerializeField] float flyingSpeed = 10f; // ���ư��� �ӵ�
    [SerializeField] float limitDistance = 10f; // �������� �����ϴ� �Ÿ�
    [SerializeField] float clingJumpForce = .5f;
    [SerializeField] float clingRay = 1f;
    [SerializeField] float rayOffset = .1f;

    public ParticleSystem yellowParticle;        //��� ��ƼŬ �ý���
    public ParticleSystem mintParticle;        //��Ʈ ��ƼŬ �ý���

    public GameObject deathYellow;           //��� ���� �̹���
    public GameObject deathMint;             //��Ʈ ���� �̹���

    [SerializeField] public float floorMaxRay;  //�ٴ� ������ RayCast
    [SerializeField] public float rightMaxRay;   //������ �� ������ RayCast
    [SerializeField] public float leftMaxRay;   //���� �� ������ RayCast
    public float rotationSpeed = 10f;
    private bool isGravityReserved = false;
    public bool isLeftMoving;      //�������� �̵��ϰ� �ִ��� Ȯ��
    public bool isJumping = false;  //jumping����
    public bool isFalling = false;  //falling����
    public bool isGrounded = true;     //���� �ִ��� Ȯ��
    public bool isCling = false; // ���� �پ�����
    public bool jumpPlatform = false;
    public bool isLong;
    private bool canEmitParticles = true;
    public float particleCoolDownTime = 1f;
    public bool isPlayerFixed = false;

    SkeletonAnimation skeletonAnimation;

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
        if (!DialogueUI.i.dialogueText.IsActive() && !GameManager.isPause)
        {
            // ���� ���� "Stage2"�� ���� isEffectRunning �˻�
            if (SceneManager.GetActiveScene().name == "Stage2")
            {
                if (Stage2Camera.i.isEffectRunning)
                {
                    return;
                }
            }

            Time.timeScale = 1f;
            jumpPower = PlayerManager.i.JumpPower;

            RayCastControl();

            isLong = CursorController.i.isLong;

            if (Input.GetMouseButtonUp(0) && canJump && isGrounded && isLong || (jumpPlatform && Input.GetMouseButtonUp(0))) // ���콺�� ���� �� ���� ���� �����̰� ���� ������ ���� ����
            {
                if (!GameManager.isPause)
                {
                    Move();
                    PlayerManager.i.JumpGauge = 0.2f; // ���� ������ �ʱ�ȭ
                }
            }

            if (chocolate && Input.GetMouseButtonUp(0))
            {
                if (!isCling) return;
                chocolate = false;
                isRotated = false;
                Move();
                transform.rotation = Quaternion.Euler(0, 0, 0);
                PlayerManager.i.JumpGauge = 0.2f;
                chocolateSound = false;
                isCling = false;
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
        if(SceneManager.GetActiveScene().name == "Stage1" && !EndTrigger.i.isTriggered)
            {
            transform.SetParent(null);
        }

    }
    public void Move()     //������ ����
    {
        rb.velocity = Vector2.zero;  // ������ ���� �ӵ� �ʱ�ȭ

        Vector2 MoveDirection = CursorController.i.GetDirection();
        //���� ���� �����ϵ���
        float jumpForce = Mathf.Sqrt(2 * rb.mass * Physics2D.gravity.magnitude * jumpPower * PlayerManager.i.JumpGauge);

        if (MoveDirection.y <= 0)       //���콺�� �÷��̾� ������� �巡�׵Ǹ� (y���� 0���� �۾�����)
            MoveDirection.y = 0;        //x��ǥ�θ� �̵��ϵ��� y�� 0����

        rb.AddForce(MoveDirection * jumpForce, ForceMode2D.Impulse);

        StartCoroutine(CheckJumping());
        StartCoroutine(JumpDelay());
        jumpPlatform = false;

        PlayerManager.i.jumpAnim();

        SfxManager.i.PlaySound("Jump");
    }
    
    void RayCastControl()  //���� ĳ��Ʈ ����
    {
        Vector2 origin = GetComponent<Collider2D>().bounds.center;
        //�Ʒ��� ���� ���
        //���̾� ����ũ�� Platform�� ���̾�� ���� ���
        //transform.TransformDirection(Vector2.down) <-- ������Ʈ ȸ���� �°� ���̵� ȸ��
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.TransformDirection(Vector2.down), floorMaxRay, LayerMask.GetMask("Platform"));
        //isGrounded = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, transform.TransformDirection(Vector2.down) * floorMaxRay, Color.red, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
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

        //���������� ���� ���
        RaycastHit2D hit1 = Physics2D.Raycast(origin, transform.TransformDirection(Vector2.right), rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, transform.TransformDirection(Vector2.right) * rightMaxRay, Color.green, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
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

        //�������� ���� ���
        RaycastHit2D hit2 = Physics2D.Raycast(origin, transform.TransformDirection(Vector2.left), leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(origin, transform.TransformDirection(Vector2.left) * leftMaxRay, Color.blue, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
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
        //���� ���� ȸ���ϱ�
        //Ray���� 20����
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 20, LayerMask.GetMask("Platform"));
        angle = Vector2.Angle(hit3.normal, Vector2.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), 0.5f);
    }*/
    //void resetRotation() //ȸ�� ���� �ʱ�ȭ
    //{
    //    rb.velocity = Vector3.zero;
    //    transform.rotation = Quaternion.identity;
    //    Cannon.i.transform.rotation = Quaternion.identity;
    //    Cannon.i.isFire = false;
    //}
    public void SetGravityReversed(bool reserved) //�߷� ����
    {
        isGravityReserved = reserved;
        rb.gravityScale = reserved ? -1f : 1f;
    }
    IEnumerator JumpDelay()     //�ѹ� �����ϰ� 1�� �ڿ� ��������
    {
        yield return new WaitForSeconds(1f);
        canJump = true;
    }
    IEnumerator CheckJumping()      //�����ϰ� 2���Ŀ��� falling���·� ����
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
        canEmitParticles = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //�÷����� ��������� ���λ��°� �ƴϹǷ� ������ �� ����
        {
            isJumping = false;
            isFalling = false;
            isGrounded = true;

            Falling(collision);

            if (canEmitParticles)
            {
                PlayParticle();
                StartCoroutine(ParticleCooldown());

                var script = collision.gameObject.GetComponent<MonoBehaviour>();

                if (script == null || script.GetType().Name != "DeathPlatform")     //�浹�� �÷����� ��� �÷����� �ƴ϶��
                {
                    SfxManager.i.PlaySound("Collision");   //�浹 ȿ���� ���
                }
            }

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
            if (SceneManager.GetActiveScene().name == "Stage1" && EndTrigger.i.isTriggered)
            {
                GameObject cart = GameObject.Find("Cart");
                transform.SetParent(cart.transform);  // �÷��̾ īƮ�� �ڽ����� ����

                if (!isPlayerFixed)
                {
                    rb.velocity = Vector2.zero;
                    Vector2 contactPoint = collision.GetContact(0).point;
                    rb.transform.position = contactPoint;
                    isPlayerFixed = true;
                }
                
            }

            PlayerManager.i.idleAnim();
        }
    }

    public bool playerActive = true;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isFalling = false;      //�÷����� ����ִ� ���� falling���� �ƴ�

            //Test��     �����̴� �÷������� �̵��ϴٰ� �ٸ� �÷����� ������ �����ȵ�
            //canJump = true;
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //�÷����� ������� ������ ���λ��� �̹Ƿ� ������ �� ����
        {
            //isJumping = true;         �ѹ� �浹�ϰ� ���� �������� ���ϵ���
            isGrounded = false;
        }
    }
    public void Falling(Collision2D collision)   //���� ����
    {
        //�Ӱ������� �ӵ��� �� �������� �߶�

        Vector2 impactVelocity = collision.relativeVelocity;

        if (impactVelocity.y > PlayerManager.i.fallingSpeed)
        {
            playerActive = false;

            gameObject.SetActive(false);

            SfxManager.i.PlaySound("Death");        //���� ȿ���� ���

            ShowDeath(gameObject.transform.position);
        }
    }
    public void ShowDeath(Vector2 position)
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        if (skeletonAnimation != null)
        {
            // ���� ��Ų�� �̸��� Ȯ��
            string currentSkinName = skeletonAnimation.Skeleton.Skin.Name;

            if (currentSkinName == "1")     
            {
                deathYellow.SetActive(true);
                deathYellow.transform.position = position;
            }
            else if(currentSkinName == "2")
            {
                deathMint.SetActive(true);
                deathMint.transform.position = position;
            }
        }
    }
    public void Rebirth()
    {
        //���� �̹��� ����
        if (deathYellow != null)
            deathYellow.SetActive(false);   
        
        if(deathMint != null)
            deathMint.SetActive(false);

    }
    public void ChangeParticleColor(string color)
    {
        if (color == "yellow")
        {
            yellowParticle.gameObject.SetActive(true);
            mintParticle.gameObject.SetActive(false);
        }
        else if (color == "mint")
        {
            mintParticle.gameObject.SetActive(true);
            yellowParticle.gameObject.SetActive(false);
        }
    }
    public void PlayParticle()
    {
        if (yellowParticle.gameObject.activeSelf)
        {
            yellowParticle.Play();
        }
        else if (mintParticle.gameObject.activeSelf)
        {
            mintParticle.Play();
        }
    }
    /*
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
                Cannon.i.RotateCannon(0f, rb); // ĳ���� ���� �������� ȸ��
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
                Cannon.i.RotateCannon(0f, rb); // ĳ���� ���� �������� ȸ��
                return;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!Cannon.i.IsWall(Vector2.down))
            {
                Cannon.i.RotateCannon(180f, rb); // �Ʒ� �������� 180�� ȸ��
                
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // ĳ���� ���� �������� ȸ��
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
                Cannon.i.RotateCannon(0f, rb); // ĳ���� ���� �������� ȸ��
                return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow)) // �� ���� �߻� �߰�
        {
            if (!Cannon.i.IsWall(Vector2.up)) // �� ����
            {
                Cannon.i.fire(rb); // �߻� �޼��� ȣ��
            }
            else
            {
                Cannon.i.RotateCannon(0f, rb); // ĳ���� ���� �������� ȸ��
                return;
            }
        }
        if (Input.GetButtonUp("Horizontal"))
        {
                Cannon.i.fire(rb);
        }
        
    }
    */
    public void ToppingJump(int i)   //������ ���� ��Ÿ���� ���� ����
    {
        StopCoroutine(CheckJumping());
        StartCoroutine(CheckJumping());

        float jumpForce = Mathf.Sqrt(2 * rb.mass * Physics2D.gravity.magnitude * jumpPower);

        if (i == 0) //����
        {
            if (rb.gravityScale > 0f)
            {
                //rb.velocity = Vector2.zero;
                rb.velocity = new Vector2(rb.velocity.x, 0);  // ������ ���� �ӵ� �ʱ�ȭ
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);  // ������ ���� �ӵ� �ʱ�ȭ
                rb.AddForce(Vector3.down * jumpForce, ForceMode2D.Impulse);
            }
        }
        else if (i == 1)    //ü��
        {
            Vector3 jumpDirection;

            if(isLeftMoving)
                jumpDirection = new Vector3(-1, 1, 0).normalized;
            else
                jumpDirection = new Vector3(1, 1, 0).normalized;

            rb.velocity = Vector2.zero;
            //rb.velocity = new Vector2(rb.velocity.x, 0);  // ������ ���� �ӵ� �ʱ�ȭ
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        }
        else if(i == 2) // ���ݷ�
        {
            if(isJumping || isFalling)
            {
                chocolate = true;
            }
        }
        else if(i == 3) // �����
        {
            if(isJumping || isFalling)
            {
                rasberry = true;
                rb.velocity = Vector2.zero;
                StartCoroutine(Rasberry_Co());
                
            }
        }
        else if(i == 4) // ����
        {
            
            if (!isFalling)
            {
                matcha = true;
            }
            
        }
        
    }
    Vector2 flyDirection;
    private float flyingDistance = 0f; // ���ư� �Ÿ�

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
    private bool isRotated = false; // ȸ��
    private bool chocolateSound = false;
    private void Chocolate()
    {
        float colliderHalfWidth = GetComponent<Collider2D>().bounds.extents.x;
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, clingRay, LayerMask.GetMask("Platform"));
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, clingRay, LayerMask.GetMask("Platform"));
        RaycastHit2D down = Physics2D.Raycast(transform.position, Vector2.down, clingRay, LayerMask.GetMask("Platform"));
        

        isCling = false;
        if (down.collider == null)
        {
            if (left.collider != null)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                transform.position = new Vector2(left.point.x - colliderHalfWidth - rayOffset, transform.position.y);
                isCling = true;
                if (!isRotated)
                {
                    transform.Rotate(0, 0, -90);
                    isRotated = true;
                }
                if (!chocolateSound)
                {
                    SfxManager.i.PlaySound("Chocolate");
                    chocolateSound = true;
                }

            }
            else if (right.collider != null)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                transform.position = new Vector2(right.point.x + colliderHalfWidth + rayOffset, transform.position.y);
                isCling = true;
                if (!isRotated)
                {
                    transform.Rotate(0, 0, 90);
                    isRotated = true;
                }
                if (!chocolateSound)
                {
                    SfxManager.i.PlaySound("Chocolate");
                    chocolateSound = true;
                }
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
