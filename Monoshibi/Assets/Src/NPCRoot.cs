using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//指定した軌道配列に沿って動く敵挙動、エネミーって書いてるけどまだコメント編集してないからだから気にしなくてよろし
public class NPCRoot : MonoBehaviour {
    //親に位置するエネミー管理クラス
    NPCClass enemyClass;

    //ステータス
    NPCStatus status;

    //初期地点を０とするマップ座標
    int originalX, originalY;
    int maxX, maxY;

    //次に進む方向
    Vector2 direction;
    //クラス内での経過時間を保存しておく
    public int classtimer;


    //マップ情報
    MapScript mapClass;
    int[,] map;

    //ルート情報
    NPCRootData rootData;
    public int rootId;
    int[,] root;




    void Start()
    {
        //エネミーステータスの格納
        enemyClass = this.transform.parent.GetComponent<NPCClass>();
        status = this.GetComponent<NPCStatus>();
        status.posX = enemyClass.NPC[status.index].posX;
        status.posY = enemyClass.NPC[status.index].posY;
        status.speed = enemyClass.NPC[status.index].speed;
        status.state = PlayerState.Idle;
        this.transform.position = new Vector3((float)(status.posX * 128), (float)(status.posY * -128), 1);

        //クラスカウンタを初期化してマップデータを取得
        classtimer = 0;
        direction = new Vector2(1, 0);
        mapClass = enemyClass.mapObj.GetComponent<MapScript>();
        map = mapClass.GetMap();
        maxX = map.GetLength(1) - 1;
        maxY = map.GetLength(0) - 1;

        //ルート情報の読み込み
        rootData = this.GetComponent<NPCRootData>();
        root = rootData.GetRoot(rootId);
        maxX = root.GetLength(1) - 1;
        maxY = root.GetLength(0) - 1;
        originalX = status.posX;
        originalY = status.posY;
    }

    void Update()
    {
        int posX = status.posX;
        int posY = status.posY;

        if (classtimer <= 0)
        {
            //移動が終わったら上下左右のマップデータを判定
             direction = CheckRoot(posX - originalX, posY - originalY, direction);

            this.transform.position = new Vector3((float)(posX * 128), (float)(posY * -128), 1);
            classtimer = 128;
        }
        else
        {
            int speed = status.speed;

            //distance分だけ移動
            this.transform.localPosition += new Vector3(direction.x, direction.y, 0) * speed;
            classtimer -= speed;
            if (classtimer <= 0)
            {
                posX += (int)direction.x;
                posY -= (int)direction.y;

            }
        }
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y / 128.0f);

        status.posX = posX;
        status.posY = posY;
    }

    //ルートデータを調べて移動方向を返す、引数は配列単位での座標と現在の移動向き
    Vector2 CheckRoot(int x, int y, Vector2 NowDirection)
    {
        int up = 0;
        int down = 0;
        int left = 0;
        int right = 0;

        if (y > 0) { up = root[y - 1, x]; }
        if (y < maxY) { down = root[y + 1, x]; }
        if (x > 0) { left = root[y, x - 1]; }
        if (x < maxX) { right = root[y, x + 1]; }


        //方向初期化
        Vector2 direct = new Vector2(0f, 0f);
        //反対方向の判定は取らない、前回と同じ方向に道があったらそちらを優先
            if (left == 1 && NowDirection.x != 1f)
            {
                direct = new Vector2(-1f, 0f);
                if(NowDirection == direct)
                    return direct;
            }
            if (right == 1 && NowDirection.x != -1f)
            {
                direct = new Vector2(1f, 0f);
                if (NowDirection == direct)
                    return direct;
            }
            if (up == 1 && NowDirection.y != -1f)
            {
                direct = new Vector2(0f, 1f);
                if (NowDirection == direct)
                    return direct;
            }
            if (down == 1 && NowDirection.y != 1f)
            {
                direct = new Vector2(0f, -1f);
                if (NowDirection == direct)
                    return direct;
            }
        return direct;
    }

}
