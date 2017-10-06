using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{

    //リスト化をして下のvoid Start内でリストに値を追加、値は適当です  
    public List<ItemStatus> items = new List<ItemStatus>();

    void Start()
    {

        //名前(string),ID(int),持ち数(int),説明文仮(string),アイテム種類(int),画像名(string)※png型を探します、.pngは不要
        items.Add(new ItemStatus("下駄", 0, 0, "足が速くなる", 0, "itemGeta"));
        items.Add(new ItemStatus("ろうそく", 1, 0, "命", 2, "item_candle"));
        items.Add(new ItemStatus("鍵", 2, 0, "鍵のついた扉をあけることができる", 1, "itemKey"));
        items.Add(new ItemStatus("こけし", 3, 0, "", 2, "item_kokesi"));
        items.Add(new ItemStatus("お守り", 4, 0, "",  2, "item_omamori"));
        items.Add(new ItemStatus("木の板", 5, 0, "人1人の重さなら耐えられそうだ",  0, "itemIta"));
        items.Add(new ItemStatus("しおれた花", 6, 0, "花",  2, "item_flower"));
        items.Add(new ItemStatus("くし", 7, 0, "髪をとかすもの", 2, "Itemcomb"));
        items.Add(new ItemStatus("でんでん太鼓", 8, 0, "test", 2, "Itemdendendram"));
        items.Add(new ItemStatus("けん玉", 9, 0, "",  0, "Itemkendama"));
        items.Add(new ItemStatus("はしご", 10, 0, "",  0, "Itemladder"));
        items.Add(new ItemStatus("竹とんぼ", 11, 0, "",  0, "Itemtaketonbo"));
        items.Add(new ItemStatus("コマ", 12, 0, "", 0, "Itemkoma"));
        items.Add(new ItemStatus("風車", 13, 0, "",  0, "Itemkazaguruma"));
    }
}
