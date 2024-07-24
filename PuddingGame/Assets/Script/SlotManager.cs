using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public static SlotManager i;

    [SerializeField] Transform Slot;   //������ ������ ����
    public int ToppingCnt = 1;      //���� ������ ���� ����
    public GameObject[] Toppings;
    int n;      //���� ���� ��Ÿ��

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
    public void AddTopping(string name)        //������ �Ծ��� �� ���� ������ ������ �����ϱ�
    {
        if (name == "Lemon") n = 0;
        else if (name == "Cherry") n = 1;
        else if (name == "Chocolate") n = 2;
        else if (name == "Rasberry") n = 3;
        else if (name == "Matcha") n = 4;

        if (slots.Count >= ToppingCnt)      //���� ������ �� �ִ� �������� ������ ������ ������ �������ٸ�
        {
            GameObject UsedTopping = slots[0];
            slots.RemoveAt(0);          //����Ʈ���� 0��° ��� ����
            Destroy(UsedTopping);       //Ui������ ����
        }

        slots.Add(Toppings[n]);
        Instantiate(Toppings[n], Slot);
        Toppings[n].SetActive(true);
    }
    public void UseTopping()
    {
        if (slots[0].name == "LemonImage")
        {
            //���� �������� ������� ��
            Debug.Log("Lemon");
            PlayerMoveControl.i.ToppingJump(0);
        }
        else if(slots[0].name == "CherryImage")
        {
            //ü�� �������� ������� ��
            Debug.Log("Cherry");
            PlayerMoveControl.i.ToppingJump(1);
        }
        else if (slots[0].name == "MatchaImage")
        {
            //���� �������� ������� ��
            Debug.Log("Matcha");
        }

        GameObject UsedTopping = slots[0];
        slots.RemoveAt(0);          //����Ʈ���� 0��° ��� ����
        Destroy(UsedTopping);       //Ui������ ����
    }
    public string GetTopping()
    {
        return slots[0].name;
    }
}
