using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMask : MonoBehaviour {

    public GameObject player;
    PlayerStatus status;
    SpriteRenderer render;

	void Start () {
        status = player.GetComponent<PlayerStatus>();
        render = this.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        render.color = new Color(render.color.r, render.color.g, render.color.b, 1.0f - 0.1f * status.lightPower / status.maxLightPower);
	}
}
