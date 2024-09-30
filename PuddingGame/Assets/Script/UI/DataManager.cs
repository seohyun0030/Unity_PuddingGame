using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.SearchService;

[SerializeField]
public class PlayerData
{
    //public Vector3 playerPos;   // ���̺� ����Ʈ
    public int saveStage; // ����� ��������
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance; // �̱�������

    public PlayerData playerData = new PlayerData(); // �÷��̾� ������ ����

    public string path; // ���

    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Path.Combine(Application.dataPath, "database.json");
    }

    private void Update()
    {
    }

    public void SaveData()
    {
       string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, jsonData);
    }

    public void LoadData()
    {
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
}
