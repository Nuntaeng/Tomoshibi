using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour {
    float time;
    float intencityDest;
    PlayerStatus status;

	void Start () {
        status = this.transform.parent.GetComponent<PlayerStatus>();
        time = 0.0f;
        intencityDest = status.lightPower;
    }

    void FixedUpdate () {
        time += 0.05f;
        if(status.ditectPoint >= 0.8f)
        {
            time += (status.ditectPoint - 0.8f) * 2.0f;
        }

        //向きを参考に光の位置を調整
        this.transform.localPosition = new Vector3(status.direction.x * 0.5f, status.direction.y * 0.5f + 0.4f, 0.0f);
        //揺らめくみたいに移動させる
        this.transform.localPosition += new Vector3(Mathf.Cos(time) * 0.05f ,Mathf.Sin(time) * 0.05f, 0.0f);
        //光の明るさをセット
        float scale = status.lightPower + (Mathf.Sin(time) - 1) * 0.4f + 80.0f;

        scale *= 0.01f;








        //隠れているときは暗くする
        if (status.state == PlayerState.Hiding)
        {
            scale *= 0.5f;
        }

        //死んでるときはライトを消す
        if (status.state != PlayerState.Dead)
        {
            SetIntencity(status.lightPower -= 0.001f);
        }
        else
        {
            SetIntencity(-80.0f);
        }
        if (status.lightPower <= 0.0f)
        {
            SetIntencity(-80.0f);
        }

        if (scale < 0.0f)
        {
            scale = 0.0f;
        }

        this.transform.localScale = new Vector3(scale, scale, 1.0f);
        GoLightDest();
	}

    //光の強さを調整
    public void SetIntencity(float power)
    {
        intencityDest = power;
    }
    //なめらかに光の強さが変化
    void GoLightDest()
    {
        float intencity = status.lightPower;
        if (intencity != intencityDest)
        {
            intencity += (intencityDest - intencity) / 5.0f;
            if((intencityDest - intencity) < 0.05f && (intencityDest - intencity) > -0.05f)
            {
                intencity = intencityDest;
            }
        }
        status.lightPower = intencity;
    }
}
