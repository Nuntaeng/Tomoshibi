using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * @brief 簡単なアニメーションを再生するとき使います
 * @author Heewon Kim (nabicore@icloud.com)
 */ 
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleAnimation : MonoBehaviour {

    public Sprite[] frames;
    public float duration = 0.5f;

    SpriteRenderer render;
    int currentFrame = 0;
    float tick = 0f;


    void Awake()
    {
        render = this.GetComponent<SpriteRenderer>();
        render.sprite = frames[0];
    }

    void Update()
    {
        tick += Time.fixedDeltaTime;
        if (tick >= duration)
        {
            tick = 0f;
            if (currentFrame < frames.Length - 1)
                currentFrame += 1;
            else
                currentFrame = 0;
            render.sprite = frames[currentFrame];
        }
    }
}
