using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class candie : MonoBehaviour
{
    public float candle_move;
    public Texture2D candie_position;
    PlayerStatus lightmove;
    public GameObject iti;
    public float StartScaleY = 0.0f;
    public CanvasGroup candleAll;
    // Use this for initialization
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0.0f;
        StartScaleY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
            lightmove = iti.GetComponent<PlayerStatus>();
            candle_move = lightmove.lightPower;
        //ろうそくの長さ（スケール）を、現在のライトに応じて変える
        if (!(candle_move < 0.0f))
        {
            float ScaleY = StartScaleY * lightmove.lightPower / lightmove.maxLightPower;
            transform.localScale = new Vector3(transform.localScale.x, ScaleY, transform.localScale.z);
        }
    }
    public void candleui()
    {
        GetComponent<CanvasGroup>().alpha = 1.0f;
    }
}
