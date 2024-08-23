using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [Header("�������ͽ� ���")]
    [SerializeField, Range(0,1)] public float Friction;     //������
    [SerializeField, Range(0,0.9f)] public float BouncePower;  //ź��
    [SerializeField] public float JumpPower;    //������
    [SerializeField] public float JumpGauge;    //���� ������

    [HideInInspector] public bool CanJump;       //������ �� �� �ִ� ��������
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D Physics;
    public BoxCollider2D boxCollider2D;
    float currentFriction;  //���� ������
    float currentBouncePower;   //���� ź��
    public Rigidbody2D rigidbody;
    public float speed; // �÷��̾� �ӵ�
    public float fallingSpeed;  //���� �ӵ�

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        i = this;
    }
    private void Start()
    {
        Physics.friction = Friction;    //PhysicsMaterial2D�� ������ �κп� ���� �Ҵ�
        Physics.bounciness = BouncePower;
        /*currentFriction = Friction;             //���� ������ ����
        currentBouncePower = BouncePower;       //���� ź�� ����*/
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(SlotManager.i.GetTopping()=="LemonImage" || SlotManager.i.GetTopping() == "CherryImage") //���� ������ �����̰ų� ü���̸�
            {
                Debug.Log("jumping= " + PlayerMoveControl.i.isJumping);
                Debug.Log("falling= " + PlayerMoveControl.i.isFalling);
                if (PlayerMoveControl.i.isJumping || PlayerMoveControl.i.isFalling)      //jumping�̳� falling �����̸�
                    SlotManager.i.UseTopping();
            }
            else
            {
                SlotManager.i.UseTopping();
            }
        }

        speed = rigidbody.velocity.magnitude; // �÷��̾� �ӵ�
    }
    public void GoToSavePoint()
    {
        transform.position = SavePointManager.i.savePoint;
        rigidbody.velocity = new Vector2(0, 0);
        //���� ���󺹱� ���� �ؾ���
    }

    /*void Slide_Bounce()    //������, ź�� ���� �� --> ���߿� ����
    {
        if (Friction != currentFriction)    //�������� �ٲ���ٸ�
        {
            FrictionControl.friction = Friction;    //������ �� �Ҵ��ϱ�
            currentFriction = Friction;             //���� ������ �� ����

            //�ݶ��̴� null�� �ٲٱ� -> ������ ���� �ٲ� �ΰ��ӿ��� �������� �ٲ��� �ʴ� ��ó�� ���̴� ���� �����ϱ� ����
            boxCollider2D.sharedMaterial = null;
            boxCollider2D.sharedMaterial = FrictionControl;     //�ݶ��̴� �ٽ� �Ҵ����ֱ�
        }
        if(BouncePower != currentBouncePower)
        {
            FrictionControl.bounciness = BouncePower;   //ź�� �� �Ҵ��ϱ�
            currentBouncePower = BouncePower;           //���� ź�� �� ����

            boxCollider2D.sharedMaterial = null;
            boxCollider2D.sharedMaterial = FrictionControl;     //�ݶ��̴� �ٽ� �Ҵ����ֱ�
        }
    }*/
    /*void ChangeBoncePower(float newBonce)
    {
        FrictionControl.bounciness = newBonce;    //������ �� �Ҵ��ϱ�
        currentBouncePower = newBonce;             //���� ������ �� ����

        //�ݶ��̴� null�� �ٲٱ� -> ������ ���� �ٲ� �ΰ��ӿ��� �������� �ٲ��� �ʴ� ��ó�� ���̴� ���� �����ϱ� ����
        boxCollider2D.sharedMaterial = null;
        boxCollider2D.sharedMaterial = FrictionControl;     //�ݶ��̴� �ٽ� �Ҵ����ֱ�
    }*/
}
