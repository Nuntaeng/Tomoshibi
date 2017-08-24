using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour {

    PlayerStatus player;

	void Start () {
        //コンポーネントを取得
        player = this.transform.parent.GetComponent<PlayerStatus>();
	}
	
	void Update () {
		
	}
    
    //衝突
    private void OnTriggerStay2D(Collider2D collision)
    {
        //敵に捕まった
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(DelayProcess());
        }

    }

    private IEnumerator DelayProcess()
    {
        player.state = PlayerState.Dead;
        yield return new WaitForSeconds(2.0f);

    }
}
