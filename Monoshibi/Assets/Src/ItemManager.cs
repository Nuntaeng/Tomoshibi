using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    //public UnityEngine.GameObject itemObj;
    public string itemName;        //名前
    public int itemID;             //アイテムID
    public Sprite itemIcon;     //アイコン
    public int itemon_hand;       //アイテムの手持ち数
    public int itemType;      //アイテムの種類
    public string itemDesc;        //アイテムの説明文
}

[System.Serializable]
public class ItemManager : MonoBehaviour
{

    private ItemDataBase itemDataBase;
    public ItemData[] itemData;
    public GameObject itemdatabase;
    public GameObject Importancebox;
    public GameObject player;
    public GameObject textbox;
    public GameObject itemmanager;
    public GameObject itembox;
    public GameObject itemtext;
    void Start()
    {
        itemDataBase = itemdatabase.GetComponent<ItemDataBase>();

        for (int i = 0; i < itemData.Length; ++i)
        {     
            itemData[i].itemID = itemDataBase.items[i].itemID;//アイテムID
            itemData[i].itemDesc = itemDataBase.items[i].itemDesc;//アイテム説明参照
            itemData[i].itemName = itemDataBase.items[i].itemName;//アイテム名参照
            itemData[i].itemType = itemDataBase.items[i].itemType;//アイテム種類参照
            itemData[i].itemon_hand = itemDataBase.items[i].itemon_hand;//手持ち数
            itemData[i].itemIcon = itemDataBase.items[i].itemIcon;//アイコン
            
        }
    }
}