using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoGameOver : MonoBehaviour {
    public PlayerStatus status;
    public FadeScript FadeObj;
    public AudioSource BGM;
    public AudioSource Detect;
    public AudioSource Chase;
    public LightMove lightScript;
    public EnemyClass enemy;
    public FlagClass flags;
    

    bool coroutineFlag = false;
    bool isLastStage = false;

    private void Update()
    {
        //死亡時以外は実行しない
        if(status.state != PlayerState.Dead)
        {
            if (flags.flag[998])
            {
                isLastStage = true;
                return;
            }
            return;
        }

        //ゲームオーバーフラグを立てる
        flags.flag[999] = true;

        //ゲームオーバーのイベントが終わってからシーン移行イベントを起こす
        if (!flags.flag[998])
        {
            return;
        }

        //コルーチンは一度だけ実行
        if (!coroutineFlag)
        {
            StartCoroutine(GameOver());
            coroutineFlag = true;
        }
        //ゲームオーバーイベント後に敵を大量発生させる
        status.detectCount = 99999;
        enemy.ChasePoint = 99999;

    }

    IEnumerator GameOver()
    {
        float dec1 = BGM.volume / 10.0f;
        float dec2 = Detect.volume / 10.0f;
        float dec3 = Chase.volume / 10.0f;

        if (!isLastStage)
        {
            yield return new WaitForSeconds(2.0f);
        }
        lightScript.SetIntencity(-status.minLightCircle);

        yield return new WaitForSeconds(0.2f);
        FadeObj.render.color = new Color(0f, 0f, 0f);
        FadeObj.FadeOut(100);
        
        for(int i = 0; i < 10; ++i)
        {
            BGM.volume -= dec1;
            Detect.volume -= dec2;
            Chase.volume -= dec3;
            yield return null;
        }
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("GameOverScene");
    }
}
