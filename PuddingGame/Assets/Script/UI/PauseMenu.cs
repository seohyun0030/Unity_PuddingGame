using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI; // �Ͻ� ���� UI �г�
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
        Time.timeScale = 0f; // �ð��� �帧 ����. 0���. �� �ð��� ����.
        Debug.Log(Time.timeScale);
    }

    public void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        OptionUI.SetActive(false);
        Time.timeScale = 1f; // 1��� (���� �ӵ�)
    }

    public void ClickSave()
    {
        Debug.Log("���̺�");
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        //Application.Quit();  // ���� ���� (������ �� �����̱� ������ ���� ������ ��ȭ X)
        SceneManager.LoadScene("Title");
        GameManager.isPause = false;
    }
}
