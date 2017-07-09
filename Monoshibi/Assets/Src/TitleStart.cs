using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TitleStart : MonoBehaviour
{

    public float speed = 0.01f;  //透明化の速さ
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数
    bool fadeIn;


    // Use this for initialization
    void Start()
    {
        red = 0;
        green = 0;
        blue = 0;
        alfa = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa -= speed;
            if (alfa <= 0.2f)
            {
                alfa = 0.2f;
            }
    }
}