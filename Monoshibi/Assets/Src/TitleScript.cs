using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour {
    int classTimer;
    public MovieScript movie;
    public TitleStart fade;
    public AudioSource BGM;
    public GameStart fadeOut;
    public AudioSource SE;
    public GameObject titleImages;

    bool playSE;

    bool isMovie;
    bool isSceneMove;
    int movieIndex;

	// Use this for initialization
	void Start () {
        classTimer = 0;
        isMovie = false;
        isSceneMove = false;
        movieIndex = 0;

        playSE = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isSceneMove)
        {
            return;
        }
        KeyCheck();
        DemoCheck();
        if(movieIndex > 1)
        {
            movieIndex = 0;
        }

        ++classTimer;
	}

    //デモムービーとBGM管理用の時間判定
    void DemoCheck()
    {
        if (classTimer == 750)
        {
            isMovie = true;
            StartCoroutine(fade.FadeOut(60));
        }
        if (classTimer == 800)
        {
            movie.PlayMovie(movieIndex, 120);
        }
        if(classTimer == 860)
        {
            titleImages.SetActive(false);
        }
        if (classTimer == 2600)
        {
            titleImages.SetActive(true);
            StartCoroutine(movie.StopMovie(movieIndex));
            StartCoroutine(fade.FadeIn(60));
        }
        if(classTimer == 2660)
        {
            isMovie = false;
            ++movieIndex;
        }
        if (classTimer > 2800)
        {
            classTimer = 0;
        }
    }

    //キー入力用の時間判定
    void KeyCheck()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isMovie && classTimer < 2600)
            {
                //ムービーが終わる時間に飛ぶ
                classTimer = 2600;
            }else if(classTimer < 2600 && classTimer > 0)
            {
                //ゲームを開始する
                if (!playSE)
                {
                    SE.Play();
                    playSE = true;
                
                    fadeOut.fadeStart();
                }
            }
        }

    }
}
