using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemtext : MonoBehaviour
{
    public string[] scenario;//文章用文字配列
    public Text uiText;
    float texttime;
    public GameObject textbox;
    public bool hit_item;
    public class ItemData
    {
        public int posX, posY;
        public UnityEngine.GameObject itemObj;
    }
    int textmax = 0;
    public GameObject im;
    public GameObject g_itemtext;
    public GameObject itemdatabase;
    void Start()
    {
        ItemManager m = im.GetComponent<ItemManager>();
        for (int i = 0; i < m.itemData.Length; ++i)
        {
            textmax++;
            scenario[i] = m.itemData[i].itemName + "を拾った";
        }
    }
    public void candietext(int scenario_vaule)
    {
        Text t = g_itemtext.GetComponent<Text>();
        t.text = scenario[scenario_vaule];
        texttime = 1.0f;
    }
    void Update()
    {
        if (texttime > 0.0f)
        {
            texttime += 1.0f;//表示時間フレーム
            if (texttime >= 120.0f)
            {
                hit_item = false;
                GameObject textboxhit = GameObject.Find("textbox");
                textboxflag flag = textboxhit.GetComponent<textboxflag>();
                flag.hit_item(hit_item);
                Text t = GetComponent<Text>();
                t.text = "";
                texttime = 0.0f;
                // textbox.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.0f);
            }
        }
    }
}
