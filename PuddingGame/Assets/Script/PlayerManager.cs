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

    [HideInInspector] public bool CanJump;       //������ �� �� �ִ� ��������
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D FrictionControl;
    public BoxCollider2D boxCollider2D;
    float currentFriction;  //���� ������

    private void Awake()
    {
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
    }
    public void plusJumpGauge()
    {
        if (time < 2f)
        {
            time += Time.deltaTime;
            JumpGauge = Mathf.Lerp(0.2f, 1, time / 2f);
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
