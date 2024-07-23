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
    public float angle;    //ȸ���� ��
    float bouncePower;
    
    [SerializeField] public float floorMaxRay;  //�ٴ� ������ RayCast
    [SerializeField] public float rightMaxRay;   //������ �� ������ RayCast
    [SerializeField] public float leftMaxRay;   //���� �� ������ RayCast
    public float rotationSpeed = 10f;
    private bool isGravityReserved = false;
    bool isLeftMoving = false;      //�������� �̵��ϰ� �ִ��� Ȯ��
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
    void Move()    //������ ����
    {
        //����Ű ���ÿ� �Է¹��� �� ���ʷ� ������ Ű���� ����
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
        else if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))    //�� ����Ű�� �� ������ ������ ��
        {
            if(isLeftMoving)        //�������� �̵����� ���¿��� ��
                rb.AddForce(Vector2.left, ForceMode2D.Impulse);
            else                    //���������� �̵����� ���¿��� ��
                rb.AddForce(Vector2.right, ForceMode2D.Impulse);
        }

        //�ִ� �ӵ� ���� �ʵ��� ����
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
    void RayCastControl()  //���� ĳ��Ʈ ����
    {
        //�Ʒ��� ���� ���
        //���̾� ����ũ�� Platform�� ���̾�� ���� ���
        //transform.TransformDirection(Vector2.down) <-- ������Ʈ ȸ���� �°� ���̵� ȸ��
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), floorMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * floorMaxRay, Color.red, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
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

        //���������� ���� ���
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * rightMaxRay, Color.green, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
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

        //�������� ���� ���
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.left) * leftMaxRay, Color.blue, 0.3f);    //���� �׸���
        //���̿� ���� �ٴ��� �±װ� Platform�� ���� ���� ����
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
        //���� ���� ȸ���ϱ�
        //Ray���� 20����
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 20, LayerMask.GetMask("Platform"));
        angle = Vector2.Angle(hit3.normal, Vector2.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), 0.5f);
    }*/
    void resetRotation() //ȸ�� ���� �ʱ�ȭ
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Cannon.i.transform.rotation = Quaternion.identity;
        Cannon.i.isFire = false;
    }
    void Bounce(bool isLeft)   //���� ������ ƨ���
    {
        if(isLeft)      //���� ���� ���� ��Ҵٸ�
            rb.AddForce(Vector3.right * bouncePower, ForceMode2D.Impulse);  //���������� ƨ���
        else            //���� ������ ���� ��Ҵٸ�
            rb.AddForce(Vector3.left * bouncePower, ForceMode2D.Impulse);  //�������� ƨ���
    }
    public void SetGravityReversed(bool reserved) //�߷� ����
    {
        isGravityReserved = reserved;
    }
    IEnumerator CheckJumping()      //�����ϰ� 2���Ŀ��� falling���·� ����
    {
        isJumping = true;
        yield return new WaitForSeconds(2f);
        isJumping = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //�÷����� ��������� ���λ��°� �ƴϹǷ� ������ �� ����
        {
            isJumping = false;
            rb.gravityScale = 1f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        //�÷����� ������� ������ ���λ��� �̹Ƿ� ������ �� ����
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

} 
