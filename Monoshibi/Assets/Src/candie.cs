using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candie : MonoBehaviour
{
    public float candie_move;
    public Texture2D candie_position;
    PlayerStatus lightmove;
    public GameObject iti;
    public float StartScaleY = 0.0f;
    // Use this for initialization
    void Start()
    {

        StartScaleY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
            lightmove = iti.GetComponent<PlayerStatus>();
            candie_move = lightmove.lightPower;
            //ろうそくの長さ（スケール）を、現在のライトに応じて変える
            float ScaleY = StartScaleY * (candie_move) / 20.0f;
            transform.localScale = new Vector3(transform.localScale.x, ScaleY, transform.localScale.z);
    }
}
