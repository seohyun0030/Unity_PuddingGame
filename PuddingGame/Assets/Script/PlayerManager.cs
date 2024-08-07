using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [Header("스테이터스 목록")]
    [SerializeField, Range(0,1)] public float Friction;     //마찰력
    [SerializeField, Range(0,0.9f)] public float BouncePower;  //탄력
    [SerializeField] public float JumpPower;    //점프력
    [SerializeField] public float JumpGauge;    //점프 게이지
    [SerializeField] public float MaxSpeed;     //최고 속도

    [SerializeField] public float firstJumpGauge;   //점프게이지를 인스펙터창에서 조절하기 위해

    [SerializeField] public float JumpChargeTime;   //점프 차지 시간
    [HideInInspector] public bool CanJump;       //점프를 할 수 있는 상태인지
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D FrictionControl;
    public BoxCollider2D boxCollider2D;
    float currentFriction;  //현재 마찰력
    float currentBouncePower;   //현재 탄력
    public Rigidbody2D rigidbody;
    public float speed; // 플레이어 속도

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        i = this;
    }
    private void Start()
    {
        FrictionControl.friction = Friction;    //PhysicsMaterial2D의 마찰력 부분에 변수 할당
        FrictionControl.bounciness = BouncePower;
        currentFriction = Friction;             //현재 마찰력 저장
        currentBouncePower = BouncePower;       //현재 탄력 저장
    }
    private void Update()
    {
        Slide_Bounce();
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(SlotManager.i.GetTopping()=="LemonImage" || SlotManager.i.GetTopping() == "CherryImage") //만약 토핑이 레몬이거나 체리이면
            {
                Debug.Log("jumping= " + PlayerMoveControl.i.isJumping);
                Debug.Log("falling= " + PlayerMoveControl.i.isFalling);
                if (PlayerMoveControl.i.isJumping || PlayerMoveControl.i.isFalling)      //jumping이나 falling 상태이면
                    SlotManager.i.UseTopping();
            }
            else
            {
                SlotManager.i.UseTopping();
            }
        }

        /*if(Input.GetKeyDown(KeyCode.R))
        {
            GoToSavePoint();
        }*/

        speed = rigidbody.velocity.magnitude; // 플레이어 속도
    }
    public void GoToSavePoint()
    {
        transform.position = SavePointManager.i.savePoint;
        rigidbody.velocity = new Vector2(0, 0);
        //상태 원상복귀 구현 해야함
    }
    public void plusJumpGauge()
    {
        if (time < JumpChargeTime)
        {
            time += Time.deltaTime;
            //JumpGauge = Mathf.Lerp(0.2f, 1, time / JumpChargeTime);
            JumpGauge = Mathf.Lerp(firstJumpGauge, 1, time / JumpChargeTime);
        }
    }
    void Slide_Bounce()    //마찰력, 탄력 시험 용 --> 나중에 삭제
    {
        if (Friction != currentFriction)    //마찰력이 바뀌었다면
        {
            FrictionControl.friction = Friction;    //마찰력 값 할당하기
            currentFriction = Friction;             //현재 마찰력 값 저장

            //콜라이더 null로 바꾸기 -> 마찰력 값이 바뀌어도 인게임에서 마찰력이 바뀌지 않는 것처럼 보이는 오류 수정하기 위해
            boxCollider2D.sharedMaterial = null;
            boxCollider2D.sharedMaterial = FrictionControl;     //콜라이더 다시 할당해주기
        }
        if(BouncePower != currentBouncePower)
        {
            FrictionControl.bounciness = BouncePower;   //탄력 값 할당하기
            currentBouncePower = BouncePower;           //현재 탄력 값 저장

            boxCollider2D.sharedMaterial = null;
            boxCollider2D.sharedMaterial = FrictionControl;     //콜라이더 다시 할당해주기
        }
    }
    void ChangeBoncePower(float newBonce)
    {
        FrictionControl.bounciness = newBonce;    //마찰력 값 할당하기
        currentBouncePower = newBonce;             //현재 마찰력 값 저장

        //콜라이더 null로 바꾸기 -> 마찰력 값이 바뀌어도 인게임에서 마찰력이 바뀌지 않는 것처럼 보이는 오류 수정하기 위해
        boxCollider2D.sharedMaterial = null;
        boxCollider2D.sharedMaterial = FrictionControl;     //콜라이더 다시 할당해주기
    }
}
