using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//この属性を使ってインスペクター上で表示
public class ItemStatus : MonoBehaviour
{
    public string itemName;        //名前
    public int itemID;             //アイテムID
    public Texture2D itemIcon;     //アイコン
    public int itemon_hand;       //アイテムの手持ち数
    public int itemType;      //アイテムの種類
    public string itemDesc;        //アイテムの説明文
    public float item_x, item_y;  //アイテム座標
    
    //ここでリスト化時に渡す引数をあてがいます   
    public ItemStatus(string name, int id, int hand, string desc,float x,float y,int type)
    {
        itemName = name;
        itemID = id;
        //アイコンはnameとイコールにするのでアイコンがあるパス＋nameで取ってきます    
        itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
        itemType = type;
        itemon_hand = hand;
        itemDesc = desc;
        item_x = x;
        item_y = y;
    }
}
