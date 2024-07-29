using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public static SlotManager i;

    [SerializeField] Transform Slot;   //토핑을 저장할 슬롯
    public int ToppingCnt = 1;      //저장 가능한 토핑 개수
    public GameObject[] Toppings;
    int n;      //토핑 순서 나타냄

    private List<GameObject> slots = new List<GameObject>();
    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        /*slots.Enqueue(Toppings);
        Instantiate(Toppings, Slot);
        Toppings.SetActive(true);*/
    }
    public void AddTopping(string name)        //토핑을 먹었을 때 저장 공간에 토핑을 저장하기
    {
        if (name == "Lemon") n = 0;
        else if (name == "Cherry") n = 1;
        else if (name == "Chocolate") n = 2;
        else if (name == "Rasberry") n = 3;
        else if (name == "Matcha") n = 4;

        if (slots.Count >= ToppingCnt)      //만약 저장할 수 있는 개수보다 슬롯의 아이템 개수가 많아진다면
        {
            GameObject UsedTopping = slots[0];
            slots.RemoveAt(0);          //리스트에서 0번째 요소 삭제
            Destroy(UsedTopping);       //Ui에서도 삭제
        }

        slots.Add(Toppings[n]);
        Instantiate(Toppings[n], Slot);
        Toppings[n].SetActive(true);
    }
    public void UseTopping()
    {
        if (slots[0].name == "LemonImage")
        {
            //레몬 아이템을 사용했을 때
            Debug.Log("Lemon");
            PlayerMoveControl.i.ToppingJump(0);
        }
        else if(slots[0].name == "CherryImage")
        {
            //체리 아이템을 사용했을 때
            Debug.Log("Cherry");
            PlayerMoveControl.i.ToppingJump(1);
        }
        else if (slots[0].name == "ChocolateImage")
        {
            //초콜릿 아이템을 사용했을 때
            Debug.Log("Chocolate");
            PlayerMoveControl.i.ToppingJump(2);
        }
        else if (slots[0].name == "RasberryImage")
        {
            //라즈베리 아이템을 사용했을 때
            Debug.Log("Rasberry");
            PlayerMoveControl.i.ToppingJump(3);
        }
        else if (slots[0].name == "MatchaImage")
        {
            //녹차 아이템을 사용했을 때
            Debug.Log("Matcha");
            PlayerMoveControl.i.ToppingJump(4);
        }

        GameObject UsedTopping = slots[0];
        slots.RemoveAt(0);          //리스트에서 0번째 요소 삭제
        Destroy(UsedTopping);       //Ui에서도 삭제
    }
    public string GetTopping()
    {
        return slots[0].name;
    }
}
