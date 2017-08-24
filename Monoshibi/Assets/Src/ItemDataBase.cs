using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です  
    public List<ItemStatus> items = new List<ItemStatus>();

    void Start()
    {

        //名前(string),ID(int),持ち数(int),説明文仮(string),x座標(float),y座標(float),アイテム種類(int),画像名(string)※png型を探します、.pngは不要
        items.Add(new ItemStatus("下駄", 0, 0, "足が速くなる", -1000, 1000, 2, "itemGeta"));
        items.Add(new ItemStatus("ろうそく", 1, 0, "命", -1000, 1000, 2, "itemCandle"));
        items.Add(new ItemStatus("鍵", 2, 0, "鍵のついた扉をあけることができる", -1000, 1000, 1, "itemKey"));
        items.Add(new ItemStatus("こけし", 3, 0, "", -1000, 1000, 0, "test4"));
        items.Add(new ItemStatus("お守り", 4, 0, "", -1000, 1000, 0, "test5"));
        items.Add(new ItemStatus("木の板", 5, 0, "人1人の重さなら耐えられそうだ", -1000, 1000, 2, "itemIta"));
        items.Add(new ItemStatus("しおれた花", 6, 0, "花", -1000, 1000, 2, "test7"));
        items.Add(new ItemStatus("くし", 7, 0, "髪をとかすもの", -1000, 1000, 1, "test8"));
        items.Add(new ItemStatus("でんでん太鼓", 8, 0, "test", -1000, 1000, 0, "test9"));
    }
    public void itemstorage()
    {

    }
}
