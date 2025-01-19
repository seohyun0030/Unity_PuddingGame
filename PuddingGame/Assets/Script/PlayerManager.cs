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
    [Header("�������ͽ� ���")]
    [SerializeField, Range(0,1)] public float Friction;     //������
    [SerializeField, Range(0,0.9f)] public float BouncePower;  //ź��
    [SerializeField] public float JumpPower;    //������
    [SerializeField] public float JumpGauge;    //���� ������

    [HideInInspector] public bool CanJump;       //������ �� �� �ִ� ��������
    [HideInInspector] public float time = 0f;
    public PhysicsMaterial2D Physics;
    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rigidbody;
    public float speed; // �÷��̾� �ӵ�
    public float fallingSpeed;  //���� �ӵ�
    public Vector3 SavePos;
    public int StageIndex;
    SkeletonAnimation anim;
    float originalBounce;

    private SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        i = this;
    }
    private void Start()
    {
        ChangeFriction_Bounce(Friction, BouncePower);
        anim = GetComponent<SkeletonAnimation>();
        StageIndex = SceneManager.GetActiveScene().buildIndex;

        originalBounce = Physics.bounciness;

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null)
        {
            // ���� ��Ų�� �̸��� Ȯ��
            string currentSkinName = skeletonAnimation.Skeleton.Skin.Name;

            if (currentSkinName == "2")     //�������� �� ��Ʈ ��Ų�̸� ��ƼŬ ��Ʈ��
            {
                PlayerMoveControl.i.ChangeParticleColor("mint");
            }
        }
    }
    private void Update()
    {
        if (!DialogueUI.i.dialogueText.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (SlotManager.i.GetTopping() == "LemonImage" || SlotManager.i.GetTopping() == "CherryImage") //���� ������ �����̰ų� ü���̸�
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

        if (SceneManager.GetActiveScene().buildIndex == 1 && transform.position.x > 90)     //��������1���� īƮ�� ���� ����
        {
            fallingSpeed = 100;
        }
            //DataManager.instance.playerData.playerPos = PlayerManager.i.SavePos;
            DataManager.instance.playerData.saveStage = PlayerManager.i.StageIndex;
            DataManager.instance.SaveData();

    }
    public void GoToSavePoint()
    {
        transform.position = SavePos;
        rigidbody.velocity = new Vector2(0, 0);
        //���� ���󺹱�
        transform.rotation = Quaternion.identity;
        Physics.bounciness = originalBounce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SavePoint"))      //������ ����Ʈ�� ������
        {
            SavePos = collision.transform.position;     //��ġ ���� ����
            PlayerPrefs.SetFloat("SavePosX", SavePos.x);
            PlayerPrefs.SetFloat("SavePosY", SavePos.y);

            collision.GetComponent<BoxCollider2D>().enabled = false;    //������ ����Ʈ �ݶ��̴� ����

            SfxManager.i.PlaySound("SavePoint");        //���̺� ����Ʈ ȿ���� ���
        }
        else if (collision.CompareTag("EndPoint"))  //���������� ������
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //���� ������ �̵�

            PlayerPrefs.DeleteKey("SavePosX");
            PlayerPrefs.DeleteKey("SavePosY");
        }
        else if (collision.CompareTag("Topping"))
        {
            if(SlotManager.i != null)
                SlotManager.i.RespawnTopping(collision);

            SfxManager.i.PlaySound("GetItem");      //������ ȹ�� ȿ���� ���
        }
    }
    public void ChangeFriction_Bounce(float newFriction, float newBoune)
    {
        Physics.friction = newFriction;    //������ �� �Ҵ��ϱ�
        Physics.bounciness = newBoune;     //ź���� �� �Ҵ��ϱ�

        //�ݶ��̴� null�� �ٲٱ� -> ������ ���� �ٲ� �ΰ��ӿ��� �������� �ٲ��� �ʴ� ��ó�� ���̴� ���� �����ϱ� ����
        boxCollider2D.sharedMaterial = null;
        boxCollider2D.sharedMaterial = Physics;     //�ݶ��̴� �ٽ� �Ҵ����ֱ�
    }
    public void jumpAnim()
    {
        anim.AnimationState.SetAnimation(0, "Jump0-Enter", false);
    }
    public void idleAnim()
    {
        if (anim.state.GetCurrent(0).Animation.Name == "#1idel")
            return;
        anim.AnimationState.SetAnimation(0, "#1idel", true);
    }
    public void char1Anim()
    {
        if (anim.state.GetCurrent(0).Animation.Name == "Char1")
            return;
        anim.AnimationState.SetAnimation(0, "Char1", true);
    }
    public void char2Anim()
    {
        if (anim.state.GetCurrent(0).Animation.Name == "Char2")
            return;
        anim.AnimationState.SetAnimation(0, "Char2", true);
    }
    public void char3Anim()
    {
        if (anim.state.GetCurrent(0).Animation.Name == "Char3")
            return;
        anim.AnimationState.SetAnimation(0, "Char3", true);
    }
}
