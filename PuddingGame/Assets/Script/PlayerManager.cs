using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;
    [SerializeField] public float MoveSpeed;    //속도
    [SerializeField] public float BouncePower;  //탄력
    [SerializeField] public float JumpPower;    //점프력
    [SerializeField] public float JumpGauge;    //점프 게이지
    [SerializeField] public float MaxSpeed;     //최고 속도
    
    [SerializeField] public bool CanJump;       //점프를 할 수 있는 상태인지
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
