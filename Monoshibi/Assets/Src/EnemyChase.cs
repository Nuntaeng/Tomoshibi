using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//追跡型エネミー挙動
public class EnemyChase : MonoBehaviour {
    //親に位置するエネミー管理クラス
    NPCClass enemyClass;

    //ステータス
    NPCStatus status;

    //配列マップ上での座標とその最大値
    int maxX, maxY;
    //目的地
    int destX, destY;
    //マップ上での自身の初期位置
    int firstX, firstY;

    //次に進む方向
    Vector2 direction;
    //クラス内での経過時間を保存しておく
    public int classtimer;

    //マップ情報
    MapScript mapClass;
    int[,] map;

    //プレイヤー情報
    UnityEngine.GameObject player;



    void Start()
    {
        status = this.GetComponent<NPCStatus>();
        enemyClass = this.transform.parent.GetComponent<NPCClass>();
        mapClass = enemyClass.mapObj.GetComponent<MapScript>();
        player = enemyClass.player;

        status.posX = enemyClass.NPC[status.index].posX;
        status.posY = enemyClass.NPC[status.index].posY;


        this.transform.position = new Vector3((float)(status.posX * 128), (float)(status.posY * -128), 1);
        classtimer = 0;
        direction = new Vector2(1, 0);

        map = mapClass.GetMap();
        maxX = map.GetLength(1) - 1;
        maxY = map.GetLength(0) - 1;
        firstX = status.posX;
        firstY = status.posY;
        status.state = PlayerState.Idle;
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

            //移動が終わったら上下左右のマップデータを判定
            switch (status.state)
            {
                //プレイヤーを感知しているときはプレイヤーの位置を目的地に
                case PlayerState.Active:
                    destX = (int)((player.transform.position.x + 64.0f) / 128.0f);
                    destY = (int)((-player.transform.position.y + 64.0f) / 128.0f);
                    speed = 5;
                    break;
                //プレイヤーが発見されていないときは追跡前の位置に戻ろうとする
                case PlayerState.Idle:
                    destX = firstX;
                    destY = firstY;
                    speed = 2;
                    break;
                //プレイヤーを見失うと壁に当たるまで直進
                case PlayerState.Hiding:
                    destX += (int)direction.x;
                    destY -= (int)direction.y;
                    if(direction.x == 0 && direction.y == 0)
                    {
                        status.state = PlayerState.Idle;
                    }
                    break;
            }

            direction = ChaseDest(direction, destX, destY);
                this.transform.position = new Vector3((float)(posX * 128), (float)(posY * -128), 1);
                classtimer = 128;
            }
            else
            {
                //distance分だけ移動
                this.transform.localPosition += new Vector3(direction.x, direction.y, 0) * speed;
                classtimer -= speed;
                if (classtimer <= 0)
                {
                    posX += (int)direction.x;
                    posY -= (int)direction.y;
                    classtimer = 0;
                }
            }

        status.posX = posX;
        status.posY = posY;
        status.speed = speed;
    }

    //壁を判定しつつ目的地に進んでみる
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

        //壁判定わからん、とりあえず０番が壁ってことしとこう
        if(map[posY - (int)direction.y, posX] == 0)
        {
            direction.y = 0.0f;
        }
        if (map[posY, posX + (int)direction.x] == 0)
        {
            direction.x = 0.0f;
        }
        if (map[posY - (int)direction.y, posX + (int)direction.x] == 0)
        {
            direction.x = 0.0f;
        }

        //現在地と目的地が一致したときは移動しない
        if (posX == destX && posY == destY)
        {
            direction = new Vector2(0.0f, 0.0f);
        }

        return direction;
    }
}
