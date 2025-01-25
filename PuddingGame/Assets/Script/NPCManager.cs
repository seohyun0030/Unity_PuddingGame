using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    public DialogueUI dialogueUI; // ��ȭ UI�� ����
    public int dialogueNo; // ��ȭ ��ȣ ����
    private bool isActive = false;
    public GameObject npc;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive) // �÷��̾�� �浹���� ��
        {
            Debug.Log("NPC");
            dialogueUI.StartDialogue(dialogueNo);
        }
    }
    private void Update()
    {
        if (DialogueUI.i.mintChoco) // ��Ʈ���� ����
        {
            SpriteRenderer sr = npc.GetComponent<SpriteRenderer>();
            Sprite loadedSprite = Resources.Load<Sprite>("NPC/4");
            sr.sprite = loadedSprite;
            DialogueUI.i.mintChoco = false;
        }
    }
    public void EndDialogue()
    {
        isActive = false;
    }

}
