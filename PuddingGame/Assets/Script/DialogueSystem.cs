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
                    int playerImageCode = 0; // �⺻��
                    int npcImageCode = 0;    // �⺻��

                    // �÷��̾ ���ڷ� ��ȯ �������� üũ
                    if (parts.Length >= 5 && !string.IsNullOrEmpty(parts[4]))
                    {
                        if (!int.TryParse(parts[4], out playerImageCode))
                        {
                            Debug.LogWarning($"�÷��̾� �̹����� ��ȿ���� �ʽ��ϴ�: {parts[4]}");
                        }
                    }

                    // NPC�� ���ڷ� ��ȯ �������� üũ
                    if (parts.Length >= 6 && !string.IsNullOrEmpty(parts[5]))
                    {
                        if (!int.TryParse(parts[5], out npcImageCode))
                        {
                            Debug.LogWarning($"NPC �̹����� ��ȿ���� �ʽ��ϴ�: {parts[5]}");
                        }
                    }
                    dialogues.Add(new DialogueEntry
                    {
                        No = no,
                        Index = index,
                        Name = parts[2], // �̸�
                        Line = parts[3], // ��ȭ ����
                        PC = playerImageCode, //�÷��̾� �̹���
                        NPC = npcImageCode //NPC �̹���
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
            currentDialogueIndex = 0; // ���ο� ��ȭ�� ���۵� �� �ε��� �ʱ�ȭ
        }

        while (currentDialogueIndex < dialogues.Count)
        {
            var dialogue = dialogues[currentDialogueIndex];
            if (dialogue.No == no)
            {
                currentDialogueIndex++;
                return dialogue; // DialogueEntry ��ȯ
            }
            currentDialogueIndex++;
            
        }

        return null;
    }
}

