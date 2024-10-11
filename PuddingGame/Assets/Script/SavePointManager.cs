using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour
{
    public static SavePointManager i;
    public GameObject Player;

    private void Awake()
    {
        i = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!Player.activeSelf)      //플레이어가 비활성화 상태라면
                Player.SetActive(true);

            PlayerManager.i.GoToSavePoint();

            SlotManager.i.DeleteTopping();
        }
    }
}
