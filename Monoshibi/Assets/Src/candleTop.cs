using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candleTop : MonoBehaviour {
    public float candie_move;
    public Texture2D candie_position;
    
    
    void Start () {
        candie_move = 0.045f;
	}
	

	void Update () {
        Vector3 pos = transform.position;
        if (pos.y > 80)
        {
            pos.y -= candie_move;
            transform.position = pos;
        }
        else
        {
            pos.y = 80;
            transform.position = pos;

        }
	}
}
