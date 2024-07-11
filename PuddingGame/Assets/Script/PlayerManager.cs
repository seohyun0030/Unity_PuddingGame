using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [Header("스테이터스 목록")]
    [SerializeField, Range(0,1)] public float Friction;     //마찰력
    [SerializeField] public float BouncePower;  //탄력
    [SerializeField] public float JumpPower;    //점프력
    [SerializeField] public float JumpGauge;    //점프 게이지
    [SerializeField] public float MaxSpeed;     //최고 속도

    [HideInInspector] public bool CanJump;       //점프를 할 수 있는 상태인지
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D FrictionControl;
    public BoxCollider2D boxCollider2D;
    float currentFriction;  //현재 마찰력

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        FrictionControl.friction = Friction;    //PhysicsMaterial2D의 마찰력 부분에 변수 할당
        currentFriction = Friction;             //현재 마찰력 저장
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
    void Slide()    //미끄러지기
    {
        if (Friction != currentFriction)    //마찰력이 바뀌었다면
        {
            FrictionControl.friction = Friction;    //마찰력 값 할당하기
            currentFriction = Friction;             //현재 마찰력 값 저장

            //콜라이더 null로 바꾸기 - 마찰력 값이 바뀌어도 인게임에서 마찰력이 바뀌지 않는 것처럼 보이는 오류 수정하기 위해
            boxCollider2D.sharedMaterial = null;
            boxCollider2D.sharedMaterial = FrictionControl;     //콜라이더 다시 할당해주기
        }
    }
}
