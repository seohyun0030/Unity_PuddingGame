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
    public float angle;    //ȸ���� ��
    float bouncePower;
    bool isSliding;        //�̲������� �ִ��� Ȯ��
    float friction;
    
    [SerializeField] public float floorMaxRay;  //�ٴ� ������ RayCast
    [SerializeField] public float rightMaxRay;   //������ �� ������ RayCast
    [SerializeField] public float leftMaxRay;   //���� �� ������ RayCast
    public float rotationSpeed = 10f;
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

        /*if (Input.GetButtonUp("Horizontal"))    //�¿� �̵��ϴٰ� ����Ű�� �� ���� �ӵ�
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
        }
        //Slide();
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
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right, rightMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.right * rightMaxRay, Color.green, 0.3f);    //���� �׸���
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
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left, leftMaxRay, LayerMask.GetMask("Platform"));
        Debug.DrawRay(transform.position, Vector2.left * leftMaxRay, Color.blue, 0.3f);    //���� �׸���
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
    void Rotate()
    {
        //���� ���� ȸ���ϱ�
        //Ray���� 20����
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
    void Bounce(bool isLeft)   //���� ������ ƨ���
    {
        if(isLeft)      //���� ���� ���� ��Ҵٸ�
            rb.AddForce(Vector3.right * bouncePower, ForceMode2D.Impulse);  //���������� ƨ���
        else            //���� ������ ���� ��Ҵٸ�
            rb.AddForce(Vector3.left * bouncePower, ForceMode2D.Impulse);  //�������� ƨ���
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
}
