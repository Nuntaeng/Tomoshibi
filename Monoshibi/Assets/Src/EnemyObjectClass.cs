using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//もう名前も若干困り始めるころでございます、エネミーの管理クラスが生み出すエネミーオブジェクトを制御する
public class EnemyObjectClass : MonoBehaviour {

    [HideInInspector]
    public int posX, posY;
    [HideInInspector]
    public PlayerStatus status;
    //[HideInInspector]
    public GameObject map;


    public void Initialize () {
        this.GetComponent<EnemyChase>().Initialize();
	}
	
	public void KillEnemy()
    {
        StartCoroutine(FadeEnemy(60));
    }


    IEnumerator FadeEnemy(int time)
    {
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();

        for(int i = 1; i <= time; ++i)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1.0f - ((float)i / (float)time));
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
