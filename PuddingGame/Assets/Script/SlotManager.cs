using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public static SlotManager i;

    [SerializeField] Transform Slot;   //토핑을 저장할 슬롯
    public int ToppingCnt = 1;      //저장 가능한 토핑 개수
    public GameObject[] Toppings;
    int n;      //토핑 순서 나타냄

    private Queue<GameObject> slots = new Queue<GameObject>();
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
            Destroy(slots.Dequeue());       //가장 처음 들어간 토핑을 제거한다
        }

        slots.Enqueue(Toppings[n]);
        Instantiate(Toppings[n], Slot);
        Toppings[n].SetActive(true);
    }
    public void UseTopping()
    {
        Debug.Log(slots.Dequeue());
    }
}
