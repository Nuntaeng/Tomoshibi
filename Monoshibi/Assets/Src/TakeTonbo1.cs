using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeTonbo1 : MonoBehaviour {

	IEnumerator eventCoroutine;
	PlayerStatus player;
	public Animator anime;


	void Start() {
		player = GameObject.Find("Player").GetComponent<PlayerStatus>();
		eventCoroutine = CheckEventStatus();
		StartCoroutine(eventCoroutine);
	}

	void OnDisable() {
		StopCoroutine(eventCoroutine);
	}

	IEnumerator CheckEventStatus() {
		while(true) {
			if (player.state.Equals(PlayerState.Idle))
				anime.SetFloat("walkingSpeed", 0f);
			else
				anime.SetFloat("walkingSpeed", 1f);
			yield return null;
		}			
	}
}
