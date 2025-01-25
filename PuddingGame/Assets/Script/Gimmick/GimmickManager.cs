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
        // 씬 내 모든 리셋 가능한 오브젝트를 가져와 리스트에 추가
        IResettable[] resettables = FindObjectsOfType<MonoBehaviour>().OfType<IResettable>().ToArray();
        resettableObjects.AddRange(resettables);
    }

    private void Update()
    {
        // 플레이어가 비활성화되거나 R 키를 눌렀을 때 모든 오브젝트 리스폰
        if (!player.activeSelf || Input.GetKeyDown(KeyCode.R))
        {
            RespawnAllObjects();
        }
    }

    private void RespawnAllObjects()
    {
        foreach (var resettable in resettableObjects)
        {
            StartCoroutine(resettable.coRespawn());
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

