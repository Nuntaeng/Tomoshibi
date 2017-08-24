using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EventIndex
{
    //条件
    EVENT_IF_IDLE,
    EVENT_IF_POSITION,
    EVENT_IF_FLAG,
    EVENT_IF_COUNT,
    EVENT_IF_CHECK,
    EVENT_IF_ITEM,

    //処理
    EVENT_FUNC_PLAYERMOVE,
    EVENT_FUNC_PLAYERSETPOSITION,
    EVENT_FUNC_PLAYERCENTER,
    EVENT_FUNC_WAIT,
    EVENT_FUNC_SETFLAG,
    EVENT_FUNC_CHANGEIMAGE,
    EVENT_FUNC_SETTEXT,
    EVENT_FUNC_SETTEXTCOLOR,
    EVENT_FUNC_SETTEXTDESTNATION,
    EVENT_FUNC_SETLIGHT,
    EVENT_FUNC_SETPLAYERLIGHT,
    EVENT_FUNC_PLAYSOUNDEFFECT,
    EVENT_FUNC_PLAY3DSOUNDEFFECT,
    EVENT_FUNC_ITEMGET,
    EVENT_FUNC_ITEMLOST,
    EVENT_FUNC_ENEMYDETECTED,
    EVENT_FUNC_ENEMYSETMAXDETECT,
    EVENT_FUNC_FADEIN,
    EVENT_FUNC_FADEOUT,
    EVENT_FUNC_CHANGEMAP,
    EVENT_FUNC_BGMSTART,
    EVENT_FUNC_BGMSTOP,
    EVENT_FUNC_BGMCHANGE,
}




[System.Serializable]
public class EventStatus
{
    //イベントのテキストファイル
    public TextAsset eventTxtFile;

    //イベントクラスの配列
    [HideInInspector]
    public EventClass eventClass;

    //イベントの条件と処理内容
    string[] eventCheck;
    string[] eventText;

    //さすがに毎フレーム文字列を数値に変換し続けるのも忍びないので数値として保管、判定にはこっちを使う
    int[][] checkData;

    //イベントが発動中かを判定（falseの時にイベントが発生してないということにする）
    public bool eventState = false;
    //デバッグ用、falseになっているイベントは発動しなくなる。もしかしたらデバッグ以外にも使えるかも
    public bool eventEnable = true;
    //イベントがプレイヤーを止めるものかどうかのフラグ
    [HideInInspector]
    public bool idleEvent;

    //イベントが起こった回数
    int eventFlag = 0;

    //イベントの進行度
    int eventCount = 0;
}

[System.Serializable]
public class EventStatusArray
{
    [HeaderAttribute("Event")]
    //ジャグ配列をInspector上に表示するためにやむなくこんな書き方をしております
    public EventStatus[] eventStatus;
}

//イベント管理クラス、イベントの実行クラスに必要な道具を貸してあげる役
public class EventManager : MonoBehaviour {
    //外部のクラスたち
    [HideInInspector]
    public EventFunctions func;
    [HideInInspector]
    public FlagClass flag;
    [HideInInspector]
    public PlayerStatus status;
    [HideInInspector]
    public LightManager lightObj;
    [HideInInspector]
    public GameObject[,] mapObj;
    [HideInInspector]
    public GameObject[] itemData;
    [HideInInspector]
    public EnemyClass enemyClass;
    [HideInInspector]
    public FadeScript fade;
    //外部のオブジェクト
    public GameObject player;
    public GameObject map;
    public GameObject lightManager;
    public GameObject itemmanager;
    public GameObject itembox;
    public GameObject enemyManager;
    public Speaker speaker;
    public Text textWindowBottom;
    public Text textWindowCenter;
    public Text textWindowTop;
    public GameObject fadeMask;
    public MapScript mapLoader;
    public SoundContainer BGM;
    public SoundContainer DetectSE;
    public SoundContainer ChaseSE;


    [HeaderAttribute("EventArray")]
    //マップごとにイベントを分けるために数値でどのイベント配列を使うかを指定する
    [HideInInspector]
    public int useEventIndex;
    public EventStatusArray[] eventArray;
    [HideInInspector]
    public EventStatus[] eventStatus;
    

    //文字列で数値を呼び出すためのあれこれ
    public Dictionary<string, int> keyWord = new Dictionary<string, int>();

    //全体でイベントが起こっているかどうか。基本的にイベントの重複発生を防ぐためのもの
    [HideInInspector]
    public int eventState = -1;
    
    void Start()
    {
        SetDictionary();

        func = this.GetComponent<EventFunctions>();
        flag = this.GetComponent<FlagClass>();
        status = player.GetComponent<PlayerStatus>();
        lightObj = lightManager.GetComponent<LightManager>();
        enemyClass = enemyManager.GetComponent<EnemyClass>();
        fade = fadeMask.GetComponent<FadeScript>();
        //mapLoader = map.GetComponent<MapScript>();

        useEventIndex = map.GetComponent<MapScriptTMX>().mapNumber;
        EventInitialize();
    }

    public void EventInitialize()
    {
        for(int i = 0; i < this.transform.childCount; ++i)
        {
            //子を破棄してからイベントを生成
            Destroy(this.transform.GetChild(i).gameObject);
        }
        //今回使用するイベント配列をセット
        eventStatus = eventArray[useEventIndex].eventStatus;

        //マップを獲得
        mapObj = map.GetComponent<MapScript>().GetMapObject();
        

        //必要な情報を取得してクラスを実体化
        for (int i = 0; i < eventStatus.Length; ++i)
        {
            if (eventStatus[i].eventTxtFile == null)
            {
                continue;
            }
            //イベントのデフォルトはプレイヤーを止める
            eventStatus[i].idleEvent = true;

            //オブジェクトにくっついてないとイベントクラスが存在できないのでからオブジェクトを生成してアタッチ
            GameObject newobj = new GameObject();
            newobj.AddComponent<EventClass>();
            newobj.transform.parent = this.transform;
            EventClass eventClass = newobj.GetComponent<EventClass>();
            eventClass.manager = this.gameObject.GetComponent<EventManager>();
            eventClass.eventTxtFile = eventStatus[i].eventTxtFile;
            eventClass.eventNumber = i;
            eventClass.Initialize();

            eventStatus[i].eventClass = eventClass;

        }
    }

    //発動中のイベントが存在したらプレイヤーは操作不能のIdle状態へ移行する
    private void FixedUpdate()
    {
        if(status.state == PlayerState.Dead)
        {
            return;
        }
        for (int i = 0; i < eventStatus.Length; ++i)
        {
            //イベントの更新処理を実行
            eventStatus[i].eventClass.Run();
            //ループ中にマップ切り替えがあるとout of range出すのでその判定
            if (eventStatus.Length <= i)
            {
                continue;
            }
            //イベント状態を判定
            if (eventStatus[i].eventState && eventStatus[i].idleEvent)
            {
                status.state = PlayerState.Idle;
                eventState = i;
                return;
            }
        }

        status.state = PlayerState.Active;
        eventState = -1;
    }


    void SetDictionary()
    {
        //数字と文字列の紐付け
        //条件
        keyWord.Add("nonIdle", (int)EventIndex.EVENT_IF_IDLE);
        keyWord.Add("position", (int)EventIndex.EVENT_IF_POSITION);
        keyWord.Add("flag", (int)EventIndex.EVENT_IF_FLAG);
        keyWord.Add("once", (int)EventIndex.EVENT_IF_COUNT);
        keyWord.Add("check", (int)EventIndex.EVENT_IF_CHECK);
        keyWord.Add("item", (int)EventIndex.EVENT_IF_ITEM);

        //処理
        keyWord.Add("playerMove", (int)EventIndex.EVENT_FUNC_PLAYERMOVE);
        keyWord.Add("playerCenter", (int)EventIndex.EVENT_FUNC_PLAYERCENTER);
        keyWord.Add("setPlayerPos", (int)EventIndex.EVENT_FUNC_PLAYERSETPOSITION);
        keyWord.Add("setFlag", (int)EventIndex.EVENT_FUNC_SETFLAG);
        keyWord.Add("wait", (int)EventIndex.EVENT_FUNC_WAIT);
        keyWord.Add("changeImage", (int)EventIndex.EVENT_FUNC_CHANGEIMAGE);
        keyWord.Add("setText", (int)EventIndex.EVENT_FUNC_SETTEXT);
        keyWord.Add("setTextDest", (int)EventIndex.EVENT_FUNC_SETTEXTDESTNATION);
        keyWord.Add("setTextColor", (int)EventIndex.EVENT_FUNC_SETTEXTCOLOR);
        keyWord.Add("setLight", (int)EventIndex.EVENT_FUNC_SETLIGHT);
        keyWord.Add("setPlayerLight", (int)EventIndex.EVENT_FUNC_SETPLAYERLIGHT);
        keyWord.Add("playSE", (int)EventIndex.EVENT_FUNC_PLAYSOUNDEFFECT);
        keyWord.Add("play3DSE", (int)EventIndex.EVENT_FUNC_PLAY3DSOUNDEFFECT);
        keyWord.Add("itemget", (int)EventIndex.EVENT_FUNC_ITEMGET);
        keyWord.Add("itemlost", (int)EventIndex.EVENT_FUNC_ITEMLOST);
        keyWord.Add("createEnemy", (int)EventIndex.EVENT_FUNC_ENEMYDETECTED);
        keyWord.Add("setEnemyMaxDetect", (int)EventIndex.EVENT_FUNC_ENEMYSETMAXDETECT);
        keyWord.Add("fadeIn", (int)EventIndex.EVENT_FUNC_FADEIN);
        keyWord.Add("fadeOut", (int)EventIndex.EVENT_FUNC_FADEOUT);
        keyWord.Add("ChangeMap", (int)EventIndex.EVENT_FUNC_CHANGEMAP);
        keyWord.Add("BGMStart", (int)EventIndex.EVENT_FUNC_BGMSTART);
        keyWord.Add("BGMStop", (int)EventIndex.EVENT_FUNC_BGMSTOP);
        keyWord.Add("BGMChange", (int)EventIndex.EVENT_FUNC_BGMCHANGE);
    }
}
