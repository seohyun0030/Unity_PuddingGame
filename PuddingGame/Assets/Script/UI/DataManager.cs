using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[SerializeField]
public class PlayerData
{
    //public Vector3 playerPos;   // 세이브 포인트
    public int saveStage; // 저장된 스테이지
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance; // 싱글톤패턴

    public PlayerData playerData = new PlayerData(); // 플레이어 데이터 생성

    public string path; // 경로

    private void Awake()
    {
        #region 싱글톤
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
