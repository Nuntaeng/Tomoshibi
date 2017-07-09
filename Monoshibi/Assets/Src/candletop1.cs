using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candletop1 : MonoBehaviour {
    public Vector3 defaultScale = Vector3.one;
    public Vector3 initalizeScale = Vector3.zero;
    void Start()
    {
        defaultScale = transform.lossyScale;
        initalizeScale = transform.localScale;
    }

    void Update()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;
        //Debug.Log(localScale.y);
        //Debug.Log(lossScale.y);
        //Debug.Log(defaultScale.y);
        if (lossScale.y == 0)
        {
            lossScale.y = -0.01f;
        }
        if (lossScale.y != 0.0f)
        {
            transform.localScale = new Vector3(
                    localScale.x / lossScale.x * defaultScale.x,
                    localScale.y / lossScale.y * defaultScale.y,
                    localScale.z / lossScale.z * defaultScale.z
            );
        }
    }
}
