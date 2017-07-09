using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//調べるコマンドのためのコライダー判定
public class PlayerCheck : MonoBehaviour {
    PlayerStatus status;
    public Text textbox;
    Collider2D colli;

	void Start () {
        status = this.transform.parent.GetComponent<PlayerStatus>();
        colli = this.GetComponent<Collider2D>();
    }
	
	void Update () {
        //プレイヤーの向きに沿って判定を移動
        colli.offset = new Vector2(status.direction.x, status.direction.y + 0.5f) * 0.4f;
    }


    private void FixedUpdate()
    {
        //イベント中は文字をいじらない
        if (status.state != PlayerState.Idle)
        {
            textbox.text = "";
        }
    }
    //判定に入ったら調べられる
    private void OnTriggerStay2D(Collider2D collision)
    {
        //イベント中は判定しない
        if(status.state == PlayerState.Idle)
        {
            return;
        }

        
        //開く
        if (collision.gameObject.tag == "Door")
        {
            textbox.text = "Ctl:開く";
        }
        //調べる
        if (collision.gameObject.tag == "MapObject")
        {
            textbox.text = "Ctl:調べる";
        }

        //調べるボタンが押されたときに判定に仕事してもらう
        if (!Input.GetButtonDown("Fire1"))
        {
            return;
        }

        //ドアはイベントじゃなくてタグで感知、だってこっちのが楽だもん！
        if(collision.gameObject.tag == "Door")
        {           
            collision.gameObject.SetActive(false);
            textbox.text = "";
        }
        //イベントのオブジェクトさん、調べたフラグと共に座標をセット
        if (collision.gameObject.tag == "MapObject")
        {
            status.checkPos = new Vector2(collision.gameObject.transform.position.x / 128.0f, collision.gameObject.transform.position.y / -128.0f);
            textbox.text = "";
        }
    }
}
