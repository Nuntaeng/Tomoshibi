using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミークラス、エネミーのパラメータと行動、感知を管理、エネミーは一人なので機能をまとめても問題なかろ
public class EnemyClass : MonoBehaviour {
    //感知度、一定値ditectMaxに達したらプレイヤーの近くにエネミーを召喚する
    public int ditectCount;
    public int ditectMax;
    public AudioSource DitectSound;
    public AudioSource BGM;
    public PlayerStatus status;

    //そもそも今プレイヤーの周りにいるかという判定
    public bool enable;


    private void Start()
    {
        ditectCount = 0;
        DitectSound.volume = 0;

    }

    private void Update()
    {
        //エネミーが有効で、プレイヤーが隠れていないときは、徐々に感知度が上昇
        if (!enable)
        {
            return;
        }

        ++ditectCount;

        //感知度が規定数を超えるとなんか・・・こう・・・どばああっ！って感じ・・・？
        if (ditectCount >= ditectMax)
        {
            ditectCount = 0;
            ditectMax = 1;
            BGM.volume = 0;
            //SEManager.Instance.PlaySE("nechi001");
        }

        float ditectPoint = (float)ditectCount / (float)ditectMax;
        status.ditectPoint = ditectPoint;
        DitectSound.volume = ditectPoint;
    }
}
