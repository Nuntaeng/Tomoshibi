using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemlist : MonoBehaviour
{
   
    public bool[] Image;
    public int[] id;
    public Image image;
    int childobj;
    public GameObject im;
    public GameObject candle;
    void Start()
    {
        childobj = this.transform.childCount;
        for (int i = 0; i < childobj; ++i)
        {
            GameObject icon = transform.GetChild(i).gameObject;
            icon.transform.localScale = new Vector3(0, 0, 0);//イメージ非表示
            Image[i] = false;//イメージ表示有無のフラグ
        }

    }  
    public void itemget(int itemid)
    {
        for(int i = 0; i < childobj; ++i)
        {
            if (Image[i] == false)//フラグが表示か調べる
            {
                Image[i] = true;//フラグを表示
                GameObject icon = transform.GetChild(i).gameObject;
                icon.transform.localScale = new Vector3(1, 1, 1);//イメージ表示
                
                ItemManager m = im.GetComponent<ItemManager>();
                id[i] = m.itemData[itemid].itemID;//アイテムボックスの番号にアイテムのIDを格納

                image = icon.GetComponent<Image>();
                image.sprite = m.itemData[itemid].itemIcon;//イメージにアイコンを埋め込む

                if (m.itemData[itemid].itemID == 1)
                {
                    if (m.itemData[itemid].itemon_hand == 1)
                    {                      
                        candie candleUI = candle.GetComponent<candie>();
                        candleUI.candleui();
                    }
                }
                break;
                
            }
        }
    }
    public void itemlost(int itemid)
    {
        ItemManager m = im.GetComponent<ItemManager>();
        if (m.itemData[itemid].itemon_hand >= 1)
        {
            int boxNo = -1;
            do
            {
                ++boxNo;
            } while (id[boxNo] != itemid);//アイテムIDのアイテムボックス番号を調べる
            while (Image[boxNo+1]==true)//１つ先のイメージがあるかどうか調べる
            {
                GameObject image_icon = transform.GetChild(boxNo).gameObject;
                image = image_icon.GetComponent<Image>();
                image.sprite = m.itemData[boxNo+1].itemIcon;//イメージにアイコンを埋め込む
                id[boxNo] = id[boxNo + 1];//１つ先のIDを今のIDに上書き
                ++boxNo;
            }
            Image[boxNo] = false;
            GameObject icon = transform.GetChild(boxNo).gameObject;
            icon.transform.localScale = new Vector3(0, 0, 0);//イメージ非表示
        }
    }
}
