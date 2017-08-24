using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour {


	public Animator animator = null;
	float moveSpeed = 2f;
    PlayerStatus status;
    Rigidbody2D rigid;

    
	void Start () {
        status = this.GetComponent<PlayerStatus>();
        status.state = PlayerState.Active;
        rigid = this.GetComponent<Rigidbody2D>();
        this.transform.position = new Vector3(status.posX * 128.0f, -status.posY * 128.0f, (status.posY - 64.0f) / 128.0f);
    }
	
	void Update () {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        
        //移動方向を十字へ固定
        if(Mathf.Abs(moveX) > Mathf.Abs(moveY))
        {
            moveY = 0.0f;
        }
        else
        {
            moveX = 0.0f;
        }

        //イベント中にIdle状態になる
        if (status.state == PlayerState.Idle)
        {
            rigid.velocity = Vector2.zero;
            status.posX = (int)((this.transform.position.x) / 128.0f);
            status.posY = (int)((this.transform.position.y) / -128.0f);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, (this.transform.position.y + 64.0f) / 128.0f);
            //Debug.Log("Idle");
            return;
        }


        //プレイヤーの向きをセット
        if (moveX != 0.0f || moveY != 0.0f)
        {
            status.direction = (new Vector2(moveX, moveY) * 100.0f).normalized;
        }

        //プレイヤーが生きてる時の処理
        if (status.state != PlayerState.Dead)
        {
            //アニメーターに値をセット
            animator.SetFloat("Player_Input_Y", moveY);
            animator.SetFloat("Player_Input_X", moveX);

            //力を加える
            rigid.velocity = new Vector2(moveX, moveY) * status.moveSpeed * 64;
            //Zの値をYの値を用いて調整
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, (this.transform.position.y + 64.0f) / 128.0f);




            //隠れているときは判定を消しておく
            if (status.state == PlayerState.Hiding)
            {
                this.transform.GetChild(2).GetComponent<CircleCollider2D>().radius = 0.0f;
            }
            else
            {
                    this.transform.GetChild(2).GetComponent<CircleCollider2D>().radius = 4.0f;
            }
        }
        else
        {
            //プレイヤーの状態が「Dead」の時
            this.animator.Stop();
            rigid.velocity = new Vector2(0, 0);
        }

        status.posX = (int)((this.transform.position.x + 64.0f) / 128.0f);
        status.posY = (int)((this.transform.position.y - 64.0f) / -128.0f);
    }


    //面倒くさいからとりあえずここに書いとく
    public void WalkEffect(string name)
    {
        SEManager.Instance.SetVolume(0.6f);
        SEManager.Instance.PlaySE(name);
    } 
}


