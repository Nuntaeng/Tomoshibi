using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shiMove : MonoBehaviour {

    int timeCnt;

	// Use this for initialization
	void Start () {
        timeCnt = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timeCnt++;
        if (timeCnt >= 100)
        {
            timeCnt = 0;
        }

        if (timeCnt % Random.Range(1, 10) == 3)
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30.0f, 0.0f));
        }
    }
}
