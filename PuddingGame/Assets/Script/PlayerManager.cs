using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [SerializeField] public float MoveSpeed;    //�÷��̾� ������ �ӵ�
    [SerializeField] public float MaxSpeed;     //�ְ� �ӵ�
    [SerializeField] public float JumpGauge;    //���� ������
    [SerializeField] public bool CanJump;       //������ �� �� �ִ� ��������
    [SerializeField] public float JumpPower;    //������
    
    public float time = 0f;

    private void Awake()
    {
        i = this;
    }
    public void plusJumpGauge()
    {
        if (time < 2f)
        {
            time += Time.deltaTime;
            JumpGauge = Mathf.Lerp(0.2f, 1, time / 2f);
        }
    }
}