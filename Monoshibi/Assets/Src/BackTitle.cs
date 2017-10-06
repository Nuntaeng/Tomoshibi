using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour {

    public float speed = 0.01f;  //不透明化の速さ
    float alfa;    //A値を操作するための変数
    bool fadeOut;
    string loadScene;

    // Use this for initialization
    void Start () {
        alfa = GetComponent<Image>().color.a;
        fadeOut = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1") && !fadeOut)
        {
            fadeOut = true;
            loadScene = "Retry";
        }
        if (Input.GetButtonDown("Fire2") && !fadeOut)
        {
            fadeOut = true;
            loadScene = "Title";
        }
        if (fadeOut)
        {
            GetComponent<Image>().color = new Color(0, 0, 0, alfa);
            alfa += speed;
            if (alfa >= 1.0f)
            {
                Invoke(loadScene, 1.5f);
            }
        }
    }


    public void Title()
    {
        SceneManager.LoadScene("JecLogoScene");
    }
    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }

}
