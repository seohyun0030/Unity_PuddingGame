using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SlotManager : MonoBehaviour
{
    public static SlotManager i;

    [SerializeField] Transform Slot;   //������ ������ ����
    public int ToppingCnt = 1;      //���� ������ ���� ����
    public GameObject[] Toppings;
    public GameObject toppingObj;   //������ ���� �θ� ������Ʈ
    int n;      //���� ���� ��Ÿ��

    public List<GameObject> slots = new List<GameObject>();
    private void Awake()
    {
        i = this;
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
            Destroy(Slot.GetChild(0).gameObject);       //Ui������ ����
        }

        slots.Add(Toppings[n]);
        Instantiate(Toppings[n], Slot);
    }
    public void UseTopping()
    {
        if (slots[0].name != "3_ChocolateImage")    //������ ���� ���ݸ��� �ƴ� ��
            SfxManager.i.PlaySound("UseItem");      //������ ��� ȿ���� ���

        if (slots[0].name == "1_LemonImage")
        {
            //���� �������� ������� ��
            Debug.Log("Lemon");
            PlayerMoveControl.i.ToppingJump(0);
        }
        else if(slots[0].name == "2_CherryImage")
        {
            //ü�� �������� ������� ��
            Debug.Log("Cherry");
            PlayerMoveControl.i.ToppingJump(1);
        }
        else if (slots[0].name == "3_ChocolateImage")
        {
            //���ݸ� �������� ������� ��
            Debug.Log("Chocolate");
            PlayerMoveControl.i.ToppingJump(2);
            
        }
        else if (slots[0].name == "4_RasberryImage")
        {
            //����� �������� ������� ��
            Debug.Log("Rasberry");
            PlayerMoveControl.i.ToppingJump(3);
        }
        else if (slots[0].name == "5_MatchaImage")
        {
            //���� �������� ������� ��
            Debug.Log("Matcha");
            PlayerMoveControl.i.ToppingJump(4);
        }

        GameObject UsedTopping = slots[0];
        slots.RemoveAt(0);          //����Ʈ���� 0��° ��� ����
        Destroy(Slot.GetChild(0).gameObject);       //Ui������ ����
    }
    public void DeleteTopping()
    {
        Debug.Log(slots.Count);
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots != null)
            {
                slots.RemoveAt(i);
                Destroy(Slot.GetChild(i).gameObject);
            }
        }
    }
    public string GetTopping()
    {
        return slots[0].name;
    }
    public void RespawnTopping(Collider2D c)
    {
        StartCoroutine(Respawn(c));
    }
    public IEnumerator Respawn(Collider2D c)
    {
        yield return new WaitForSeconds(5f);
        c.gameObject.SetActive(true);
    }
}
