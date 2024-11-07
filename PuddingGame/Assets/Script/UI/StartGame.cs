using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StartGame : MonoBehaviour
{
    public static bool loadDialogue = false;
    public void StartButton()
    {
        PlayerPrefs.SetFloat("SavePosX", 0);
        PlayerPrefs.SetFloat("SavePosY", 0);
        SceneManager.LoadScene("Stage1");
        loadDialogue = true;
    }

    public void Continuing()
    {
        loadDialogue = false;
        if (File.Exists(DataManager.instance.path))
        {
            DataManager.instance.LoadData();

            SceneManager.LoadScene(DataManager.instance.playerData.saveStage);
            //PlayerManager.i.gameObject.transform.position = DataManager.instance.playerData.playerPos;
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); // ���ø����̼� ����
        #endif
    }
}
