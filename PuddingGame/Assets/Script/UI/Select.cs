using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{

    void Start()
    {
        DataManager.instance.LoadGameData();
    }

    private void OnApplicationQuit()
    {
        DataManager.instance.SaveGameData();
    }

    public void ChapterUnlock(int chapterNum)
    {

        DataManager.instance.SaveGameData();
    }
}
