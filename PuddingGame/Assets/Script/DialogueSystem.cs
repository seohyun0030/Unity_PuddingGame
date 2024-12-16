using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private List<DialogueEntry> dialogues = new List<DialogueEntry>();
    private int currentDialogueIndex = -1;
    private int currentNo = -1;

    [System.Serializable]
    public class DialogueEntry
    {
        public int No;
        public int Index;
        public string Name;
        public string Line;
        public int PC;
        public int NPC;
    }

    public void LoadDialogues(string data)
    {
        string[] lines = data.Split('\n');
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                var parts = line.Split('\t');
                if (parts.Length >= 6 && int.TryParse(parts[0], out int no) && int.TryParse(parts[1], out int index))
                {
                    int playerImageCode = 0; // 기본값
                    int npcImageCode = 0;    // 기본값

                    // 플레이어가 숫자로 변환 가능한지 체크
                    if (parts.Length >= 5 && !string.IsNullOrEmpty(parts[4]))
                    {
                        if (!int.TryParse(parts[4], out playerImageCode))
                        {
                            Debug.LogWarning($"플레이어 이미지가 유효하지 않습니다: {parts[4]}");
                        }
                    }

                    // NPC가 숫자로 변환 가능한지 체크
                    if (parts.Length >= 6 && !string.IsNullOrEmpty(parts[5]))
                    {
                        if (!int.TryParse(parts[5], out npcImageCode))
                        {
                            Debug.LogWarning($"NPC 이미지가 유효하지 않습니다: {parts[5]}");
                        }
                    }
                    dialogues.Add(new DialogueEntry
                    {
                        No = no,
                        Index = index,
                        Name = parts[2], // 이름
                        Line = parts[3], // 대화 내용
                        PC = playerImageCode, //플레이어 이미지
                        NPC = npcImageCode //NPC 이미지
                    }); 
                }
            }
        }
    }

    public DialogueEntry GetNextDialogue(int no)
    {
        if (currentNo != no)
        {
            currentNo = no;
            currentDialogueIndex = 0; // 새로운 대화가 시작될 때 인덱스 초기화
        }

        while (currentDialogueIndex < dialogues.Count)
        {
            var dialogue = dialogues[currentDialogueIndex];
            if (dialogue.No == no)
            {
                currentDialogueIndex++;
                return dialogue; // DialogueEntry 반환
            }
            currentDialogueIndex++;
            
        }
        DialogueUI.i.background.gameObject.SetActive(false);
        return null;
    }
}

