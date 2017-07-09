using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int posX, posY;
    public UnityEngine.GameObject itemObj;
    public string itemName;        //名前
    public int itemID;             //アイテムID
    public Texture2D itemIcon;     //アイコン
    public int itemon_hand;       //アイテムの手持ち数
    public int itemType;      //アイテムの種類
    public string itemDesc;        //アイテムの説明文
    public float item_x, item_y;  //アイテム座標
}

[System.Serializable]
public class ItemManager : MonoBehaviour
{

    private ItemDataBase itemDataBase;
    public ItemData[] itemData;
    //ItemStatus[] item;
    public GameObject itemtextdata;
    // Use this for initialization
    void Start()
    {
        itemDataBase = itemtextdata.GetComponent<ItemDataBase>();

        for (int i = 0; i < itemData.Length; ++i)
        {
            UnityEngine.GameObject clone;
            ItemStatus item;

            clone = Instantiate(itemData[i].itemObj, new Vector3(itemDataBase.items[i].item_x * 128, itemDataBase.items[i].item_y * 128, 1), Quaternion.identity, this.transform);//アイテムオブジェクトの配置
            item = clone.GetComponent<ItemStatus>();
            itemData[i].itemID = itemDataBase.items[i].itemID;//アイテムID
            itemData[i].item_x = itemDataBase.items[i].item_x;//アイテムX
            itemData[i].item_y = itemDataBase.items[i].item_y;//アイテムY
            itemData[i].itemDesc = itemDataBase.items[i].itemDesc;//アイテム説明参照
            itemData[i].itemName = itemDataBase.items[i].itemName;//アイテム名参照
            itemData[i].itemType = itemDataBase.items[i].itemType;//アイテム種類参照
            itemData[i].itemon_hand = itemDataBase.items[i].itemon_hand;//手持ち数
            item.itemID = itemDataBase.items[i].itemID;//アイテムID
            item.item_x = itemDataBase.items[i].item_x;//アイテムX
            item.item_y = itemDataBase.items[i].item_y;//アイテムY
            item.itemDesc = itemDataBase.items[i].itemDesc;//アイテム説明参照
            item.itemName = itemDataBase.items[i].itemName;//アイテム名参照
            item.itemType = itemDataBase.items[i].itemType;//アイテム種類参照
            item.itemon_hand = itemDataBase.items[i].itemon_hand;//手持ち数
        }
    }

    void Update()
    {
        //for (int i = 0; i < itemData.Length; ++i)
        //{
        //       this.transform.position = new Vector3(itemData[i].posX * 128, itemData[i].posY * 128, 1);
        //}
    }
}