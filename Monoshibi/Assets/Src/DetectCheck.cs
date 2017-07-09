using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCheck : MonoBehaviour {

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //敵が光を感知
        if (collision.gameObject.tag == "Player")
        {
            NPCStatus enemy = collision.GetComponent<NPCStatus>();
            if(enemy.state == PlayerState.Idle)
            enemy.state = PlayerState.Active;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //敵が光を見失う
        if (collision.gameObject.tag == "Player")
        {
            NPCStatus enemy = collision.GetComponent<NPCStatus>();
            enemy.state = PlayerState.Hiding;
        }

    }

}
