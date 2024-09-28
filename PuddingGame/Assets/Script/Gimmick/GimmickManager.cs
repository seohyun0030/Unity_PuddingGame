using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GimmickManager : MonoBehaviour
{
    public GameObject player;
    private List<IResettable> resettableObjects = new List<IResettable>();

    private void Start()
    {
        // �� �� ��� ���� ������ ������Ʈ�� ������ ����Ʈ�� �߰�
        IResettable[] resettables = FindObjectsOfType<MonoBehaviour>().OfType<IResettable>().ToArray();
        resettableObjects.AddRange(resettables);
    }

    private void Update()
    {
        // �÷��̾ ��Ȱ��ȭ�ǰų� R Ű�� ������ �� ��� ������Ʈ ������
        if (!player.activeSelf || Input.GetKeyDown(KeyCode.R))
        {
            RespawnAllObjects();
        }
    }

    private void RespawnAllObjects()
    {
        foreach (var resettable in resettableObjects)
        {
            resettable.Respawn();
        }
    }
    public void RegisterResettable(IResettable resettable)
    {
        if (!resettableObjects.Contains(resettable))
        {
            resettableObjects.Add(resettable);
        }
    }

    public void UnregisterResettable(IResettable resettable)
    {
        if (resettableObjects.Contains(resettable))
        {
            resettableObjects.Remove(resettable);
        }
    }
}

