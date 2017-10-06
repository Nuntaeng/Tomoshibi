using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//三階層の演出用、プレイヤーが近づくとマスが実体化する
public class PopFloor : MonoBehaviour {
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
            if (sprite.enabled == false)
            {
                sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                sprite.enabled = true;
                StartCoroutine(FadeIn(20));
            }
        }
    }

    IEnumerator FadeIn(int time)
    {
        SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
        for (int i = 0; i < time; ++i)
        {
            sprite.color = new Color(1.0f, 1.0f, 1.0f, (float)i / time);
            yield return null;
        }
    }
}

