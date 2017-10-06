using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マスクを柔軟に扱いたいがためのクラス、今のところほぼ出番なし
public class LightMask : MonoBehaviour {

    public GameObject player;
    PlayerStatus status;
    SpriteRenderer render;

	void Start () {
        status = player.GetComponent<PlayerStatus>();
        render = this.GetComponent<SpriteRenderer>();
        render.enabled = true;
	}
	
	void Update () {
	}
}
