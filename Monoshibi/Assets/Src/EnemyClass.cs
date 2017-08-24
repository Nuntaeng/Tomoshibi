using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の出現位置と距離
enum PopPosition
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
    public int detectCount;
    public int detectMax;
    public GameObject map;
    public AudioSource ChaseSound;
    public AudioSource DetectSound;
    public AudioSource BGM;
    public PlayerStatus status;
    public List<GameObject> enemyObj;

    //そもそも今プレイヤーの周りにいるかという判定
    public bool enable;
    //エネミーの出現する場所
    PopPosition PopPosX = PopPosition.POP_RANDOM;
    PopPosition PopPosY = PopPosition.POP_RANDOM;
    int EnemySpeedLevel = 0;
    System.Random rand = new System.Random();



    private void Start()
    {
        detectCount = 0;
        DetectSound.volume = 0;
        enemyObj = new List<GameObject>();
    }

    private void Update()
    {
        detectCount = status.detectCount;
        //エネミーが有効で、プレイヤーが隠れていないときは、徐々に感知度が上昇
        if (!enable || detectMax == 0 || status.state != PlayerState.Active)
        {
            status.detectPoint = 0.0f;
            return;
        }

        ++detectCount;

        //感知度が規定数を超えるとなんか・・・こう・・・どばああっ！って感じ・・・？
        if (detectCount >= detectMax && detectMax != 0)
        {
            detectCount = 0;
            detectMax = 200;
            ChaseSound.volume = 1.0f;
            //CreateEnemy(status.posX - (int)status.direction.x * 5, status.posY + (int)status.direction.y * 5, 6);
            PopEnemy();
        }

        //感知度の割合にそってなんかこわい音が大きくなる
        float detectPoint = (float)detectCount / (float)detectMax;
        status.detectPoint = detectPoint;
        status.detectCount = detectCount;
        DetectSound.volume = detectPoint;
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
                    popDirY = 1;
                    break;
                }
            case PopPosition.POP_DOWN:
                {
                    popDirY = -1;
                    break;
                }
            case PopPosition.POP_RANDOM:
                {
                    popDirY = rand.Next(2) * 2 - 1;
                    break;
                }
        }

        CreateEnemy(status.posX + popDirX * (3 + rand.Next(3)), status.posY + popDirY * (3 + rand.Next(3)), rand.Next(EnemySpeedLevel * 3 + 2, EnemySpeedLevel * 3 + 5));
    }

    //エネミーを召喚
    void CreateEnemy(int x, int y, int speed)
    {
        GameObject clone;
        EnemyObjectClass enemyObjClass;
        NPCStatus enemyStatus;

        clone = Instantiate(Resources.Load<GameObject>("prehubs/Enemy"), new Vector3(x * 128.0f, y * -128.0f, y), Quaternion.identity, this.transform);
        
        enemyObjClass = clone.GetComponent<EnemyObjectClass>();
        enemyObjClass.posX = x;
        enemyObjClass.posY = y;
        enemyObjClass.map = map;
        enemyObjClass.status = status;
        enemyObjClass.Initialize();
        enemyStatus = clone.GetComponent<NPCStatus>();
        enemyStatus.speed = speed;
        clone.SetActive(true);
        
        enemyObj.Add(clone);
    }

    //エネミーを消去
    public void KillEnemy()
    {
        detectMax = 0;
        for(int i = 0; i < enemyObj.Count; ++i)
        {
            enemyObj[i].GetComponent<EnemyObjectClass>().KillEnemy();
        }
        ChaseSound.volume = 0.0f;
        DetectSound.volume = 0.0f;
        BGM.volume = 1.0f;
    }
}
