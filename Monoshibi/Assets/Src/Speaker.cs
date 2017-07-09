using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour {

	public AudioClip[] soundList;
	public AudioSource mySource;



	public void PlayEffect(string name) {
		foreach (var c in soundList)
			if (c.name.Equals(name)) {
				mySource.PlayOneShot(c);
			}
	}

}
