using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{

}

public class DataManager : MonoBehaviour
{
    // ---싱글톤으로 선언--- //
    public static DataManager instance;

    PlayerData nowPlayer = new PlayerData();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) 
        { 
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }


    // 불러오기
    public void LoadGameData()
    {

    }


    // 저장하기
    public void SaveGameData()
    {

    }
}
