using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public static SlotManager i;

    [SerializeField] Transform Slot;   //������ ������ ����
    public int ToppingCnt = 1;      //���� ������ ���� ����
    public GameObject[] Toppings;
    int n;      //���� ���� ��Ÿ��

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
    public void AddTopping(string name)        //������ �Ծ��� �� ���� ������ ������ �����ϱ�
    {
        if (name == "Lemon") n = 0;
        else if (name == "Cherry") n = 1;
        else if (name == "Chocolate") n = 2;
        else if (name == "Rasberry") n = 3;
        else if (name == "Matcha") n = 4;

        if (slots.Count >= ToppingCnt)      //���� ������ �� �ִ� �������� ������ ������ ������ �������ٸ�
        {
            Destroy(slots.Dequeue());       //���� ó�� �� ������ �����Ѵ�
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
