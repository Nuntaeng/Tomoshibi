using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UItext : MonoBehaviour {
    public string[] scenario;//文章用文字配列
    public Text ui_Text;
    int scenariovaule;//配列数値
    float text_time;
	// Use this for initialization
	void Start () {
        TextUpdate();
        text_time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (scenariovaule < 5)
        {
            ++text_time;
            if (text_time % 120.0f == 0) //２秒
            {
                TextUpdate();
                text_time = 0.0f;//タイムフレームリセット
            }
        }
	}

    void TextUpdate()//テキスト表示
    {
        ui_Text.text = scenario[scenariovaule];
        ++scenariovaule;
    }
    
}
