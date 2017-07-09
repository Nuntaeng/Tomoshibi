using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : MonoBehaviour {
    //エネミーのステータスを保持するクラス、エネミーって書いてるけどまだコメント編集してないからだから気にしなくてよろし

    //エネミー配列での自身のインデックス
    public int index;
    //配列マップ上での座標
    public int posX, posY;
    //移動速度
    public int speed;
    //状態
    public PlayerState state;

}
