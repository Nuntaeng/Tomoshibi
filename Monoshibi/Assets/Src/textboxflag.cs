using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textboxflag : MonoBehaviour {
    public Texture2D textbox;
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数
    void Start () {
        alfa = 0.0f;
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        GetComponent<Image>().color = new Color(red, green, blue, alfa);
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    public void hit_item(bool check)
    {
        if (check)
        {
            alfa = 1.0f;
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
        }
        else
        {
            alfa = 0.0f;
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
        }
    }
}
