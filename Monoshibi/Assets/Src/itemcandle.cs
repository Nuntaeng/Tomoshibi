using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemcandle : MonoBehaviour
{
    public bool hit_item = false;

    public ItemData[] itemdata;
    public ItemManager itemmanager;
    
    void Start()
    {
        
    }
    void OnTriggerStay2D(Collider2D hit)
    {
        if (hit.gameObject == GameObject.Find("Player"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //hit処理
                hit_item = true;//flag.hitに送るbool型
                GameObject textboxhit = GameObject.Find("textbox");//テキストボックスの画像
                textboxflag flag = textboxhit.GetComponent<textboxflag>();
                flag.hit_item(hit_item);//これでテキストボックスを表示させる

                this.gameObject.SetActive(false);//アイテムオブジェクト非表示

                ItemStatus status = this.gameObject.GetComponent<ItemStatus>();
                GameObject im = GameObject.Find("ItemManager");       
                ItemManager m = im.GetComponent<ItemManager>();
                GameObject itemtext = GameObject.Find("itemtext");
                itemtext ct = itemtext.GetComponent<itemtext>();//テキスト文字

                for (int i = 0; i < m.itemData.Length; ++i)//入手したアイテムIDとデータベースを照合
                {
                    if (m.itemData[i].itemID == status.itemID)
                    {
                        m.itemData[i].itemon_hand = 1;//手持ち数を１に
                        ct.candietext(m.itemData[i].itemID);//テキスト表示
                    }
                }

                GameObject icon = GameObject.Find("itemboxs");
                itemlist item_icon = icon.GetComponent<itemlist>();
                item_icon.itemget(status.itemID);
            }
        }
    }
}