using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TextData
{
    public bool textdraw;
    public float text_x;
    public string textdata;
    public int drawtime;
}

public class TextManager : MonoBehaviour
{
    public int ChildText;
    public TextData[] textdata;
    public float texttime;
    public Image tomosibi;
    public List<TextData> texts = new List<TextData>();
    public Text owari;
    void Start()
    {
        texttime = -180.0f;
        ChildText = this.transform.childCount;//子の数

        CanvasGroup text_finalize = owari.GetComponent<CanvasGroup>();
        text_finalize.alpha = 0.0f;

        CanvasGroup tomosibi_a = tomosibi.GetComponent<CanvasGroup>();
        tomosibi_a.alpha = 0.0f;

        textdata[0].textdata = "企画／開発　日本電子専門学校";
        textdata[1].textdata = "ゲーム制作科　「ろうそく職人」"; 
        textdata[2].textdata = "メインプログラマ　　　　　　 　イベントシステム／本編リソース";
        textdata[3].textdata = "　板倉　広司";
        textdata[4].textdata = "アイテム／ U I ／エンドロール";
        textdata[5].textdata = "　金子　翔";
        textdata[6].textdata = "サウンドプログラム　　　　　　　　　　　　／ゲームオブジェクト";
        textdata[7].textdata = "　キム　ヒウォン";
        textdata[8].textdata = "メインデザイナー／シナリオ　　　　　　　　　　　　　／デモ制作";
        textdata[9].textdata = "　横田　麻梨子";
        textdata[10].textdata = "作曲／効果音制作／録音";
        textdata[11].textdata = "　佐藤　太晟";
        textdata[12].textdata = "ポスター／エンドロールイラスト";
        textdata[13].textdata = "　加藤　百合香様　　　　　　　　　　(アニメーション制作科)";
        textdata[14].textdata = "　オン　イェ　スル様　　　　　　　　(アニメーション研究科)";
        textdata[15].textdata = "開発ツール";
        textdata[16].textdata = "　U  n  i  t  y";

        for (int i = 0; i < ChildText; ++i)
        {
            RectTransform text_position = transform.GetChild(i).GetComponent<RectTransform>();
            GameObject text = transform.GetChild(i).gameObject;//子オブジェクトのi番目
            text.GetComponent<Text>().color = new Color(0.89f, 0.89f, 0.87f, 1);
            text.GetComponent<Text>().text = textdata[i].textdata;
            text_position.localPosition = new Vector3(-850.0f, 0.0f, 0.0f);//座標指定
            CanvasGroup Alpha = transform.GetChild(i).GetComponent<CanvasGroup>();
            Alpha.alpha = 0.0f;//テキスト透明化
            textdata[i].drawtime = (i + 1) * 480;//表示時間の設定
            textdata[i].textdraw = false;
            textdata[i].text_x = 0.0f;
        }
    }
    void Update()
    {
        CanvasGroup tomosibi_a = tomosibi.GetComponent<CanvasGroup>();
        CanvasGroup text_finalize = owari.GetComponent<CanvasGroup>();
        if (tomosibi_a.alpha <= 1 &&texttime < 800)
        {
            tomosibi_a.alpha += 0.02f;
        }
        texttime += 3.0f;
        for (int i=0; i < ChildText; ++i){
            CanvasGroup Alpha = transform.GetChild(i).GetComponent<CanvasGroup>();

            if (textdata[i].text_x >= 1200)
            {
                textdata[i].textdraw = false;
                Alpha.alpha = 0.0f;//テキスト消去
                if ((i+1) == textdata.Length)
                {
                    //最後の文章終了後
                    if (tomosibi_a.alpha != 0)
                    {
                        tomosibi_a.alpha -= 0.005f;
                    }
                    else
                    {
                        if (text_finalize.alpha <= 1 && texttime < 11000)
                        {
                            text_finalize.alpha += 0.01f;
                        }
                        else
                        {
                            text_finalize.alpha -= 0.01f;
                        }
                        if (text_finalize.alpha == 0)
                        {
                            SceneManager.LoadScene("JecLogoScene");//タイトル移動用
                        }
                    }
                }
            }
            else if (textdata[i].drawtime <= texttime)
            {
                textdata[i].textdraw = true;
            }
            if (textdata[i].textdraw == true)
            {
                if (Alpha.alpha <= 1 &&textdata[i].text_x < 600)
                {
                    Alpha.alpha += 0.005f;//テキスト徐々に表示
                }
                else if (Alpha.alpha != 0)
                {
                    Alpha.alpha -= 0.005f;//テキスト徐々に消す
                }
                textdata[i].text_x += 2.0f;
                RectTransform text_position = transform.GetChild(i).GetComponent<RectTransform>();
                text_position.localPosition = new Vector3(-850.0f + textdata[i].text_x, 0.0f, 0.0f);//座標移動
            }
            
        }
    }
}
