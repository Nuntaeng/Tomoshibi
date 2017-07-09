using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
    文字列の記法について
    ・とりあえず必ず「条件 → 処理」の順で書く
    ・それぞれの数を示すsizeの直後の条件や処理の部分以外は別に厳密には決まってない
    ・どこぞの某言語のように分の終わりにはセミコロンを入れる
    ・sizeの文字列で文字列を切り分けるために"："に一回文字列を置き換えてるので"："は書かないほうがたぶんいい
    ・座標指定とか指定は残念ながら画面上でクリックして座標調べるとかはできません、地道に調べましょう・・・・・・すまぬ、すまぬ

    ・この手の処理にはド定番の「はい/いいえ」の分岐はまだない、考え中
    ・まだ音と敵はいじれません、はい


    条件：

    size 1                                  サイズ指定は一番初めに書く、この直後の文を配列にぶち込んでいるので変に改行しないこと
    position, x, y                          プレイヤーの座標が指定した位置にある（引数は座標XY）
    flag, index, 0or1                       特定のフラグが立っているとき/立っていないとき (引数はフラグの番号、boolの真偽で０をfalseとし、１をtrueとする)
    once, count                             このイベントの起こった回数が一定回数以下のとき（基本的には０を指定して一回だけイベントを起こすように使う）
    check, x, y                             指定した座標のマップオブジェクトを調べたとき(あたり判定のないものは今は調べられない);
 

    処理：

    size 2                                                          サイズ指定は一番初めに書く、この直後の文を配列にぶち込んでいるので変に改行しないこと
    setFlag, setIndex, set                                          指定したフラグを立てたり折ったりする、０がfalseで１がtrue
    playerMove, direction, distance, speed                          「プレイヤー」を指定方向へ移動させる（directionはup,down,left,rightの四つ）
    playerCenter                                                    引数なし、プレイヤーをマスの中心へ移動させる
    wait, time                                                      指定したフレーム数イベント処理を待つ
    changeImage, x, y, name                                         指定したマップ座標のオブジェクトをnameのプレハブと入れ替える
   *setTextDest, position                                           テキストの表示位置の指定、同一イベント内でのみ有効（位置はtop,center,bottomの三つ）
    setText, speed, string                                          setTextDist処理で指定した位置にstringの文字列をwaitをかけつつ表示、位置の初期値は下
    setLight, index, x, y, size, R1, G1, B1, A1, R2, G2, B2, A1     index番目の光の、位置、色、サイズを変更、色は透明度含む二色
    setPlayerLight, enable                                          ほとんどオープニング用、ろうそくをつけたり消したりする
    playSE, name                                                    効果音を鳴らす。
    Play3DSE, x, y, name                                            ３D効果音を鳴らす。音源位置調整もできる
    
    end:                                                            テキストの末尾には"end"と書く、ぶっちゃけ仕様上いらないけど見やすいと思うので

    ※随時追加予定、忘れてたら言ってちょ
    　新規追加処理には今度から*をつけとく、今後の参考にでもしておくれなんし
 */






//イベントの書かれたテキストの入れ物クラス
[System.Serializable]
public class EventClass : MonoBehaviour {

    //外部のクラス
    public EventManager manager;
    public TextAsset eventTxtFile;


    //このイベントがマネージャーの管理する何番目のイベントかを保持、主にマネージャーに情報を返すために使う
    [HideInInspector]
    public int eventNumber;


    //イベントの条件と処理内容
    string[] eventCheck;
    string[] eventText;
    
    //さすがに毎フレーム文字列を数値に変換し続けるのも忍びないので数値として保管、判定にはこっちを使う
    int[][] checkData;

    //イベントが発動中かを判定（falseの時にイベントが発生してないということにする）
    bool eventState = false;

    //イベントが起こった回数
    int eventFlag = 0;

    //イベントの進行度
    int eventCount = 0;

    //文字の表示位置
    Text textDest;


    
    public void Initialize()
    {
        //イベント情報を読み取り
        ReadEventText(ref eventCheck, ref eventText);
        SetCheckData();
        textDest = manager.textWindowBottom;
    }

    void Update()
    {
        //ずっと判定垂れ流すって実際どうなんでしょう、処理的に
        if ((eventCount > 0 || CheckEventEnter()) && !eventState && eventText != null && eventText.Length > 0)
        {
            //アニメーションを止める（正確には待機アニメーションへ移行）
            Animator animator = manager.player.GetComponent<Animator>();
            animator.SetFloat("Player_Input_Y", 0.0f);
            animator.SetFloat("Player_Input_X", 0.0f);

            textDest.text = "";


            StartEvent(eventCount);

            eventCount++;
            if (eventCount >= eventText.Length)
            {
                eventCount = 0;
                ++eventFlag;
            }
        }
        manager.eventStatus[eventNumber].eventState = eventState;
    }


    //アタッチされたテキストファイルからイベント情報を読み取る
    void ReadEventText(ref string[] condition, ref string[] eventData)
    {
        string text = eventTxtFile.text;
        //Splitが文字列に対応してないのでsizeをコロンに置換してそれで区切る、そのためまずはファイル内のコロンをエラー防止として除去しておく
        text = text.Replace(":", "");
        //余計な文字列を除去
        text = text.Replace("\r", "").Replace("\n", "");
        text = text.Replace(" ", "");
        text = text.Replace("　", "");
        text = text.Replace("size", ":");

        string[] array = text.Split(':');

        //一つ目が条件
        string[] str = array[1].Split(';');

        condition = new string[int.Parse(str[0])];
        for(int i = 0; i < int.Parse(str[0]); ++i)
        {
            condition[i] = str[i + 1];
        }
        //二つ目が処理
        str = array[2].Split(';');

        eventData = new string[int.Parse(str[0])];
        for (int i = 0; i < int.Parse(str[0]); ++i)
        {
            eventData[i] = str[i + 1];
        }
    }


    //判定用に文字列を数値にあらかじめ変換しておく
    void SetCheckData()
    {
        //条件文字列の読み取りと格納
        int numCheck = eventCheck.Length;
        checkData = new int[numCheck][];

        //処理の関係で調べるコマンドの発生するcheck条件は配列の最初へ入れ替え
        for (int i = numCheck - 1; i > 0; --i)
        {
            if(eventCheck[i].Substring(0, 5) == "check")
            {
                string c;
                c = eventCheck[i];
                eventCheck[i] = eventCheck[0];
                eventCheck[0] = c;
                break;
            }
        }
        for (int i = 0; i < numCheck; ++i)
        {
            checkData[i] = new int[3];
            string[] split = eventCheck[i].Split(',');
            //一個目は文字列によるキーなのが確定なのでDictionaryから取得
            checkData[i][0] = manager.keyWord[split[0]];

            //二個目以降は引数なので数値へ変換（とりあえずint以外は扱わないこととする）
            for (int k = 1; k < split.Length; ++k)
            {
                checkData[i][k] = int.Parse(split[k]);
            }
        }
    }

    //条件の判定
    bool CheckEventEnter()
    {
        //調べられる場所の位置は記憶しておく(まどろっこしいので注意、調べることができて、調べる以外の条件を満たすときは調べるってメッセージを出したいがためにやってる)
        int checkX = -1;
        int checkY = -1;

        for (int i = 0; i < checkData.Length; ++i)
        {

            //インデックスによって処理を判断
            switch ((EventIndex)checkData[i][0])
            {
                //プレイヤーの位置を判定
                case EventIndex.EVENT_IF_POSITION:
                    if(checkData[i][1] != manager.status.posX || checkData[i][2] != manager.status.posY)
                    {
                        return false;
                    }
                    break;

                //指定したフラグが立っているか/立っていないか
                case EventIndex.EVENT_IF_FLAG:
                    if ((manager.flag.flag[checkData[i][1]] && checkData[i][2] == 0) || (!manager.flag.flag[checkData[i][1]] && checkData[i][2] == 1))
                    {
                        return false;
                    }
                    break;

                //このイベントが起こった回数が指定値以下
                case EventIndex.EVENT_IF_COUNT:
                    if (eventFlag > checkData[i][1])
                    {
                        return false;
                    }
                    break;

                //特定の座標にあるマップオブジェクトを調べたとき
                case EventIndex.EVENT_IF_CHECK:
                    //調べるコマンドの発生するマップチップのタグ初期化
                    checkX = checkData[i][1];
                    checkY = checkData[i][2];
                    manager.mapObj[checkY, checkX].tag = "Untagged";
                    break;

                //文字列がデータにないやつだったらとりあえずfalse返しとく
                default:
                    return false;
            }
        }

        //調べるコマンドが存在して、かつ調べられる条件を満たすときにそのマスのタグをMapobjectへ
        if(checkX != -1 && checkY != -1)
        {

            manager.mapObj[checkY, checkX].tag = "MapObject";

            if (checkX != (int)manager.status.checkPos.x || checkY != (int)manager.status.checkPos.y)
            {
                return false;
            }
            manager.mapObj[checkY, checkX].tag = "Untagged";
            manager.status.checkPos = new Vector2(-100.0f, -100.0f);
        }

        return true;
    }

    //イベントの実行
    void StartEvent(int i)
    {
        string[] text = eventText[i].Split(',');
        int index = manager.keyWord[text[0]];
        //番号にて関数を呼び出す
        switch ((EventIndex)index)
        {
            //"playerMove"処理
            case EventIndex.EVENT_FUNC_PLAYERMOVE:
                {
                    int x = 0, y = 0;
                    if(text[1] == "up")
                    {
                        y = -1;
                    }
                    else if (text[1] == "down")
                    {
                        y = 1;
                    }
                    else if (text[1] == "left")
                    {
                        x = -1;
                    }
                    else if (text[1] == "right")
                    {
                        x = 1;
                    }

                    int distance = int.Parse(text[2]);
                    int speed = int.Parse(text[3]);

                    StartCoroutine(manager.func.PlayerMove((r) => eventState = r, manager.player, new Vector2(x, y), distance, speed));
                    break;
                }
            //"playerCenter"処理
            case EventIndex.EVENT_FUNC_PLAYERCENTER:
                {
                    StartCoroutine(manager.func.PlayerCenterMove((r) => eventState = r, manager.player));
                    break;
                }

            //"setFlag"処理
            case EventIndex.EVENT_FUNC_SETFLAG:
                {
                    int setIndex = int.Parse(text[1]);
                    int set = int.Parse(text[2]);
                    if (set == 0)
                    {
                        manager.flag.flag[setIndex] = false;
                    }
                    else
                    {
                        manager.flag.flag[setIndex] = true;
                    }
                    break;
                }

            //"wait"処理
            case EventIndex.EVENT_FUNC_WAIT:
                {
                    int wait = int.Parse(text[1]);
                    StartCoroutine(manager.func.Wait((r) => eventState = r, wait));
                    break;
                }

            //"changeImage"処理
            case EventIndex.EVENT_FUNC_CHANGEIMAGE:
                {
                    int x = int.Parse(text[1]);
                    int y = int.Parse(text[2]);

                    manager.mapObj[y, x] = manager.func.ChangeImage(manager.mapObj[y, x], text[3]);

                    break;
                }

            //"setText"処理
            case EventIndex.EVENT_FUNC_SETTEXT:
                {
                    int wait = int.Parse(text[1]);
                    Debug.Log(text[2]);
                    StartCoroutine(manager.func.SetTextWindow((r) => eventState = r, wait, text[2], textDest, manager.player));
                    break;
                }

            //"setTextDest"処理
            case EventIndex.EVENT_FUNC_SETTEXTDESTNATION:
                {
                    if (text[1] == "top")
                    {
                        textDest = manager.textWindowTop;
                    }
                    if (text[1] == "center")
                    {
                        textDest = manager.textWindowCenter;
                    }
                    if (text[1] == "bottom")
                    {
                        textDest = manager.textWindowBottom;
                    }
                    break;
                }
            //"setLight"処理
            case EventIndex.EVENT_FUNC_SETLIGHT:
                {
                    int no = int.Parse(text[1]);
                    int x = int.Parse(text[2]);
                    int y = int.Parse(text[3]);
                    float size = float.Parse(text[4]);
                    Color color1 = new Color(float.Parse(text[5]), float.Parse(text[6]), float.Parse(text[7]), float.Parse(text[8]));
                    Color color2 = new Color(float.Parse(text[9]), float.Parse(text[10]), float.Parse(text[11]), float.Parse(text[12]));
                    Debug.Log(color1);

                    manager.func.SetLight(manager.lightObj, no, x, y, color1, color2, size);
                    break;
                }
            //"setPlayerLight"処理
            case EventIndex.EVENT_FUNC_SETPLAYERLIGHT:
                {
                    int enable = int.Parse(text[1]);
                    manager.func.SetPlayerLight(enable, manager.player);
                    break;
                }
            //"playSE"処理
            case EventIndex.EVENT_FUNC_PLAYSOUNDEFFECT:
                {
                    Debug.Log(text[1]);
                    SEManager.Instance.PlaySE(text[1]);
                    break;
                }
            //"play3DSE"処理
            case EventIndex.EVENT_FUNC_PLAY3DSOUNDEFFECT:
                {
                    Vector2 pos = new Vector3(float.Parse(text[1]), float.Parse(text[2]));
                    manager.func.Play3DSound(pos, text[3]);
                    break;
                }

            default:
                return;
        }

    }


}




