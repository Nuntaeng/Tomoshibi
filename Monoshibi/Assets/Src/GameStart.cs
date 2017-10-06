using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

    public AudioSource BGM;
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
        if (fadeOut) { 
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += speed;
            if (alfa >= 1.0f)
            {
                Invoke("Load", 1.5f);
            }
        }
    }

    //外部からキー入力があったときにシーン移行を開始
    public void fadeStart()
    {
        fadeOut = true;
        StartCoroutine(BGMFadeOut(60));
        anim.SetBool("extFire", true);
    }

    public void Load()
    {
        ////////////////////////////////////
        //マップとアイテムのセーブを初期化//
        ////////////////////////////////////
        Itemsavedata.candle_save = 0;
        Itemsavedata.kokesi_save = 0;
        Itemsavedata.omamori_save = 0;
        Itemsavedata.flower_save = 0;
        Itemsavedata.kusi_save = 0;
        Itemsavedata.dendendram_save = 0;
        MapScriptTMX.savedMapNumber = -1;
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator BGMFadeOut(int time)
    {
        //BGMのボリュームは０から１で都合よく割合計算に利用できるので保持
        float BGMVolume = BGM.volume;
        //BGMをフェードアウト
        for(int i = 0; i < time; ++i)
        {
            BGM.volume = (1.0f - (float)i / (float)time) * BGMVolume;
            yield return null;
        }
    }
}
