using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueUI dialogueUI; // 대화 UI를 참조
    public int dialogueNo; // 대화 번호 설정
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive) // 플레이어와 충돌했을 때
        {
            dialogueUI.StartDialogue(dialogueNo);
        }
    }
    public void EndDialog()
    {
        isActive = false;
    }
}
