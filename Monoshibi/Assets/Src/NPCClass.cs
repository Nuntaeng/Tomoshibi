using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミーの状態構造体(Inspectorでは構造体が表示されないのでクラスで代用)
[System.Serializable]
public class NPCData
{
    public UnityEngine.GameObject enemyObj;
    public int posX, posY;
    public int speed;
}

//エネミーの位置や種類を管理してマップに配置するクラス、エネミーって書いてるけどまだコメント編集してないからだから気にしなくてよろし
[System.Serializable]
public class NPCClass : MonoBehaviour {
    public UnityEngine.GameObject mapObj;
    public UnityEngine.GameObject player;
    public NPCData[] NPC;

	void Start () {
        for(int i = 0; i < NPC.Length; ++i)
        {
            UnityEngine.GameObject clone;
            NPCStatus status;
            clone = Instantiate(NPC[i].enemyObj, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
            status = clone.GetComponent<NPCStatus>();
            status.index = i;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
