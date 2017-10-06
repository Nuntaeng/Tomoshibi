using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideObserver : MonoBehaviour {

	public bool isCollide = false;
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Player")
			isCollide = true;
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.name == "Player")
			isCollide = false;
	}	
}
