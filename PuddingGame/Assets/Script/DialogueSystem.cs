using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private List<DialogueEntry> dialogues = new List<DialogueEntry>();
    private int currentDialogueIndex = 0;
    private int currentNo = 0;

    [System.Serializable]
    public class DialogueEntry
    {
        public int No;
        public int Index;
        public string Line;
    }

    public void LoadDialogues(string data)
    {
        string[] lines = data.Split('\n');
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                var parts = line.Split('\t');
                if (parts.Length >= 3 && int.TryParse(parts[0], out int no) && int.TryParse(parts[1], out int index))
                {
                    dialogues.Add(new DialogueEntry
                    {
                        No = no,
                        Index = index,
                        Line = parts[2] // 대화 내용
                    });
                }
            }
        }
    }

    public string GetNextDialogue(int no)
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
                return dialogue.Line; // 대화 내용 반환
            }
            currentDialogueIndex++;
        }

        return null;
    }
}

