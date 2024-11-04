using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SlotManager : MonoBehaviour
{
    public static SlotManager i;

    [SerializeField] Transform Slot;   //토핑을 저장할 슬롯
    public int ToppingCnt = 1;      //저장 가능한 토핑 개수
    public GameObject[] Toppings;
    public GameObject toppingObj;   //토핑을 모은 부모 오브젝트
    int n;      //토핑 순서 나타냄

    public List<GameObject> slots = new List<GameObject>();
    private void Awake()
    {
        i = this;
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
            Destroy(Slot.GetChild(0).gameObject);       //Ui에서도 삭제
        }

        slots.Add(Toppings[n]);
        Instantiate(Toppings[n], Slot);
    }
    public void UseTopping()
    {
        if (slots[0].name != "3_ChocolateImage")    //아이템 사용시 초콜릿이 아닐 때
            SfxManager.i.PlaySound("UseItem");      //아이템 사용 효과음 재생

        if (slots[0].name == "1_LemonImage")
        {
            //레몬 아이템을 사용했을 때
            Debug.Log("Lemon");
            PlayerMoveControl.i.ToppingJump(0);
        }
        else if(slots[0].name == "2_CherryImage")
        {
            //체리 아이템을 사용했을 때
            Debug.Log("Cherry");
            PlayerMoveControl.i.ToppingJump(1);
        }
        else if (slots[0].name == "3_ChocolateImage")
        {
            //초콜릿 아이템을 사용했을 때
            Debug.Log("Chocolate");
            PlayerMoveControl.i.ToppingJump(2);
            
        }
        else if (slots[0].name == "4_RasberryImage")
        {
            //라즈베리 아이템을 사용했을 때
            Debug.Log("Rasberry");
            PlayerMoveControl.i.ToppingJump(3);
        }
        else if (slots[0].name == "5_MatchaImage")
        {
            //녹차 아이템을 사용했을 때
            Debug.Log("Matcha");
            PlayerMoveControl.i.ToppingJump(4);
        }

        GameObject UsedTopping = slots[0];
        slots.RemoveAt(0);          //리스트에서 0번째 요소 삭제
        Destroy(Slot.GetChild(0).gameObject);       //Ui에서도 삭제
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
