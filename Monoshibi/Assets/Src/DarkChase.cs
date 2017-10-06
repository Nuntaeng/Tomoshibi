using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//闇がこっちを追いかけるスクリプト
public class DarkChase : MonoBehaviour {
    public GameObject darkObj;
    public GameObject player;
    PlayerStatus status;
    public float speed;
    public bool enable;

    System.Random rand = new System.Random();
    int numDark = 2500;
    SpriteRenderer[] renderObj;

	void Start () {
        status = player.GetComponent<PlayerStatus>();
        renderObj = new SpriteRenderer[numDark];

        //闇を生成
        for (int i = 0; i < numDark; ++i)
        {
            //位置や色とかアニメ速度をランダムで決定

            float x = (float)rand.Next(4000) - 1000;
            float y = (float)rand.Next(1600);
            if (i < 1000)
            {
                y = (float)rand.Next(1000) + 600;
            }
            float color = (float)rand.Next(200) / 200.0f;
            GameObject clone = Instantiate(darkObj, new Vector3(x, y, -50.0f), Quaternion.identity);
            clone.transform.parent = this.transform;
            clone.GetComponent<Animator>().SetInteger("animState", 1);
            clone.GetComponent<Animator>().speed = (float)rand.Next(1, 20) * 0.1f;
            renderObj[i] = clone.GetComponent<SpriteRenderer>();
            clone.GetComponent<SpriteRenderer>().color = new Color(color, color, color, 0.0f);
        }
	}
	
	void Update () {
        if (enable && status.state == PlayerState.Active)
        {
            this.transform.localPosition += new Vector3(0.0f, -speed, 0.0f);
        }
        for(int i = 0; i < numDark; ++i)
        {
            Color c = renderObj[i].color;
            renderObj[i].color = new Color(c.r, c.g, c.b, (2400.0f + (player.transform.position.y - this.transform.position.y)) / 2400.0f);
        }
        if (this.transform.position.y < player.transform.position.y + 1000.0f && enable)
        {
            status.state = PlayerState.Dead;
        }
    }
}
