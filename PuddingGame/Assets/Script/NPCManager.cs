using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public DialogueUI dialogueUI; // ��ȭ UI�� ����
    public int dialogueNo; // ��ȭ ��ȣ ����
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive) // �÷��̾�� �浹���� ��
        {
            Debug.Log("NPC");
            dialogueUI.StartDialogue(dialogueNo);
        }
    }
    public void EndDialogue()
    {
        isActive = false;
    }

}
