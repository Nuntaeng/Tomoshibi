using System;
using System.Collections.Generic;
using UnityEngine;





[Serializable]
public class Sound {
	public string name;
	public AudioClip clip;
}

public class SoundContainer : MonoBehaviour {
	[SerializeField] 
	Sound[] sounds;
	public AudioSource src;

	Dictionary<string, AudioClip> dic = new Dictionary<string, AudioClip>();
	public Dictionary<string, AudioClip> Clips {
		get {
			return dic;
		}
	}

	void Awake() {
		foreach(var snd in sounds)
			dic.Add(snd.name, snd.clip);
	}
}
