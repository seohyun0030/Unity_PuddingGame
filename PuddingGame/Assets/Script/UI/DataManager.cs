using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{

}

public class DataManager : MonoBehaviour
{
    // ---�̱������� ����--- //
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


    // �ҷ�����
    public void LoadGameData()
    {

    }


    // �����ϱ�
    public void SaveGameData()
    {

    }
}
