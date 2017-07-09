using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です  
    public List<ItemStatus> items = new List<ItemStatus>();

    void Start()
    {

        //名前(string),ID(int),持ち数(int),説明文（仮）(string),x座標(float),y座標(float),アイテム種類(int)
        items.Add(new ItemStatus("下駄", 0, 0, "足が速くなる", 44, -45, 2));
        items.Add(new ItemStatus("ろうそく", 1, 0, "命", 44, -38, 2));
        items.Add(new ItemStatus("鍵", 2, 0, "鍵のついた扉をあけることができる", 44, -44, 1));
        items.Add(new ItemStatus("お守り", 3, 0, "", 1, -33, 0));
        items.Add(new ItemStatus("こんぺいとう", 4, 0, "", 2, -32, 0));
        items.Add(new ItemStatus("テスト", 5, 0, "test", 44, -37, 2));
        items.Add(new ItemStatus("テスト２", 6, 0, "test", 44, -36, 2));
        items.Add(new ItemStatus("テスト３", 7, 0, "test", 44, -35, 1));
        items.Add(new ItemStatus("テスト４", 8, 0, "test", 1, -34, 0));
    }
    public void itemstorage()
    {

    }
}
