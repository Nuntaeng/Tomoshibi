using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour {

    PlayerStatus player;

	void Start () {
        //もっとうまい取得方法はないんでしょうかね、たぶんこれが一番うまいと思います
        player = this.transform.parent.GetComponent<PlayerStatus>();
	}
	
	void Update () {
		
	}
    
    //衝突
    private void OnTriggerStay2D(Collider2D collision)
    {
        //衝突


        //敵に捕まった
        //if (collision.gameObject.tag == "Player")
        //{
        //    StartCoroutine(DelayProcess());
        //}


        if (player.state != PlayerState.Dead)
        {
            //隠れる場所に入った
            if (collision.gameObject.tag == "HidingSpace")
            {
                player.state = PlayerState.Hiding;
            }
        }
    }

    //衝突の終了時
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player.state != PlayerState.Dead)
        {
            //隠れる場所から出た
            if (collision.gameObject.tag == "HidingSpace")
            {
                player.state = PlayerState.Idle;
            }
        }
    }

    private IEnumerator DelayProcess()
    {
        player.state = PlayerState.Dead;
        yield return new WaitForSeconds(0.5f);
    }
}
