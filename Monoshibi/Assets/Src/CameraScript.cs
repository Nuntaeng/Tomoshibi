using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;
    //プレイヤーに追従するか？
    public bool isPlayerChase = true;
    //カメラの現在地と、そこからのずれ
    public Vector3 cameraPos;
    public Vector3 shiftCount;

    private void Start()
    {
        cameraPos = this.transform.position;
        cameraPos = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z - 100.0f);

        shiftCount = Vector3.zero;
    }
    private void FixedUpdate()
    {
        if (isPlayerChase)
        {
            cameraPos = this.transform.position;
            cameraPos += (player.transform.position - this.transform.position) / 15.0f;
            cameraPos = new Vector3(cameraPos.x, cameraPos.y, player.transform.position.z - 100.0f);
        }
        //相対位置
        this.transform.position = cameraPos + shiftCount;
    }
    //カメラ追従
    private void LateUpdate()
    {

        if (isPlayerChase)
        {
            cameraPos = this.transform.position;
            cameraPos += (player.transform.position - this.transform.position) / 15.0f;
            cameraPos = new Vector3(cameraPos.x, cameraPos.y, player.transform.position.z - 100.0f);
        }
        //相対位置
        this.transform.position = cameraPos + shiftCount;
    }

    //カメラを揺らして画面揺れを実現
    public void Quake(int time, int power)
    {
        StartCoroutine(QuakeColutine(time, power));
    }
    //揺れをするコルーチン
    IEnumerator QuakeColutine(int time, int power)
    {
        float decrease = (float)power / (float)time;
        float quakePow = power;
        for(int i = 0; i < time; ++i)
        {
            //ループ一回ごとにプラスとマイナスを切り替える
            int plusMinus = i % 2 * 2 - 1;
            shiftCount = new Vector3(0.0f, quakePow * plusMinus * 2, 0.0f);
            quakePow -= decrease;
            yield return null;
            yield return null;
        }
        shiftCount = new Vector3(0.0f, 0.0f, 0.0f);

        yield return null;
    }

}
