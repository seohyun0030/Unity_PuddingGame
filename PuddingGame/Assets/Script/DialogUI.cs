using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI i;
    public TMP_Text dialogueText; // 대화 내용을 표시할 Text 컴포넌트
    private DialogueSystem dialogueSystem;
    private NPCManager npc;
    private int currentNo;

    private void Start()
    {
        i = this;
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        npc = FindObjectOfType<NPCManager>();
        string documentID = "1jwP7whZvA5w7gJFezyhpXNXFDhdBbbDSXM1Wb_BHO58";
        string gid = "0";
        GoogleSheet.GetSheetData(documentID, gid, this, (success, data) =>
        {
            if (success && data != null)
            {
                dialogueSystem.LoadDialogues(data);
                DisplayNextDialogue();
            }
            else
            {
                Debug.LogError("데이터 로드 실패");
            }
        });
    }
    private void Update()
    {
        if (dialogueText.IsActive() && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) // 마우스 클릭 시 대화 넘기기
        {
            DisplayNextDialogue();
        }
    }
    public void StartDialogue(int no)
    {
        dialogueText.gameObject.SetActive(true);
        currentNo = no;
        DisplayNextDialogue();
    }
    private void DisplayNextDialogue()
    {
        string nextDialogue = dialogueSystem.GetNextDialogue(currentNo);

        if (nextDialogue == null)
        {
            dialogueText.gameObject.SetActive(false); // 대화가 끝날 때 호출
            npc.EndDialog();
        }
        else
        {
            dialogueText.text = nextDialogue; // 대화 내용을 표시
        }
    }
}
