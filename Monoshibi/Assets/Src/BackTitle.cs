using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BackTitle : MonoBehaviour {

    public float speed = 0.01f;  //不透明化の速さ
    float alfa;    //A値を操作するための変数
    bool fadeOut;

    // Use this for initialization
    void Start () {
        alfa = GetComponent<Image>().color.a;
        fadeOut = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Return))
        {
            fadeOut = true;
        }
        if (fadeOut)
        {
            GetComponent<Image>().color = new Color(0, 0, 0, alfa);
            alfa += speed;
            if (alfa >= 1.0f)
            {
                Invoke("Load", 1.5f);
            }
        }
    }


    public void Load()
    {
        Application.LoadLevel("TitleScene");
    }

}
