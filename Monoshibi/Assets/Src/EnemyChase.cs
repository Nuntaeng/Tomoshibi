using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//追跡型エネミー挙動
public class EnemyChase : MonoBehaviour {
    //親に位置するエネミー管理クラス
    EnemyObjectClass enemyClass;

    //ステータス
    NPCStatus status;

    //配列マップ上での座標とその最大値
    int maxX, maxY;
    //目的地
    int destX, destY;

    //次に進む方向
    Vector2 direction;
    //前回進んだ方向
    Vector2 prevDir;

    //クラス内での経過時間を保存しておく
    public int classtimer;

    //マップ情報
    MapScript mapClass;
    int[,] map;

    //プレイヤー情報
    PlayerStatus playerStatus;

    //カウント
    int classCount;


    public void Initialize()
    {
        status = this.GetComponent<NPCStatus>();
        enemyClass = this.transform.GetComponent<EnemyObjectClass>();
        mapClass = enemyClass.map.GetComponent<MapScript>();
        playerStatus = enemyClass.status;

        status.posX = enemyClass.posX;
        status.posY = enemyClass.posY;


        this.transform.position = new Vector3((float)(status.posX * 128), (float)(status.posY * -128), 1);
        classtimer = 0;
        direction = new Vector2(1, 0);

        map = mapClass.GetMap();
        maxX = map.GetLength(1) - 1;
        maxY = map.GetLength(0) - 1;
        status.state = PlayerState.Idle;
        classCount = 0;
    }

    void Update()
    {
        MoveForDest();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y / 128.0f);

    }


    //設定した目的地へ移動
    void MoveForDest()
    {
        int posX = status.posX;
        int posY = status.posY;
        int speed = status.speed;

        if (classtimer == 0)
        {
            classtimer = 128;

            //移動が終わったら上下左右のマップデータを判定
            switch (playerStatus.state)
            {
                //プレイヤーを感知しているときはプレイヤーの位置を目的地に
                case PlayerState.Active:
                    destX = playerStatus.posX;
                    destY = playerStatus.posY;
                    break;
                //プレイヤーが通常時以外なら動かない
                default:
                    destX = posX;
                    destY = posY;
                    classtimer = 0;
                    break;
            }
            direction = ChaseDest(direction, destX, destY);

        }
        else if (Mathf.Abs(destX - posX) > 2 || Mathf.Abs(destY - posY) > 2)
        {
            //distance分だけ移動
            this.transform.localPosition += new Vector3(direction.x, direction.y, 0) * (speed + 1);
            classtimer -= speed;
            if (classtimer <= 0)
            {
                posX = (int)(this.transform.position.x / 128.0f);
                posY = (int)(-this.transform.position.y / 128.0f);
                classtimer = 0;
            }

        }
        else
        {
            //近くのマスにいるときはマス座標を無視して襲い掛かる（移動方向は2秒ごとに更新）


            if (classCount == 0)
            {
                prevDir = new Vector2(playerStatus.transform.position.x - this.transform.position.x, playerStatus.transform.position.y - this.transform.position.y);
                prevDir = prevDir.normalized * speed;
            }
            ++classCount;
            if(classCount > 120)
            {
                classCount = 0;
            }

            this.transform.localPosition += new Vector3(prevDir.x, prevDir.y, 0);
            posX = (int)(this.transform.position.x / 128.0f);
            posY = (int)(-this.transform.position.y / 128.0f);

            //遠ざかったらもう一回追尾判定する
            if (Mathf.Abs(destX - posX) > 2 || Mathf.Abs(destY - posY) > 2)
            {
                classtimer = 0;
                classCount = 0;
            }

        }


        //ステータスの変更は最後にまとめて変数に収める
        status.posX = posX;
        status.posY = posY;
        status.speed = speed;
    }

    //目的地に進んでみる
    Vector2 ChaseDest(Vector2 prevDir, int nDestX, int nDestY)
    {
        int posX = status.posX;
        int posY = status.posY;
        Vector2 direction = new Vector2(0.0f, 0.0f);

        if (nDestX > posX)
        {
            direction.x = 1.0f;
        }
        if (nDestX < posX)
        {
            direction.x = -1.0f;
        }
        if (nDestY > posY)
        {
            direction.y = -1.0f;
        }
        if (nDestY < posY)
        {
            direction.y = 1.0f;
        }

        //現在地と目的地が一致したときは移動しない
        if (posX == destX && posY == destY)
        {
            direction = new Vector2(0.0f, 0.0f);
        }

        return direction;
    }
}
