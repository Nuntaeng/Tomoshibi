using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class itemsavedata
{
    public int itemid;
    public int item_hand;
    public int item_type;
}

public class Itemsavedata : MonoBehaviour {
    public static itemsavedata[] itemsave;
    public GameObject itemmanager;
    public GameObject itembox;
    public GameObject Importancebox;
    public static int candle_save = 0;
    public static int kokesi_save = 0;
    public static int omamori_save = 0;
    public static int flower_save = 0;
    public static int kusi_save = 0;
    public static int dendendram_save = 0;
    void Start()
    {
       
    }

    public void itemsaved(int id, int hand,int type)
    {
     
        if (id == 1) { candle_save = 1; }
        if (id == 3) { kokesi_save = 1; }
        if (id == 4) { omamori_save = 1; }
        if (id == 6) { flower_save = 1; }
        if (id == 7) { kusi_save = 1; }
        if (id == 8) { dendendram_save = 1; }
    }
    public void itemloads()
    {
        GameObject im = itemmanager;
        ItemManager m = im.GetComponent<ItemManager>();
        GameObject icon = itembox;
        itemlist item_icon = icon.GetComponent<itemlist>();
        GameObject importance_icon = Importancebox;
        Importancelist Importance_icon = importance_icon.GetComponent<Importancelist>();
   
        for(int i = 0; i < 14; ++i)
        {
            if (i == 1) {
                m.itemData[i].itemon_hand = candle_save;
                if (candle_save == 1) {
                    item_icon.itemget(i);
                }
            }
            if (i == 3) { m.itemData[i].itemon_hand = kokesi_save; if (kokesi_save == 1) { Importance_icon.itemget(i);} }
            if (i == 4) { m.itemData[i].itemon_hand = omamori_save; if (omamori_save == 1) { Importance_icon.itemget(i); } }
            if (i == 6) { m.itemData[i].itemon_hand = flower_save; if (flower_save == 1) { Importance_icon.itemget(i); } }
            if (i == 7) { m.itemData[i].itemon_hand = kusi_save; if (kusi_save == 1) { Importance_icon.itemget(i); } }
            if (i == 8) { m.itemData[i].itemon_hand = dendendram_save; if (dendendram_save == 1) { Importance_icon.itemget(i); } }
            
        }

    }

}
