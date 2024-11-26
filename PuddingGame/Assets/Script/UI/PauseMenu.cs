using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI; // 일시 정지 UI 패널
    [SerializeField] private GameObject OptionUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isPause)
                CallMenu();
            else
                CloseMenu();
        }

    }

    public void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f; // 시간의 흐름 설정. 0배속. 즉 시간을 멈춤.
        Debug.Log(Time.timeScale);
    }

    public void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        OptionUI.SetActive(false);
        Time.timeScale = 1f; // 1배속 (정상 속도)
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        //Application.Quit();  // 게임 종료 (에디터 상 실행이기 때문에 종료 눌러도 변화 X)
        SceneManager.LoadScene("Title");
        GameManager.isPause = false;
    }
}
