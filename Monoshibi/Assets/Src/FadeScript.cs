using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {
    [HideInInspector]
    public Image render;
    

	void Start () {
        render = this.GetComponent<Image>();
        Color color = render.color;
        render.color = new Color(color.r, color.g, color.b, 1.0f);
    }



    public void FadeIn(int time)
    {
        StartCoroutine(In(time));
    }
    public void FadeOut(int time)
    {
        StartCoroutine(Out(time));
    }

    public void FadeIO(int time) {
        StartCoroutine(IO(time));
    }

    IEnumerator In(int time)
    {
        Color color = render.color;
        for (int i = 1; i <= time; ++i)
        {
            render.color = (new Color(color.r, color.g, color.b, 1.0f - ((float)i / (float)time)));
            yield return null;
        }
    }
    IEnumerator Out(int time)
    {
        Color color = render.color;
        for (int i = 1; i <= time; ++i)
        {
            render.color = (new Color(color.r, color.g, color.b, ((float)i / (float)time)));
            yield return null;
        }
    }

    IEnumerator IO(int time) {
        yield return StartCoroutine(In(time));
        yield return StartCoroutine(Out(time));
    }
}
