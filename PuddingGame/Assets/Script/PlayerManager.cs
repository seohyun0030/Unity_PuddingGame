using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        i = this;
    }
    private void Start()
    {
        ChangeFriction_Bounce(Friction, BouncePower);
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
    }
    public void GoToSavePoint()
    {
        transform.position = SavePos;
        rigidbody.velocity = new Vector2(0, 0);
        //���� ���󺹱� ���� �ؾ���
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SavePoint"))      //������ ����Ʈ�� ������
        {
            SavePos = collision.transform.position;     //��ġ ���� ����
            PlayerPrefs.SetFloat("SavePosX", SavePos.x);
            PlayerPrefs.SetFloat("SavePosY", SavePos.y);

            collision.GetComponent<BoxCollider2D>().enabled = false;    //������ ����Ʈ �ݶ��̴� ����
        }
        else if (collision.CompareTag("EndPoint"))  //���������� ������
        {
            SceneManager.LoadScene(SceneManager.sceneCount+1);      //���� ������ �̵�
            PlayerPrefs.DeleteAll();
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
}
