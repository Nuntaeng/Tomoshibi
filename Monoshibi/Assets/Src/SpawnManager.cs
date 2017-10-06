using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	[System.Serializable]
	public class MapObjs {
		public GameObject[] prefabs;
	}
	public MapObjs[] spawnList;
	public PlayerCheck playerCheck;

    [HideInInspector]
    public GameObject[] objList;

	public void MapChange(int idx) {
		playerCheck.dynamicObjNames = new string[spawnList[idx].prefabs.Length];
        objList = new GameObject[spawnList[idx].prefabs.Length];

		int dynamicCnt = 0;
		foreach (var o in spawnList[idx].prefabs) {
			playerCheck.dynamicObjNames[dynamicCnt] = o.name + "(Clone)";
            objList[dynamicCnt] = GameObject.Instantiate(o);
            objList[dynamicCnt].transform.parent = GameObject.Find("MapManager").transform;
			dynamicCnt += 1;
		}
	}
}
