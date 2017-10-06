using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieScript : MonoBehaviour {
    public float panelAlpha;
    public MovieTexture[] movie;
    int classTimer;
    int movieIndex;

	// Use this for initialization
	void Start () {
        classTimer = 0;
        this.GetComponent<UnityEngine.UI.Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.gameObject.GetComponent<UnityEngine.UI.Image>().material.mainTexture = (Texture)movie[movieIndex];
    }

    //Movie再生
    public void PlayMovie(int index, int time)
    {
        movieIndex = index;
        StartCoroutine(FadeIn(time));
    }
    //Movie停止
    public IEnumerator StopMovie(int index)
    {
        movieIndex = index;
        StartCoroutine(FadeOut(120));
        yield return new WaitForSeconds(1.0f);
    }

    //フェードイン
    IEnumerator FadeIn(int time)
    {
        UnityEngine.UI.Image texture = this.GetComponent<UnityEngine.UI.Image>();



        int looptime = time / 2;
        for (int i = 0; i <= looptime; ++i)
        {
            texture.color = new Color(texture.color.r, texture.color.g, texture.color.b, panelAlpha * i / looptime);
            yield return null;
        }

        movie[movieIndex].Play();

        texture.enabled = false;
        yield return null;
        texture.enabled = true;

        for (int i = 0; i <= looptime; ++i)
        {
            float color = panelAlpha * i / looptime;
            texture.color = new Color(color, color, color, texture.color.a);
            yield return null;
        }


        yield return null;
    }

    //フェードアウト
    IEnumerator FadeOut(int time)
    {
        UnityEngine.UI.Image texture = this.GetComponent<UnityEngine.UI.Image>();
        int looptime = time / 2;

        for (int i = 0; i <= looptime; ++i)
        {
            float color = panelAlpha * (1.0f - (float)i / looptime);
            texture.color = new Color(color, color, color, texture.color.a);
            yield return null;
        }
        for (int i = 0; i <= looptime; ++i)
        {
            texture.color = new Color(texture.color.r, texture.color.g, texture.color.b, panelAlpha * (1.0f - (float)i / looptime));
            yield return null;
        }
        movie[movieIndex].Stop();
        yield return null;
    }
}
