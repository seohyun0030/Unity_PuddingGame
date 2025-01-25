using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    public DialogueUI dialogueUI; // 대화 UI를 참조
    public int dialogueNo; // 대화 번호 설정
    private bool isActive = false;
    public GameObject npc;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive) // 플레이어와 충돌했을 때
        {
            Debug.Log("NPC");
            dialogueUI.StartDialogue(dialogueNo);
        }
    }
    private void Update()
    {
        if (DialogueUI.i.mintChoco) // 민트초코 등장
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
