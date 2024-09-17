using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [Header("스테이터스 목록")]
    [SerializeField, Range(0,1)] public float Friction;     //마찰력
    [SerializeField, Range(0,0.9f)] public float BouncePower;  //탄력
    [SerializeField] public float JumpPower;    //점프력
    [SerializeField] public float JumpGauge;    //점프 게이지

    [HideInInspector] public bool CanJump;       //점프를 할 수 있는 상태인지
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D Physics;
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rigidbody;
    public float speed; // 플레이어 속도
    public float fallingSpeed;  //낙하 속도
    
    public Vector3 SavePos;

    SkeletonAnimation anim;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        i = this;
    }
    private void Start()
    {
        ChangeFriction_Bounce(Friction, BouncePower);
        anim = GetComponent<SkeletonAnimation>();

    }
    private void Update()
    {
        if (!DialogueUI.i.dialogueText.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (SlotManager.i.GetTopping() == "LemonImage" || SlotManager.i.GetTopping() == "CherryImage") //만약 토핑이 레몬이거나 체리이면
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

            speed = rigidbody.velocity.magnitude; // 플레이어 속도
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && transform.position.x > 90)     //스테이지1에서 카트로 낙사 방지
        {
            fallingSpeed = 100;
        }
    }
    public void GoToSavePoint()
    {
        transform.position = SavePos;
        rigidbody.velocity = new Vector2(0, 0);
        //상태 원상복귀 구현 해야함
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SavePoint"))      //세이프 포인트에 닿으면
        {
            SavePos = collision.transform.position;     //위치 정보 저장
            PlayerPrefs.SetFloat("SavePosX", SavePos.x);
            PlayerPrefs.SetFloat("SavePosY", SavePos.y);

            collision.GetComponent<BoxCollider2D>().enabled = false;    //세이프 포인트 콜라이더 삭제
        }
        else if (collision.CompareTag("EndPoint"))  //종료지점에 닿으면
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //다음 씬으로 이동
            PlayerPrefs.DeleteAll();
        }
        else if (collision.CompareTag("Topping"))
        {
            StartCoroutine(RespawnTopping(collision));
        }
    }
    IEnumerator RespawnTopping(Collider2D c)
    {
        yield return new WaitForSeconds(5f);
        c.gameObject.SetActive(true);
    }
    public void ChangeFriction_Bounce(float newFriction, float newBoune)
    {
        Physics.friction = newFriction;    //마찰력 값 할당하기
        Physics.bounciness = newBoune;     //탄성력 값 할당하기

        //콜라이더 null로 바꾸기 -> 마찰력 값이 바뀌어도 인게임에서 마찰력이 바뀌지 않는 것처럼 보이는 오류 수정하기 위해
        boxCollider2D.sharedMaterial = null;
        boxCollider2D.sharedMaterial = Physics;     //콜라이더 다시 할당해주기
    }
    public void Animation(string b)
    {
        if(b=="jump")
            anim.AnimationState.SetAnimation(0, "jump2", false);

        else if(b=="idle")
            anim.AnimationState.SetAnimation(0, "idel", true);
    }

}
