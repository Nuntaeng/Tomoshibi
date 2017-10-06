using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagClass : MonoBehaviour {

    #region Singletone Definition
    static FlagClass _instance;
    public static FlagClass Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(FlagClass)) as FlagClass;
                if(_instance == null)
                {
                    return null;
                }
            }
            return _instance;
        }
    }
    #endregion

    //フラグを管理するクラス、それだけ
    public bool[] flag;
    private void Start()
    {
        this.hideFlags = HideFlags.NotEditable;
        flag = new bool[1000];
    }
}