using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief 竹とんぼの動きを定義
 * @author Heewon Kim (nabicore@icloud.com)
 */ 
[RequireComponent(typeof(NPCStatus))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TakeTonbo : MonoBehaviour {

    public float detectRange = 700f;

    bool isColliding = false;
    Transform player = null;
    NPCStatus status = null;
    Vector2 prevDir = Vector2.zero;
    Vector2 direction = Vector2.zero;
    Rigidbody2D rigid = null;
    static readonly Vector2[] DIRECTIONS = {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };


    void OnEnable()
    {
        direction = DIRECTIONS[Random.Range(0, DIRECTIONS.Length)];
        status = this.GetComponent<NPCStatus>();
        rigid = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        if (player == null)
            Debug.LogError("*****SERIOUS ERROR NO PLAYER*****");
    }

    void Update()
    {
        //プレイヤーと近づいたときの判定：プレイヤーから離れるようにがんばる
        float playerDist = Vector2.Distance(player.position, this.transform.position);
        if (playerDist <= detectRange && !isColliding)
        {
            status.state = PlayerState.Hiding;
            Vector2 maxDir = Vector2.zero;
            float maxDist = 0f;
            for (int i = 0; i < DIRECTIONS.Length; ++i)
            {
                // プレイヤーと予想経路間の距離計算
                Vector2 myPosWihDir = new Vector2(this.transform.position.x, this.transform.position.y) + (DIRECTIONS[i] * 64);
                float mukiDist = Vector2.Distance(player.position, myPosWihDir);
                
                // 一番遠い（プレイヤーから離れる）方向を算出
                if (maxDist < mukiDist)
                {
                    maxDir = DIRECTIONS[i];
                    maxDist = mukiDist;
                }

                // Debugging 
                Debug.Log(DIRECTIONS[i].ToString() + " => " + mukiDist);
                Debug.Log("---------------------");
            }
            // 次の移動経路として確定
            direction = maxDir;
            prevDir = direction;

            // Debugging
            Debug.Log("--------決定: " + direction.ToString());
        }
        else
            direction = prevDir;


        // 動く
        rigid.velocity = direction * status.speed * 64;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, (this.transform.position.y + 64.0f) / 128.0f);
        status.posX = (int)((this.transform.position.x + 64.0f) / 128.0f);
        status.posY = (int)((this.transform.position.y - 64.0f) / -128.0f);
    }

    // 壁などにぶつけた時ランダムな方向へ転換
    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        switch (status.state)
        {
            case PlayerState.Hiding:                //　プレイヤーから逃げている状態

                // 一番安全な経路を探す
                // ＊進行方向の逆方向にすればそこにプレイヤーがいる可能性もあるから一々計算して方向を決定
                Vector2 maxDir = Vector2.zero;
                float maxDist = 0f;
                for (int i = 0; i < DIRECTIONS.Length; ++i)
                {
                    Vector2 myPosWihDir = new Vector2(this.transform.position.x, this.transform.position.y) + (DIRECTIONS[i] * 64);
                    float mukiDist = Vector2.Distance(player.position, myPosWihDir);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(myPosWihDir, DIRECTIONS[i]);
                    foreach (var hit in hits)
                        if (hit.transform.gameObject.name.Contains("MapChip") ||
                            hit.transform.gameObject.name.Equals("Player"))
                        {
                            // 進行方向に壁がすぐある時
                            if (hit.transform.position.Equals(collision.transform.position))
                            {
                                mukiDist = 0f;
                                this.transform.position += new Vector3(DIRECTIONS[i].x * -1, DIRECTIONS[i].y * -1) * 20f;
                            }
                            else
                                mukiDist += Vector2.Distance(hit.transform.position, myPosWihDir);
                            
                            // Debugging
                            Debug.Log("PICKED => " + hit.transform.name + "//" + hit.transform.position.ToString() + " // " + Vector2.Distance(hit.transform.position, myPosWihDir) + "+" + Vector2.Distance((new Vector2(this.transform.position.x, this.transform.position.y) + (DIRECTIONS[i] * 64)), player.position) + "=" + mukiDist);
                            break;
                        }
                    if (maxDist < mukiDist)
                    {
                        maxDist = mukiDist;
                        maxDir = DIRECTIONS[i];
                    }
                    // Debugging 
                    Debug.Log(DIRECTIONS[i].ToString() + " => " + mukiDist);
                    Debug.Log("---------------------");
                }
                direction = maxDir;
                // Debugging
                Debug.Log("--------決定: " + direction.ToString());
                break;
        }
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
}
