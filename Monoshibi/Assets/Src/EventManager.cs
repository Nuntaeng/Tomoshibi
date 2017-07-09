using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EventIndex
{
    //条件
    EVENT_IF_POSITION,
    EVENT_IF_FLAG,
    EVENT_IF_COUNT,
    EVENT_IF_CHECK,

    //処理
    EVENT_FUNC_PLAYERMOVE,
    EVENT_FUNC_PLAYERCENTER,
    EVENT_FUNC_WAIT,
    EVENT_FUNC_SETFLAG,
    EVENT_FUNC_CHANGEIMAGE,
    EVENT_FUNC_SETTEXT,
    EVENT_FUNC_SETTEXTDESTNATION,
    EVENT_FUNC_SETLIGHT,
    EVENT_FUNC_SETPLAYERLIGHT,
    EVENT_FUNC_PLAYSOUNDEFFECT,
    EVENT_FUNC_PLAY3DSOUNDEFFECT,
}

[System.Serializable]
public class EventStatus
{
    //イベントのテキストファイル
    public TextAsset eventTxtFile;
    //イベントの条件と処理内容
    string[] eventCheck;
    string[] eventText;

    //さすがに毎フレーム文字列を数値に変換し続けるのも忍びないので数値として保管、判定にはこっちを使う
    int[][] checkData;

    //イベントが発動中かを判定（falseの時にイベントが発生してないということにする）
    public bool eventState = false;

    //イベントが起こった回数
    int eventFlag = 0;

    //イベントの進行度
    int eventCount = 0;
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

    //外部のオブジェクト
    public GameObject player;
    public GameObject map;
    public GameObject lightManager;
    public Speaker speaker;
    public Text textWindowBottom;
    public Text textWindowCenter;
    public Text textWindowTop;

    [HeaderAttribute("Event")]
    public EventStatus[] eventStatus;
    

    //文字列で数値を呼び出すためのあれこれ
    public Dictionary<string, int> keyWord = new Dictionary<string, int>();

    
    void Start()
    {
        SetDictionary();

        func = this.GetComponent<EventFunctions>();
        flag = this.GetComponent<FlagClass>();
        status = player.GetComponent<PlayerStatus>();
        mapObj = map.GetComponent<MapScript>().GetMapObject();
        lightObj = lightManager.GetComponent<LightManager>();


        //必要な情報を取得してクラスを実体化
        for (int i = 0; i < eventStatus.Length; ++i)
        {
            if (eventStatus[i].eventTxtFile == null)
            {
                continue;
            }
            GameObject newobj = new GameObject();
            newobj.AddComponent<EventClass>();
            newobj.transform.parent = this.transform;
            EventClass eventClass = newobj.GetComponent<EventClass>();
            eventClass.manager = this.gameObject.GetComponent<EventManager>();
            eventClass.eventTxtFile = eventStatus[i].eventTxtFile;
            eventClass.eventNumber = i;
            eventClass.Initialize();
        }
    }

    //発動中のイベントが存在したらプレイヤーは操作不能のIdle状態へ移行する
    private void FixedUpdate()
    {
        for (int i = 0; i < eventStatus.Length; ++i)
        {
            if (eventStatus[i].eventState)
            {
                status.state = PlayerState.Idle;
                return;
            }
            status.state = PlayerState.Active;
        }
    }


    void SetDictionary()
    {
        //数字と文字列の紐付け
        //条件
        keyWord.Add("position", (int)EventIndex.EVENT_IF_POSITION);
        keyWord.Add("flag", (int)EventIndex.EVENT_IF_FLAG);
        keyWord.Add("once", (int)EventIndex.EVENT_IF_COUNT);
        keyWord.Add("check", (int)EventIndex.EVENT_IF_CHECK);

        //処理
        keyWord.Add("playerMove", (int)EventIndex.EVENT_FUNC_PLAYERMOVE);
        keyWord.Add("playerCenter", (int)EventIndex.EVENT_FUNC_PLAYERCENTER);
        keyWord.Add("petFlag", (int)EventIndex.EVENT_FUNC_SETFLAG);
        keyWord.Add("wait", (int)EventIndex.EVENT_FUNC_WAIT);
        keyWord.Add("changeImage", (int)EventIndex.EVENT_FUNC_CHANGEIMAGE);
        keyWord.Add("setText", (int)EventIndex.EVENT_FUNC_SETTEXT);
        keyWord.Add("setTextDest", (int)EventIndex.EVENT_FUNC_SETTEXTDESTNATION);
        keyWord.Add("setLight", (int)EventIndex.EVENT_FUNC_SETLIGHT);
        keyWord.Add("setPlayerLight", (int)EventIndex.EVENT_FUNC_SETPLAYERLIGHT);
        keyWord.Add("playSE", (int)EventIndex.EVENT_FUNC_PLAYSOUNDEFFECT);
        keyWord.Add("play3DSE", (int)EventIndex.EVENT_FUNC_PLAY3DSOUNDEFFECT);
    }
}
