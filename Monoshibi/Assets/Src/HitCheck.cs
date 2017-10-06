using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitCheck : MonoBehaviour {

    PlayerStatus player;
    bool coroutineState = false;

	void Start () {
        //コンポーネントを取得
        player = this.transform.parent.GetComponent<PlayerStatus>();
	}
	
	void Update () {
		
	}
    
    //衝突
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(player.state == PlayerState.Dead)
        {
            return;
        }

        //敵に捕まった
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(DelayProcess());
        }
        //青い光に入っている
        if (collision.gameObject.tag == "HidingSpace")
        {
            if (!coroutineState && player.detectCount > 120)
            {
                StartCoroutine(DetectFadeOut((r) => coroutineState = r));
            }
        }
        //ｶｰﾁｬﾝの手に捕まった
        if(collision.gameObject.tag == "Hand")
        {
            Debug.Log(collision.transform.parent.GetChild(1).name);
            this.transform.parent.position = collision.transform.parent.GetChild(1).position - new Vector3(0.0f, 32.0f, 0.0f);
            this.transform.parent.GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(CatchHand((r) => coroutineState = r));
        }

    }

    //カゲたちに捕まったら囲まれて輪ｋ・・・もといゲームオーバーになる
    private IEnumerator DelayProcess()
    {
        player.state = PlayerState.Dead;
        yield return new WaitForSeconds(2.0f);

    }
    //感知度は青い光の部屋に入った瞬間にゼロまで落ちる
    private IEnumerator DetectFadeOut(UnityAction<bool> result)
    {
        int decrease = (int)((float)player.detectCount / 30.0f);
        result(true);
        do
        {
            yield return null;
            player.detectCount -= decrease;
            if (player.detectCount < 0)
            {
                player.detectCount = -1;
            }
        } while (player.detectCount >= 0);

        result(false);
        yield return null;
    }
    //手に捕まったらゲームオーバー演出へ
    private IEnumerator CatchHand(UnityAction<bool> result)
    {
        result(true);
        SEManager.Instance.SetVolume(1.0f);
        SEManager.Instance.PlaySE("hole_open");

        int numHand = 30;
        GameObject clone;
        System.Random rand = new System.Random();
        yield return new WaitForSeconds(1.2f);

        for(int i = 0; i < numHand; ++i)
        {
            if (player.state == PlayerState.Dead)
            {
                break;
            }
            clone = Instantiate((GameObject)Resources.Load("Prehubs/Map3/GameOverHand"), new Vector3(0, 0, 0), Quaternion.identity, this.transform.parent);
            StartCoroutine(HandAnim(clone, (float)rand.Next(360)));
            if (i > 4)
            {
                player.minLightCircle *= 0.8f;
                player.lightPower *= 0.3f;
                yield return new WaitForSeconds(rand.Next(5) / 10.0f);
            }
            else
            {
                yield return new WaitForSeconds(rand.Next(7) / 10.0f);
            }
        }
        result(false);
        yield return null;
    }
    //手の動き
    IEnumerator HandAnim(GameObject obj, float deg)
    {
        int wait = 6;
        float rad = (deg + 90) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0.0f) * 4.0f;
        obj.transform.localRotation = Quaternion.AngleAxis(deg, new Vector3(0.0f, 0.0f, 1.0f));

        for (int i = 0; i <= wait; ++i)
        {
            obj.transform.localPosition = direction * (1.0f - (float)i / wait);
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y + 0.4f, -4.0f);
            yield return null;
        }
        SEManager.Instance.SetVolume(1.0f);
        SEManager.Instance.PlaySE("bloodHand");

        yield return null;
    }
}
