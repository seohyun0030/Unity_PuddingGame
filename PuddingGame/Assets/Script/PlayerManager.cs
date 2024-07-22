using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [Header("�������ͽ� ���")]
    [SerializeField, Range(0,1)] public float Friction;     //������
    [SerializeField] public float BouncePower;  //ź��
    [SerializeField] public float JumpPower;    //������
    [SerializeField] public float JumpGauge;    //���� ������
    [SerializeField] public float MaxSpeed;     //�ְ� �ӵ�

    [SerializeField] public float JumpChargeTime;   //���� ���� �ð�
    [HideInInspector] public bool CanJump;       //������ �� �� �ִ� ��������
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D FrictionControl;
    public BoxCollider2D boxCollider2D;
    float currentFriction;  //���� ������
    public Rigidbody2D rigidbody;
    public float speed; // �÷��̾� �ӵ�

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        i = this;
    }
    private void Start()
    {
        FrictionControl.friction = Friction;    //PhysicsMaterial2D�� ������ �κп� ���� �Ҵ�
        currentFriction = Friction;             //���� ������ ����
    }
    private void Update()
    {
        Slide();
        if (Input.GetKeyDown(KeyCode.X))
        {
            SlotManager.i.UseTopping();
        }

        speed = rigidbody.velocity.magnitude; // �÷��̾� �ӵ�
    }
    public void plusJumpGauge()
    {
        if (time < JumpChargeTime)
        {
            time += Time.deltaTime;
            JumpGauge = Mathf.Lerp(0.2f, 1, time / JumpChargeTime);
        }
    }
    void Slide()    //�̲�������
    {
        if (Friction != currentFriction)    //�������� �ٲ���ٸ�
        {
            FrictionControl.friction = Friction;    //������ �� �Ҵ��ϱ�
            currentFriction = Friction;             //���� ������ �� ����

            //�ݶ��̴� null�� �ٲٱ� - ������ ���� �ٲ� �ΰ��ӿ��� �������� �ٲ��� �ʴ� ��ó�� ���̴� ���� �����ϱ� ����
            boxCollider2D.sharedMaterial = null;
            boxCollider2D.sharedMaterial = FrictionControl;     //�ݶ��̴� �ٽ� �Ҵ����ֱ�
        }
    }
}
