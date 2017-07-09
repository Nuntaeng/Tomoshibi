using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagClass : MonoBehaviour {
    //フラグを管理するクラス、それだけ
    public bool[] flag;
    private void Start()
    {
        this.hideFlags = HideFlags.NotEditable;
        flag = new bool[1000];
    }
}
