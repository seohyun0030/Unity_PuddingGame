using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static DialogueSystem;
using Spine;
using Spine.Unity;
using UnityEngine.SceneManagement;


public class DialogueUI : MonoBehaviour
{
    public static DialogueUI i;
    public SkeletonAnimation playerSkeletonAnimation;
    public TMP_Text nameText; // �̸� 
    public TMP_Text dialogueText; // ��ȭ ������ ǥ���� Text ������Ʈ
    public Image playerImage;
    public Image npcImage;
    public Image dialogueBack; // ���
    public Image pcnameBack; // �÷��̾� �̸� ���
    public Image npcnameBack; // npc �̸� ���
    public float nameX; // �̸�UI ��ġ
    public float nameY;
    public Attachment cream;
    public Attachment cherry;
    public Attachment cherry2;
    private DialogueSystem dialogueSystem;
    private NPCManager npc;
    private int currentNo;
    public Image background; //���� ȭ��
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            var skeleton = playerSkeletonAnimation.Skeleton;

            var slot1 = skeleton.FindSlot("cream");
            var slot2 = skeleton.FindSlot("cherry");
            var slot3 = skeleton.FindSlot("cherry2");
            slot1.Attachment = null;
            slot2.Attachment = null;
            slot3.Attachment = null;
            

        }
    }
    private void Start()
    {
        i = this;
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        npc = FindObjectOfType<NPCManager>();
        string documentID = "1jwP7whZvA5w7gJFezyhpXNXFDhdBbbDSXM1Wb_BHO58";
        string gid = "0";


        GoogleSheetRuntime.GetSheetData(documentID, gid, this, (success, data) =>
        {
            if (success && data != null)
            {
                dialogueSystem.LoadDialogues(data);
                //DisplayNextDialogue();
                if (SceneManager.GetActiveScene().name == "Stage1" && StartGame.loadDialogue)
                {

                    StartDialogue(0);
                }
            }
            else
            {
                Debug.LogError("������ �ε� ����");
            }
        });
        if (SceneManager.GetActiveScene().name == "Stage2")
        {
            var skeleton = playerSkeletonAnimation.Skeleton;

            var slot1 = skeleton.FindSlot("cream");
            var slot2 = skeleton.FindSlot("cherry");
            var slot3 = skeleton.FindSlot("cherry2");
            cream = slot1.Attachment;
            cherry = slot2.Attachment;
            cherry2 = slot3.Attachment;
            slot2.Attachment = null;
            slot1.Attachment = null;
            slot3.Attachment = null;

        }
        //gameObject.SetActive(false);
    }
    private void Update()
    {
        if (dialogueText.gameObject.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) // ���콺 Ŭ�� �� ��ȭ �ѱ��
        {
            DisplayNextDialogue();
        }
    }
    public void StartDialogue(int no)
    {
        nameText.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);
        dialogueBack.gameObject.SetActive(true);
        currentNo = no;
        DisplayNextDialogue();
        Time.timeScale = 0.00001f;
    }
    private void DisplayNextDialogue()
    {
        DialogueEntry nextDialogue = dialogueSystem.GetNextDialogue(currentNo);

        if (dialogueText.IsActive() && nextDialogue == null) // ��ȭ ��
        {
            GameObjectActivate(false);

            Time.timeScale = 1f;
            /*if (SceneManager.GetActiveScene().name == "Stage1")
            {
                StartCoroutine(CameraController.i.Stage1Animation());
            }*/
            if (SceneManager.GetActiveScene().name == "Stage2")
            {
                npc.EndDialogue();
                var skeleton = playerSkeletonAnimation.Skeleton;

                var slot1 = skeleton.FindSlot("cream");
                var slot2 = skeleton.FindSlot("cherry");
                if (slot1 != null && slot2 != null)
                {
                    slot2.Attachment = cherry;
                    slot1.Attachment = cream;
                }

            }
            else if (SceneManager.GetActiveScene().name == "Stage3")
            {
                npc.EndDialogue();
                ChangePlayerSkin("2");
            }
            else if (SceneManager.GetActiveScene().name == "Stage4")
            {
                npc.EndDialogue();
            }
            else if (SceneManager.GetActiveScene().name == "Stage5")
            {
                npc.EndDialogue();
            }
        }
        else
        {
            //Debug.Log($"���� ��ȭ: {nextDialogue.Name}: {nextDialogue.Line}");
            nameText.text = nextDialogue.Name;
            dialogueText.text = nextDialogue.Line; // ��ȭ ������ ǥ��
            HandleImage(nextDialogue);
        }
    }
    private void GameObjectActivate(bool no)
    {
        dialogueText.gameObject.SetActive(no);
        nameText.gameObject.SetActive(no);
        playerImage.gameObject.SetActive(no);
        npcImage.gameObject.SetActive(no);
        dialogueBack.gameObject.SetActive(no);
        npcnameBack.gameObject.SetActive(no);
        pcnameBack.gameObject.SetActive(no);
    }
    private void HandleImage(DialogueEntry next)
    {
        background.gameObject.SetActive(true);
        int pc = next.PC;
        int npc = next.NPC;

        Color activeColor = Color.white;
        Color inactiveColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        

        //�÷��̾� �̹���
        if (nameText.text == "Ǫ��")
        {
            playerImage.sprite = LoadPCImage(pc);
            playerImage.gameObject.SetActive(true);
            pcnameBack.gameObject.SetActive(true);
            Debug.Log(playerImage.color.ToString());
            playerImage.color = activeColor;
            npcImage.color = inactiveColor;
            RectTransform nameTextRect = nameText.GetComponent<RectTransform>();
            nameTextRect.anchoredPosition = new Vector2(nameX, nameY); // ��ġ ����
        }
        else if (npcImage.IsActive())
        {
            playerImage.color = inactiveColor;
            pcnameBack.gameObject.SetActive(false);
        }
        else
        {
            playerImage.gameObject.SetActive(false);
            pcnameBack.gameObject.SetActive(false);
        }
        //NPC �̹���
        if (nameText.text != "Ǫ��")
        {
            if (npc != 0) npcImage.sprite = LoadNPCImage(npc);
            npcnameBack.gameObject.SetActive(true);
            npcImage.gameObject.SetActive(true);
            npcImage.color = activeColor;
            playerImage.color = inactiveColor;
            RectTransform nameTextRect = nameText.GetComponent<RectTransform>();
            nameTextRect.anchoredPosition = new Vector2(-nameX, nameY); // ��ġ ����
        }
        else if (playerImage.IsActive())
        {
            npcImage.color = inactiveColor;
            npcnameBack.gameObject.SetActive(false);
        }
        else
        {
            npcImage.gameObject.SetActive(false);
            npcnameBack.gameObject.SetActive(false);

        }

    }
    private Sprite LoadPCImage(int num)
    {
        string imageName = "";
        switch (num)
        {
            case 0:
                imageName = "PC/0";
                break;
            case 1:
                imageName = "PC/1";
                break;
            case 2:
                imageName = "PC/2";
                break;
            case 3:
                imageName = "PC/3";
                break;
            case 4:
                imageName = "PC/4";
                break;
            case 5:
                imageName = "PC/5";
                break;
            case 6:
                imageName = "PC/6";
                break;
            case 7:
                imageName = "PC/7";
                break;
            case 8:
                imageName = "PC/8";
                break;
            case 9:
                imageName = "PC/9";
                break;
            case 10:
                imageName = "PC/10";
                break;
            case 11:
                imageName = "PC/11";
                break;
            case 12:
                imageName = "PC/12";
                break;
            case 13:
                imageName = "PC/13";
                break;
            default:
                Debug.Log("PC x");
                return null;
        }
        Sprite loadedSprite = Resources.Load<Sprite>(imageName);
        if (loadedSprite == null)
        {
            Debug.Log("PC �̹���x");
        }
        return loadedSprite;
    }
    private Sprite LoadNPCImage(int num)
    {
        string imageName = "";
        switch (num)
        {
            case 1:
                imageName = "NPC/1";
                break;
            case 2:
                imageName = "NPC/2";
                break;
            case 3:
                imageName = "NPC/3";
                break;
            case 4:
                imageName = "NPC/4";
                break;
            case 5:
                imageName = "NPC/5";
                break;
            default:
                Debug.Log("NPC x");
                return null;
        }
        Sprite loadedSprite = Resources.Load<Sprite>(imageName);
        if (loadedSprite == null)
        {
            Debug.Log("NPC �̹���x");
        }
        return loadedSprite;
    }
    private void ChangePlayerSkin(string skinName)
    {
        if (playerSkeletonAnimation != null)
        {
            var skeleton = playerSkeletonAnimation.Skeleton;
            var newSkin = skeleton.Data.FindSkin(skinName);

            if (newSkin != null)
            {
                skeleton.SetSkin(newSkin);
                skeleton.SetSlotsToSetupPose();
                playerSkeletonAnimation.AnimationState.Apply(skeleton);
            }
            else
            {
                Debug.LogError($"��Ų '{skinName}'�� ã�� �� ����.");
            }
        }
        else
        {
            Debug.LogError("Skeleton ���� x");
        }
    }
}
