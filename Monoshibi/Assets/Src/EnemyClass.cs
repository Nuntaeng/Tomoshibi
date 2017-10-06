using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の出現位置と距離
public enum PopPosition
{
    POP_RIGHT,
    POP_LEFT,
    POP_UP,
    POP_DOWN,
    POP_RANDOM,
    POP_FAR,
    POP_NEAR,
}





//エネミークラス、エネミーのパラメータと行動、感知を管理
public class EnemyClass : MonoBehaviour {
    //感知度、一定値detectMaxに達したらプレイヤーの近くにエネミーを召喚する
    [HideInInspector]
    public int detectCount;
    public int detectMax;
    public GameObject map;
    public AudioSource ChaseSound;
    public AudioSource DetectSound;
    public AudioSource BGM;
    public PlayerStatus status;
    public GameObject Enemy;
    [HideInInspector]
    public List<GameObject> enemyObj;

    //そもそも今プレイヤーの周りにいるかという判定
    public bool enable;
    //エネミーの出現する場所
    public PopPosition PopPosX;
    public PopPosition PopPosY;
    public int EnemySpeedLevel = 0;
    System.Random rand = new System.Random();

    //感知度最大がしばらく続いたら出現、と仕様変更
    public int ChasePoint;
    public int ChaseMaxPoint;




    private void Start()
    {
        detectCount = 1;
        DetectSound.volume = 0;
        ChaseSound.volume = 0;
        enemyObj = new List<GameObject>();
    }

    private void Update()
    {
        detectCount = status.detectCount;
        //エネミーが有効で、プレイヤーが隠れていないときは、徐々に感知度が上昇
        if (!enable || detectMax == 0 || status.state == PlayerState.Idle)
        {
            status.detectPoint = 0.0f;
            return;
        }

        //感知度が最低まで下がったら敵を全部消す
        if (detectCount <= 0)
        {
            KillEnemy();
        }
        ++detectCount;

        //だんだん感知されていって、基準値を超えると敵出現のカウントが作動する
        if (detectCount >= detectMax && detectMax != 0)
        {
            ++ChasePoint;
        } else if (detectCount <= 10)
        {
            //感知されてないときは敵出現のカウントが減少
            ChasePoint -= 10;
            if(ChasePoint < 0)
            {
                ChasePoint = 0;
            }
        }

        //感知度が規定数を超えるとなんか・・・こう・・・どばああっ！って感じ・・・？
        if (ChasePoint > ChaseMaxPoint)
        {
            ChasePoint = ChaseMaxPoint;
            detectCount = detectMax - 200;
            PopEnemy();
        }

        //感知度の割合にそってなんかこわい音が大きくなったりBGMがちっちゃくなったり
        float detectPoint = ((float)detectCount / (float)detectMax - 0.5f) * 2.0f;
        status.detectPoint = detectPoint;
        status.detectCount = detectCount;



        //プレイヤーが死んでいるときは音に干渉しない
        if(status.state == PlayerState.Dead)
        {
            return;
        }

        if (detectPoint > 0)
        {
            DetectSound.volume = detectPoint;
        }
        else
        {
            DetectSound.volume = 0;
        }
        //敵出現時は音量の変化をなくす
        if(ChasePoint == ChaseMaxPoint)
        {
            DetectSound.volume = 1.0f;
        }
        ChaseSound.volume = ((float)ChasePoint / (float)ChaseMaxPoint) * 0.3f;
    }

    //事前にセットされた召喚情報に従ってエネミーを生成
    void PopEnemy()
    {
        int popDirX = 0;
        int popDirY = 0;

        switch (PopPosX)
        {
            case PopPosition.POP_RIGHT:
                {
                    popDirX = 1;
                    break;
                }
            case PopPosition.POP_LEFT:
                {
                    popDirX = -1;
                    break;
                }
            case PopPosition.POP_RANDOM:
                {
                    popDirX = rand.Next(2) * 2 - 1;
                    break;
                }
        }
        switch (PopPosY)
        {
            case PopPosition.POP_UP:
                {
                    popDirY = -1;
                    break;
                }
            case PopPosition.POP_DOWN:
                {
                    popDirY = 1;
                    break;
                }
            case PopPosition.POP_RANDOM:
                {
                    popDirY = rand.Next(2) * 2 - 1;
                    break;
                }
        }

        int x = status.posX + popDirX * (rand.Next(1, 3));
        int y = status.posY + popDirY * (rand.Next(1, 3));
        int speed = rand.Next(EnemySpeedLevel + 1, EnemySpeedLevel + 4);
        //エネミーを生成
        CreateEnemy(x, y, speed);
    }

    //エネミーを召喚
    void CreateEnemy(int x, int y, int speed)
    {
        GameObject clone;
        EnemyObjectClass enemyObjClass;
        NPCStatus enemyStatus;

        clone = Instantiate(Enemy, new Vector3(x * 128.0f, y * -128.0f, y), Quaternion.identity, this.transform);
        //フェードイン
        StartCoroutine(FadeInEnemy(30, clone));

        enemyObjClass = clone.GetComponent<EnemyObjectClass>();
        enemyObjClass.posX = x;
        enemyObjClass.posY = y;
        enemyObjClass.map = map;
        enemyObjClass.playerStatus = status;
        enemyObjClass.Initialize();
        enemyStatus = clone.GetComponent<NPCStatus>();
        enemyStatus.speed = speed;
        clone.SetActive(true);
        clone.GetComponent<Collider2D>().enabled = false;

        enemyObj.Add(clone);
    }

    //エネミーを消去
    public void KillEnemy()
    {
        for(int i = 0; i < enemyObj.Count; ++i)
        {
            if (enemyObj[i] == null || enemyObj[i].GetComponent<EnemyObjectClass>().killEnable)
            {
                continue;
            }
            StartCoroutine(FadeOutEnemy(60, enemyObj[i]));
            enemyObj[i].GetComponent<EnemyObjectClass>().killEnable = true;
        }

    }

    IEnumerator FadeOutEnemy(int time, GameObject enemy)
    {
        SpriteRenderer renderer = enemy.gameObject.GetComponent<SpriteRenderer>();


            for (int i = 1; i <= time; ++i)
            {
            if (renderer != null)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1.0f - ((float)i / (float)time));
                yield return null;
            }
        }
        //フェードアウト後にオブジェクトを消去
        Destroy(enemy);
        enemyObj.Clear();
    }

    IEnumerator FadeInEnemy(int time, GameObject enemy)
    {
        SpriteRenderer renderer = enemy.gameObject.GetComponent<SpriteRenderer>();
        for (int i = 1; i <= time; ++i)
        {
            if (renderer != null)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b,((float)i / (float)time));
                yield return null;
            }
        }
        //フェードイン後にあたり判定出現
        enemy.GetComponent<Collider2D>().enabled = true;
    }
}
