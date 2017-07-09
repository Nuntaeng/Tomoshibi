using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

    public float speed = 0.05f;  //不透明化の速さ
    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数
    bool fadeOut;
    Animator anim;
    GameObject fire;

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alfa = GetComponent<Image>().color.a;

        fadeOut = false;

        fire = GameObject.Find("fire");
        anim = fire.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            fadeOut = true;

            anim.SetBool("extFire", true);
        }
        if (fadeOut) { 
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += speed;
            if (alfa >= 1.0f)
            {
                Invoke("Load", 1.5f);
            }
        }
    }

    public void Load()
    {
            SceneManager.LoadScene("GameScene");
    }
}
