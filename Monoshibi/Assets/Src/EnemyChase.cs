using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//追跡型エネミー挙動
public class EnemyChase : MonoBehaviour {
    //親に位置するエネミー管理クラス
    EnemyObjectClass enemyClass;

    //ステータス
    NPCStatus status;

    //目的地
    int destX, destY;

    //次に進む方向
    Vector2 direction;
    Vector2 prevDir;

    //クラス内での経過時間を保存しておく
    [HideInInspector]
    public int classtimer;

    //方向転換の間隔と、待機時間（フレーム単位）
    public int turnInterval;
    public int stopInterval;
    //追尾のブレ（マス単位）
    public int randamDirection;

    //マップ情報
    MapScript mapClass;
    int[,] map;

    //プレイヤー情報
    PlayerStatus playerStatus;

    //カウント
    int classCount;
    System.Random rand;


    public void Initialize()
    {
        status = this.GetComponent<NPCStatus>();
        enemyClass = this.transform.GetComponent<EnemyObjectClass>();
        mapClass = enemyClass.map.GetComponent<MapScript>();
        playerStatus = enemyClass.playerStatus;

        status.posX = enemyClass.posX;
        status.posY = enemyClass.posY;


        this.transform.position = new Vector3((float)(status.posX * 128), (float)(status.posY * -128), 1);
        classtimer = 0;

        map = mapClass.GetMap();
        status.state = PlayerState.Idle;
        classCount = -10;
        direction = Vector2.zero;
        rand = new System.Random();
    }

    //更新
    void Update()
    {
        //プレイヤーがイベント中なら何もしない
        if(playerStatus.state == PlayerState.Idle)
        {
            return;
        }
        MoveForDest();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y / 128.0f);

        //一定時間経過後、消えていなくなる
        if(classtimer > 2400)
        {
            FadeEnemy();
        }
        ++classtimer;
    }


    //設定した目的地へ移動
    void MoveForDest()
    {
        int posX = status.posX;
        int posY = status.posY;
        int speed = status.speed;
        prevDir = direction;

        //プレイヤーを追いかける(目的地を設定フレームごとに更新)
        {
            if (classCount == 0)
            {
                //プレイヤーの位置をぶれて認識させる
                float playerX = playerStatus.transform.position.x + rand.Next(randamDirection * 128) / 10.0f;
                float playerY = playerStatus.transform.position.y + rand.Next(randamDirection * 128) / 10.0f;
                float dirX = playerX - this.transform.position.x;
                float dirY = playerY - this.transform.position.y;
                direction = new Vector2(dirX, dirY);
                direction = direction.normalized * speed;
                
            }
            setAnimation();
            ++classCount;
            //方向転換前に指定フレーム分待機(最大二秒ほどのブレがある)
            if(classCount > turnInterval + rand.Next(120))
            {
                classCount = -stopInterval - rand.Next(20);
            }
            if (classCount < 0)
            {
                direction = direction * 0.9f;
            }
            this.transform.localPosition += new Vector3(direction.x, direction.y, 0);
            posX = (int)(this.transform.position.x / 128.0f);
            posY = (int)(-this.transform.position.y / 128.0f);
        }


        //ステータスの変更は最後にまとめて変数に収める
        status.posX = posX;
        status.posY = posY;
        status.speed = speed;


    }

    //アニメーション用の変数をセット
    void setAnimation()
    {
        Animator animator = GetComponent<Animator>();

        if (classCount <= 1)
        {
            animator.SetBool("isMove", false);
        }
        else
        {
            animator.SetBool("isMove", true);
        }
        if(classCount < 0)
        {
            return;
        }


        float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 180.0f + 22.0f;
        //Debug.Log(direction);
        int animID = (int)(angle / 45.0f);
        animator.SetInteger("animID", animID);
    }

    //一定時間経過後にエネミーは消滅
    void FadeEnemy()
    {
        if (enemyClass.killEnable)
        {
            return;
        }
        enemyClass.KillEnemy();
    }
}
