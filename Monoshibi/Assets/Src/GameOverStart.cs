using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverStart : MonoBehaviour {

    public float speed = 0.01f;  //パネルの透明化の速さ
    float alfa;    //パネルのA値を操作するための変数
    bool textAppear;

    public UnityEngine.GameObject Text;
    float redT, greenT, blueT, alfaT;  //GameOverテキストの色
    public float speedT = 0.001f;  //GameOverテキストの透明化の速さ

    // Use this for initialization
    void Start () {
        alfa = 1.0f;
        textAppear = false;

        Text = UnityEngine.GameObject.Find("GameOver2");
        if (Text)
        {
            redT = Text.GetComponent<SpriteRenderer>().color.r;
            greenT = Text.GetComponent<SpriteRenderer>().color.g;
            blueT = Text.GetComponent<SpriteRenderer>().color.b;
            alfaT = 0.0f;
        }
        else
        {
            Debug.Log("Text Not Find");
        }
    }

    // Update is called once per frame
    void Update () {
        GetComponent<Image>().color = new Color(0, 0, 0, alfa);
        alfa -= speed;
        if (alfa <= 0.0f)
        {
            alfa = 0.0f;
            textAppear = true;
        }

        Text.GetComponent<SpriteRenderer>().color = new Color(redT, greenT, blueT, alfaT);
        if (textAppear)
        {
            alfaT += speedT;
        }
    }
}
