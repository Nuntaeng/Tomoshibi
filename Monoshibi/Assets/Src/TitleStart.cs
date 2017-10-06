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
        StartCoroutine(FadeIn(60));
    }

    // Update is called once per frame
    void Update()
    {
            //GetComponent<Image>().color = new Color(red, green, blue, alfa);
            //alfa -= speed;
            //if (alfa <= 0.2f)
            //{
            //    alfa = 0.2f;
            //}
    }

    //フェードインとアウト
    public IEnumerator FadeIn(int time)
    {
        Image tex = GetComponent<Image>();

        for (int i = 0; i < time; ++i)
        {
            tex.color = new Color(tex.color.r, tex.color.g, tex.color.b, 1.0f - (float)i / time);
            yield return null;
        }
        yield return null;
    }
    public IEnumerator FadeOut(int time)
    {
        Image tex = GetComponent<Image>();

        for (int i = 0; i < time; ++i)
        {
            tex.color = new Color(tex.color.r, tex.color.g, tex.color.b, (float)i / time);
            yield return null;
        }
        yield return null;
    }
}