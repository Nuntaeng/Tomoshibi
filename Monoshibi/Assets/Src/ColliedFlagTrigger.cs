using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliedFlagTrigger : MonoBehaviour {

	[System.Serializable]
	public class FlagStatus {
		public int flagNum;
		public bool status;
	}
	public FlagStatus[] destroyFlagNum;
	public FlagStatus[] editFlagNum;
	public FlagStatus[] checkFlagNum;
	public string[] target;

	void OnTriggerEnter2D(Collider2D other)
	{
		foreach (var t in target)
			if (other.name.Equals(t)) {
				foreach (var f in checkFlagNum) {
					if (FlagClass.Instance.flag[f.flagNum] == f.status)
						foreach (var fg in editFlagNum)
							FlagClass.Instance.flag[fg.flagNum] = fg.status;
				}

				bool checker = false;
				foreach (var f in destroyFlagNum)
					if (FlagClass.Instance.flag[f.flagNum] == f.status) {
						checker = true;	
						break;
					}
				if (checker)
					this.gameObject.SetActive(false);
			}
	}
}
