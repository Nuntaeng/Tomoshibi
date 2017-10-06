using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
    文字列の記法について
    ・とりあえず必ず「条件 → 処理」の順で書く
    ・それぞれの数を示すsizeの直後の条件や処理の部分以外は別に厳密には決まってない
    ・どこぞの某言語のように分の終わりにはセミコロンを入れる
    ・座標指定とか指定は残念ながら画面上でクリックして座標調べるとかはできません、地道に調べましょう・・・・・・すまぬ、すまぬ

    ・この手の処理にはド定番の「はい/いいえ」の分岐はない、考え中だがたぶんやらない


    条件：

    size 1                                  サイズ指定は一番初めに書く、この直後の文を配列にぶち込んでいるので変に改行しないこと
    nonIdle                                 引数なし、この条件を指定したイベントはプレイヤーが動き続けられる
    position, x, y                          プレイヤーの座標が指定した位置にある（引数は座標XY）
    flag, index, 0or1                       特定のフラグが立っているとき/立っていないとき (引数はフラグの番号、boolの真偽で０をfalseとし、１をtrueとする)
    once, count                             このイベントの起こった回数が一定回数以下のとき（基本的には０を指定して一回だけイベントを起こすように使う）
    check, x, y                             指定した座標のマップオブジェクトを調べたとき(あたり判定のないものは今は調べられない);
    item, index, count                      指定したIDのアイテムをcount分持っているかどうか
    collision, num                          observerの中にあるオブジェクトがプレイヤーとぶつかったのか
 

    処理：

    size 2                                                          サイズ指定は一番初めに書く、この直後の文を配列にぶち込んでいるので変に改行しないこと
    setFlag, setIndex, bool                                         指定したフラグを立てたり折ったりする、０がfalseで１がtrue
    playerMove, direction, distance, speed                          「プレイヤー」を指定方向へ移動させる（directionはup,down,left,rightの四つ）
    setPlayerPos, posX, poyY                                        「プレイヤー」を指定座標（マス単位）へ移動させる。なおアニメーションはなく瞬間移動なのに注意されたし
    playerCenter                                                    引数なし、プレイヤーをマスの中心へ移動させる
    wait, time                                                      指定したフレーム数イベント処理を待つ
    changeImage, x, y, name                                         指定したマップ座標のオブジェクトをnameのプレハブと入れ替える
    setTextDest, position                                           テキストの表示位置の指定、同一イベント内でのみ有効（位置はtop,center,bottomの三つ）
    setTextColor, r, g, b, a                                        テキストの色を指定、値は０から１までの小数で指定する点に注意。floatを示すfはつけなくていいよん。
    setText, speed, string                                          setTextDist処理で指定した位置にstringの文字列をwaitをかけつつ表示、位置の初期値は下
    setLight, index, x, y, size, R1, G1, B1, A1, R2, G2, B2, A1     index番目の光の、位置、色、サイズを変更、色は透明度含む二色
    setPlayerLight, enable                                          ほとんどオープニング用、ろうそくをつけたり消したりする
    playSE, name, volume                                            効果音を鳴らす。ボリューム値は省略できる
    Play3DSE, x, y, name, volume                                    ３D効果音を鳴らす。音源位置調整もできるし音の大きさまで設定できる。ボリュームは省略できる。
    itemget,itemID                                                  アイテムを手に入れる。アイコンをUIに表示。
    itemlost,itemID                                                 アイテムを使う。アイコンUIを消して詰める。

    createEnemy, detected                                           エネミーを有効化／無効化する
    setEnemyMaxDetect, maxDetect                                    エネミーの最大感知度を設定、エネミーは最大感知度を現在の感知度が上回ると出現するのだ
    fadeIn, time                                                    タイムの速度でフェードイン
    fadeOut, time                                                   タイムの速度でフェードアウト
    *fadeColor, r, g, b                                             フェード色を指定、色は０から１までの小数で指定
    ChangeMap,mapIndex                                              ゲームの実行中にマップを変更する
    BGMStart, type, time, name, loop                                BGM切り替え、timeの値でフェードする。　タイプでBGMなのか、DetectSEなのか、ChaseSEなのかを入力してください。
    BGMStop, type, time                                             BGMストップ、timeの値でフェードする。　タイプでBGMなのか、DetectSEなのか、ChaseSEなのかを入力してください。
    BGMChange, type, name                                           BGMソースを切り替える。　タイプでBGMなのか、DetectSEなのか、ChaseSEなのかを入力してください。すまんなキム君、勝手に改造させてもらった。
    BGMFadeChange, type, name, time, loop                           BGMソースを切り替える。　タイプでBGMなのか、DetectSEなのか、ChaseSEなのかを入力してください。
    BGMChangeOneShot, type, volume, name, loop                      BGMソースを一発で切り替える。　タイプでBGMなのか、DetectSEなのか、ChaseSEなのかを入力してください。すまんなキム君、勝手に改造させてもらった。
    quake, time, power                                              画面をtime分のフレームだけ揺らす。powerは揺らす強さ（ドット単位）
    cameraMove, direction, distance, speed                          directionに "reset" を指定すると位置をプレイヤーに戻す。細かいところはプレイヤーのと大体は同じだが、quakeとは今のところは同時使用できない
    setCameraPos, x, y                                              これもプレイヤーの同じ、瞬間移動でカメラの位置を変更
    setCameraChase, bool                                            カメラがプレイヤーに追従するかどうかをboolの値で設定
    removeCollider,idx                                              そのインデックスにあるコライダーを壊す

    uiErase                                                         エンディング用、いらないUI等を非表示させます。
    Endingroot                                                      エンディング用、ルート変更します。
    endImage,name                                                   エンディングに出力する画像用,ファイル名(.pngは不要)

    setAnimState, index, valueName, type, value                     Animatorで定義した変数に値を代入する、indexはspawnオブジェクトの番号、valueName が変数名、type はbool,int,floatのどれか、value が値
    playerNoWaitMove, direction, distance, speed                    プレイヤーを移動、こっちは終わるまで待たない
    objectMove, index, direction, distance, speed                   spawnListに登録してあるオブジェクトを移動、オブジェクトの番号を指定しているところ以外はその他のMove系と同じ

    *setDark,bool,visible                                            三階層の闇の有効無効を切り替え、有効時は上から下に落ちてプレイヤーを殺す判定が出る、visibleを真の値に設定すると見える、偽で隠せる
    *setTextSize, size                                               テキストのサイズを指定、一文字ごとにサイズ指定はできないので注意
    *setTextFont, fontName                                           テキストに使用するフォントを名前で指定する
    *setLightSize, size                                              プレイヤーの持つライトのサイズ保証値を書き換えるint型でどうぞ

    ※随時追加予定、忘れてたら言ってちょ
    　新規追加処理には今度から * をつけとく、今後の参考にでもしておくれなんし
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

    //マップ切り替え時は処理をちょっと変える
    bool isChangeMap = false;

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

    //文字の表示位置と色
    Text textDest;
    Color textColor;


    
    
    public void Initialize()
    {
        //イベント情報を読み取り
        ReadEventText(ref eventCheck, ref eventText);
        SetCheckData();
        textDest = manager.textWindowBottom;
        textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void Run()
    {
        //Enableがfalseならイベントを無効にできる、また、ほかのイベント中はイベントが起こらない（eventStateが-1の時イベントはフリー）
        if (!manager.eventStatus[eventNumber].eventEnable || (manager.eventState != eventNumber && manager.eventState >= 0))
        {
            return;
        }

        //ずっと判定垂れ流すって実際どうなんでしょう、処理的に
        if (((eventCount > 0 || CheckEventEnter()) && !eventState && eventText != null && eventText.Length > 0))
        {
            //アニメーションを止める（正確には待機アニメーションへ移行）
            Animator animator = manager.player.GetComponent<Animator>();
            if (eventCount == 0)
            {
                animator.SetFloat("Player_Input_Y", 0.0f);
                animator.SetFloat("Player_Input_X", 0.0f);
            }

            textDest.text = "";
            StartEvent(eventCount);

            eventCount++;
            if (eventCount >= eventText.Length)
            {
                eventCount = 0;
                ++eventFlag;
            }
        }
        if (!isChangeMap)
        {
            if (eventCount > 0 || eventState)
            {
                manager.eventStatus[eventNumber].eventState = true;
            } else
            {
                manager.eventStatus[eventNumber].eventState = false;
            }
        }
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

        //フラグ999（ゲームオーバーイベント）を用いたイベントであったか
        bool isGameOverEvent = false;

        for (int i = 0; i < checkData.Length; ++i)
        {
            //インデックスによって処理を判断
            switch ((EventIndex)checkData[i][0])
            {
                //プレイヤーを止めるイベントかどうか、イベントに入るための条件というよりはイベント情報な気もする
                case EventIndex.EVENT_IF_IDLE:
                    {
                        manager.eventStatus[eventNumber].idleEvent = false;
                        break;
                    }

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
                    if(checkData[i][1] == 999)
                    {
                        isGameOverEvent = true;
                    }
                    break;

                //アイテムを指定数持っているか
                case EventIndex.EVENT_IF_ITEM:
                    if(manager.itemmanager.GetComponent<ItemManager>().itemData[checkData[i][1]].itemon_hand != checkData[i][2])
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
                    //調べるコマンドの発生するマップチップ初期化
                    checkX = checkData[i][1];
                    checkY = checkData[i][2];
                    //manager.mapObj[checkY, checkX].tag = "Untagged";
                    break;

                case EventIndex.EVENT_IF_COLLIDE:
                    if (manager.collideObservers.Count <= 0)
                        return false;
                    foreach (var obj in manager.collideObservers) {
                        if (obj.Key == checkData[i][1]) {
                            if (!manager.playerCheck.IsPushing ||
                                !obj.Value.name.Equals(manager.playerCheck.currentObjName)) 
                                    return false;
                            Debug.Log(obj.Value.name + " = " + manager.playerCheck.currentObjName);
                        }
                    }
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
            manager.mapObj[checkY, checkX].layer = LayerMask.NameToLayer("CheckAble");

            //調べたときにはマップのタグをUntaggedにする
            if (checkX != (int)manager.status.checkPos.x || checkY != (int)manager.status.checkPos.y)
            {
                return false;
            }
            Debug.Log("");
            manager.mapObj[checkY, checkX].tag = "Untagged";
            manager.status.checkPos = new Vector2(-100.0f, -100.0f);
        }

        //プレイヤーが死んでいて、ゲームオーバーイベントでないときはfalseでイベントも止めちゃう
        if (manager.prevState == PlayerState.Dead && !isGameOverEvent)
        {
            eventCount = 0;
            return false;
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

                    StartCoroutine(manager.func.PlayerMove((r) => eventState = r, manager.player, new Vector2(x, y), distance, speed, 0));
                    break;
                }
            //"playerNoWaitMove"処理
            case EventIndex.EVENT_FUNC_PLAYERNOWAITMOVE:
                {
                    int x = 0, y = 0;
                    if (text[1] == "up")
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

                    StartCoroutine(manager.func.PlayerMove((r) => eventState = r, manager.player, new Vector2(x, y), distance, speed, 1));
                    break;
                }
            //"setPlayerPos"処理
            case EventIndex.EVENT_FUNC_PLAYERSETPOSITION:
                {
                    int x = int.Parse(text[1]);
                    int y = int.Parse(text[2]);
                    PlayerStatus status = manager.player.GetComponent<PlayerStatus>();
                    status.posX = x;
                    status.posY = y;
                    manager.player.transform.position = new Vector3(status.posX * 128.0f, -status.posY * 128.0f, (status.posY - 64.0f) / 128.0f);
                    manager.cameraScr.transform.position = manager.player.transform.position;
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
                    //ファイルのパスを指定する（フォルダ名はMap + 階層数）
                    text[3] = "Map" + (manager.map.GetComponent<MapScriptTMX>().usedMapIndex + 1).ToString() + "/" + text[3];
                    manager.mapObj[y, x] = manager.func.ChangeImage(manager.mapObj[y, x], text[3]);
                    break;
                }

            //"setText"処理
            case EventIndex.EVENT_FUNC_SETTEXT:
                {
                    float wait = float.Parse(text[1]);
                    textDest.color = textColor;
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

            //"setColor"処理
            case EventIndex.EVENT_FUNC_SETTEXTCOLOR:
                {
                    float r = float.Parse(text[1]);
                    float g = float.Parse(text[2]);
                    float b = float.Parse(text[3]);
                    float a = float.Parse(text[4]);
                    textColor = new Color(r, g, b, a);
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
                    if (text.Length >= 3)
                        SEManager.Instance.SetVolume(float.Parse(text[2]));
                    else
                        SEManager.Instance.SetVolume(1f);
                    SEManager.Instance.PlaySE(text[1]);
                    break;
                }
            //"play3DSE"処理
            case EventIndex.EVENT_FUNC_PLAY3DSOUNDEFFECT:
                {
                    Vector2 pos = new Vector3(float.Parse(text[1]), float.Parse(text[2]));
                    float vol = 1f;
                    if (text.Length < 5)
                        vol = float.Parse(text[4]);
                    manager.func.Play3DSound(manager.speaker, pos, vol, text[3]);
                    break;
                }
            //"itemget"処理
            case EventIndex.EVENT_FUNC_ITEMGET:
                {
                    GameObject im = manager.itemmanager;
                    ItemManager m = im.GetComponent<ItemManager>();
                    m.itemData[int.Parse(text[1])].itemon_hand = 1;
                    if (m.itemData[int.Parse(text[1])].itemType == 2 && m.itemData[int.Parse(text[1])].itemID != 1)
                    {
                        GameObject Im_icon = manager.Importancebox;
                        Importancelist im_icon = Im_icon.GetComponent<Importancelist>();
                        im_icon.itemget(int.Parse(text[1]));
                    }
                    else
                    {
                        GameObject icon = manager.itembox;
                        itemlist item_icon = icon.GetComponent<itemlist>();
                        item_icon.itemget(int.Parse(text[1]));
                    }
                    Itemsavedata s = im.GetComponent<Itemsavedata>();
                    s.itemsaved(int.Parse(text[1]), m.itemData[int.Parse(text[1])].itemon_hand, m.itemData[int.Parse(text[1])].itemType);
                    
                    break;
                }
            //"itemlost"処理
            case EventIndex.EVENT_FUNC_ITEMLOST:
                {
                    GameObject icon = manager.itembox;
                    itemlist item_icon = icon.GetComponent<itemlist>();
                    item_icon.itemlost(int.Parse(text[1]));
                    manager.itemmanager.GetComponent<ItemManager>().itemData[int.Parse(text[1])].itemon_hand = 0;
                    break;
                }

            //"createEnemy"処理
            case EventIndex.EVENT_FUNC_ENEMYDETECTED:
                {
                    //createって書いてあるのになぜか消去もやってるんだが・・・過去の俺よ・・・てめえ・・・・・・
                    if(int.Parse(text[1]) == 0)
                    {
                        manager.enemyClass.enable = false;
                        manager.func.SoundFadeOut(manager.DetectSE.src, 30);
                        manager.func.SoundFadeOut(manager.ChaseSE.src, 30);
                        manager.enemyClass.KillEnemy();
                    }
                    else
                    {
                        manager.enemyClass.enable = true;
                    }
                    manager.status.detectCount = 1;
                    //manager.enemyClass.detectCount = 1;
                    manager.enemyClass.ChasePoint = 0;

                    break;
                }
            //"setEnemyMaxDetect"処理
            case EventIndex.EVENT_FUNC_ENEMYSETMAXDETECT:
                {
                    manager.enemyClass.detectMax = int.Parse(text[1]);
                    break;
                }
            //"fadeIn"処理
            case EventIndex.EVENT_FUNC_FADEIN:
                {
                    manager.fade.FadeIn(int.Parse(text[1]));
                    break;
                }
            //"fadeOut"処理
            case EventIndex.EVENT_FUNC_FADEOUT:
                {
                    manager.fade.FadeOut(int.Parse(text[1]));
                    break;
                }
            //"fadeColor"処理
            case EventIndex.EVENT_FUNC_FADECOLOR:
                {
                    manager.fade.render.color = new Color(float.Parse(text[1]), float.Parse(text[2]), float.Parse(text[3]), manager.fade.render.color.a);
                    break;
                }
            //"ChangeMap"処理
            case EventIndex.EVENT_FUNC_CHANGEMAP: 
                {
                    int mapNumber = int.Parse(text[1]);
                    manager.useEventIndex = mapNumber;
                    manager.mapLoader.ChangeMap(mapNumber);
                    manager.EventInitialize();
                    isChangeMap = true;
                    break;
                }
            //"BGMStart"処理
            case EventIndex.EVENT_FUNC_BGMSTART:
                {
                    AudioSource src = null;
                    switch (text[1]) {
                        case "BGM":             
                            manager.BGM.src.clip = manager.BGM.Clips[text[3]];
                            try { manager.BGM.src.loop = (int.Parse(text[4]) > 0 ? true : false); }
                            catch (System.IndexOutOfRangeException e) { manager.BGM.src.loop = true; }
                            src = manager.BGM.src;  
                            break;                        
                        case "DetectSE":        
                            manager.DetectSE.src.clip = manager.DetectSE.Clips[text[3]];
                            try { manager.DetectSE.src.loop = (int.Parse(text[4]) > 0 ? true : false); }
                            catch (System.IndexOutOfRangeException e) { manager.DetectSE.src.loop = true; }
                            src = manager.DetectSE.src;  
                            break;                        
                        case "ChaseSE":         
                            manager.ChaseSE.src.clip = manager.ChaseSE.Clips[text[3]];
                            try { manager.ChaseSE.src.loop = (int.Parse(text[4]) > 0 ? true : false); }
                            catch (System.IndexOutOfRangeException e) { manager.ChaseSE.src.loop = true; }
                            src = manager.ChaseSE.src;  
                            break;         
                        default: Debug.LogError("Scripting Error on BGMSTART"); break;               
                    }
                    src.volume = 0f;
                    src.Play();
                    manager.func.SoundFadeIn(src, int.Parse(text[2]));

                    break;
                }
            //"BGMStop"処理
            case EventIndex.EVENT_FUNC_BGMSTOP:
                {
                    switch (text[1]) {
                        case "BGM":      manager.func.SoundFadeOut(manager.BGM.src, int.Parse(text[2]));    break;                        
                        case "DetectSE": manager.func.SoundFadeOut(manager.DetectSE.src, int.Parse(text[2]));    break;
                        case "ChaseSE":  manager.func.SoundFadeOut(manager.ChaseSE.src, int.Parse(text[2]));    break;
                        default: Debug.LogError("Scripting Error on BGMSTOP"); break;               
                    }
                    break;
                }
            //"BGMChange"処理
            case EventIndex.EVENT_FUNC_BGMCHANGE:
                {
                    switch (text[1]) {
                        case "BGM":         manager.func.SoundChagne(manager.BGM.src, manager.BGM.Clips[text[2]]);  break;                        
                        case "DetectSE":    manager.func.SoundChagne(manager.DetectSE.src, manager.DetectSE.Clips[text[2]]);  break; 
                        case "ChaseSE":     manager.func.SoundChagne(manager.ChaseSE.src, manager.ChaseSE.Clips[text[2]]);  break; 
                        default: Debug.LogError("Scripting Error on BGMCHANGE"); break;               
                    }
                    break;
                }
            //"BGMFadeChange"処理 type name time loop
            case EventIndex.EVENT_FUNC_BGMFADECHANGE:
                {
                    bool isLoop = true;
                    if (text.Length >= 5)
                        isLoop = int.Parse(text[4]) > 0 ? true : false;
                    switch (text[1]) {
                        case "BGM":         manager.func.SoundChangeFade(manager.BGM.src, manager.BGM.Clips[text[2]], int.Parse(text[3]), isLoop);  break;                        
                        case "DetectSE":    manager.func.SoundChangeFade(manager.DetectSE.src, manager.DetectSE.Clips[text[2]], int.Parse(text[3]), isLoop);  break; 
                        case "ChaseSE":     manager.func.SoundChangeFade(manager.ChaseSE.src, manager.ChaseSE.Clips[text[2]], int.Parse(text[3]), isLoop);  break; 
                        default: Debug.LogError("Scripting Error on BGMCHANGE"); break;               
                    }
                    break;
                }
            //"BGMChangeOneShot"処理
            case EventIndex.EVENT_FUNC_BGMCHANGEONESHOT:
                {
                    switch (text[1]) {
                        case "BGM":         manager.BGM.PlayOneShot(float.Parse(text[2]), text[3], int.Parse(text[4]) > 0 ? true : false); break;
                        case "DetectSE":    manager.DetectSE.PlayOneShot(float.Parse(text[2]), text[3], int.Parse(text[4]) > 0 ? true : false); break;
                        case "ChaseSE":     manager.ChaseSE.PlayOneShot(float.Parse(text[2]), text[3], int.Parse(text[4]) > 0 ? true : false); break;
                        default: Debug.LogError("Scripting Error on BGMCHANGE"); break;               
                    }
                    break;
                }
            //quake"処理
            case EventIndex.EVENT_FUNC_QUAKE:
                {
                    manager.cameraScr.Quake(int.Parse(text[1]), int.Parse(text[2]));
                    break;
                }

            //"setDark"処理
            case EventIndex.EVENT_FUNC_SETDARK:
                {
                    //有効無効
                    if (int.Parse(text[1]) == 0)
                    {
                        manager.Darks.enable = false;
                    } else
                    {
                        manager.Darks.enable = true;
                    }

                    //表示
                    if (int.Parse(text[2]) == 0)
                    {
                        manager.Darks.gameObject.SetActive(false);
                    }else
                    {
                        manager.Darks.gameObject.SetActive(true);
                    }
                    break;
                }
            //"uiErase"処理
            case EventIndex.EVENT_FUNC_UIERASE:
                {
                    manager.UI.GetComponent<Canvas>().enabled = false;
                    manager.maskerase.mask_display(false);
                    manager.lighterase.Light_display(false);
                    Debug.Log("UI");
                    break;
                }
            //"Endingroot"処理
            case EventIndex.EVENT_FUNC_ENDROOT:
                {
                    GameObject m = manager.itemmanager;
                    ItemManager im = m.GetComponent<ItemManager>();
                    int STitem, Total;
                    STitem = 0;
                    Total = 0;
                    for(int check = 0; check < im.itemData.Length; ++check)//アイテムの数だけループ
                    {
                        if (im.itemData[check].itemID != 1) {//蝋燭を除外
                            if (im.itemData[check].itemType == 2)//ストーリーアイテムを選別
                            {
                                if (im.itemData[check].itemon_hand >= 1)//手持ち数が１以上のものを選別
                                {
                                    STitem++;
                                }
                            }
                        }
                    }
                    if (2 == STitem)
                    {
                        Total = 931;//Normal
                    }
                    else if (0 <= STitem && 1 >= STitem)
                    {
                        Total = 932;//Bad
                    }
                    else
                    {
                        Total = 930;//True
                    }
                    int setIndex = Total;
                    int set = 1;
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
            //"endImage"処理
            case EventIndex.EVENT_FUNC_BACKIMAGE:
                {
                    manager.endimage.endImage(text[1]);
                    break;
                }
            //"clear"処理
            case EventIndex.EVENT_FUNC_CLEAR:
                {
                    manager.gameclear.gameclear();
                    break;
                }
            //"cameraMove"処理
            case EventIndex.EVENT_FUNC_MOVECAMERA:
                {
                    int x = 0, y = 0;
                    if (text[1] == "up")
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

                    StartCoroutine(manager.func.CameraMove((r) => eventState = r, manager.cameraScr, new Vector2(x, y), distance, speed));
                    break;
                }

            //"setCameraPos"処理
            case EventIndex.EVENT_FUNC_SETCAMERAPOS:
                {
                    int x = int.Parse(text[1]);
                    int y = int.Parse(text[2]);
                    manager.cameraScr.cameraPos = new Vector3(x * 128.0f, -y * 128.0f, (y - 64.0f) / 128.0f - 100.0f);
                    manager.cameraScr.shiftCount = Vector3.zero;
                    break;
                }
            //"setCameraChase"処理
            case EventIndex.EVENT_FUNC_SETCAMERACHASE:
                {
                    if (int.Parse(text[1]) == 0)
                    {
                        manager.cameraScr.isPlayerChase = false;
                    }else
                    {
                        manager.cameraScr.isPlayerChase = true;
                    }
                    break;
                }

            //"destroyCollider"処理
            case EventIndex.EVENT_FUNC_REMOVECOLLIDER:
                {
                    Debug.Log("Destroy Object = " + text[1]);
                    manager.collideObservers[int.Parse(text[1])].gameObject.SetActive(false);
                    break;   
                }

            //"setAnimState"処理
            case EventIndex.EVENT_FUNC_SETANIMSTATE:
                {
                    int objIndex = int.Parse(text[1]);
                    string valueName = text[2];
                    string typeName = text[3];
                    //SpawnListのobjIndex番目のアニメーターを獲得
                    Animator animator = manager.map.GetComponent<SpawnManager>().objList[objIndex].GetComponent<Animator>();
                    Debug.Log(manager.map.GetComponent<SpawnManager>().objList[objIndex]);

                    if(typeName == "int")
                    {
                        animator.SetInteger(valueName, int.Parse(text[4]));
                    }
                    if(typeName == "bool")
                    {
                        if (int.Parse(text[4]) == 0) animator.SetBool(valueName, false);
                        else
                            animator.SetBool(valueName, true);
                    }
                    if (typeName == "float")
                    {
                        animator.SetFloat(valueName, float.Parse(text[4]));
                    }
                    break;
                }
            //"ObjectMove"処理
            case EventIndex.EVENT_FUNC_OBJECTMOVE:
                {
                    int objIndex = int.Parse(text[1]);
                    int x = 0, y = 0;
                    if (text[2] == "up")
                    {
                        y = -1;
                    }
                    else if (text[2] == "down")
                    {
                        y = 1;
                    }
                    else if (text[2] == "left")
                    {
                        x = -1;
                    }
                    else if (text[2] == "right")
                    {
                        x = 1;
                    }

                    int distance = int.Parse(text[3]);
                    int speed = int.Parse(text[4]);

                    StartCoroutine(manager.func.ObjMove((r) => eventState = r, manager.map.GetComponent<SpawnManager>().objList[objIndex], new Vector2(x, y), distance, speed));
                    break;
                }
            //"setTextSize"処理
            case EventIndex.EVENT_FUNC_SETTEXTSIZE:
                {
                    textDest.fontSize = int.Parse(text[1]);
                    break;
                }
            //"setTextFont"処理
            case EventIndex.EVENT_FUNC_SETTEXTFONT:
                {
                    textDest.font = (Font)Resources.Load("Font/" + text[1]);
                    break;
                }
            //"setLightSize"処理
            case EventIndex.EVENT_FUNC_SETLIGHTMINSIZE:
                {
                    manager.status.minLightCircle = int.Parse(text[1]);
                    break;
                }
            default:
                return;
        }
    }
    

}




