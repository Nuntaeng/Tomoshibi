using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameFire : MonoBehaviour {

    float alfa;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数
    public Texture2D fire_position;
    public GameObject iti;
    PlayerStatus lightmove;
    public float candle_move;
    // Use this for initialization
    void Start () {
        alfa = 1.0f;
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
    }

    // Update is called once per frame
    void Update () {
        lightmove = iti.GetComponent<PlayerStatus>();
        candle_move = lightmove.lightPower;
        if (candle_move<0.0f)
        {
            alfa = 0;
            GetComponent<Image>().color = new Color(red, green, blue, alfa);            
        }
	}
}
