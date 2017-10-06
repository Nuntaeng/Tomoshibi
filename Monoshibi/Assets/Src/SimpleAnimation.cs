using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleAnimation : MonoBehaviour {
	public float speed = 0.5f;
	public Sprite[] frames;
	SpriteRenderer renderer = null;
	int curFrame = 0;


	void Start() {
		renderer = this.GetComponent<SpriteRenderer>();
		renderer.sprite = frames[0];
		StartCoroutine(ChangeCut());
	}


	IEnumerator ChangeCut() {
		while (true) {
			yield return new WaitForSecondsRealtime(speed);
			if (curFrame < frames.Length - 1)
				curFrame += 1;
			else
				curFrame = 0;
			renderer.sprite = frames[curFrame];
		}
	}
}
