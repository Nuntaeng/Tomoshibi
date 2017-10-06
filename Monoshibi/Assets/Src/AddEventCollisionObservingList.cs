using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEventCollisionObservingList : MonoBehaviour {

	public int index = 0;
	void Start () {
		GameObject.Find("EventManager").GetComponent<EventManager>().collideObservers.Add(index, this.transform);
	}
}
