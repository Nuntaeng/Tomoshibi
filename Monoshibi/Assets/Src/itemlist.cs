using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemlist : MonoBehaviour
{
    public GameObject itemtextdata;
    public bool[] Image;
    public int[] id;
    int a;
    void Start()
    {
        a = this.transform.childCount;
        for (int i = 0; i < a; ++i)
        {
            GameObject icon = transform.GetChild(i).gameObject;
            icon.transform.localScale = new Vector3(0, 0, 0);
            Image[i] = false;
        }

    }


    void Update()
    {
        
    }
    public void itemget(int x)
    {
        for(int i = 0; i < a; ++i)
        {
            if (Image[i] == false)//アイテム画像が表示されてるか調べる
            {
                Image[i] = true;//表示させる
                GameObject icon = transform.GetChild(i).gameObject;
                icon.transform.localScale = new Vector3(1, 1, 1);

                GameObject im = GameObject.Find("ItemManager");
                ItemManager m = im.GetComponent<ItemManager>();
                id[i] = m.itemData[x].itemID;//アイテムボックスの番号にアイテムのIDを格納

                Debug.Log(id[i]);
                break;
                
            }
        }
        
    }
}
