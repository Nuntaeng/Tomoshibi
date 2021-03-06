﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;



[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    #region Singletone Definition
    static SEManager _instance;
    public static SEManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(SEManager)) as SEManager;
                if(_instance == null)
                {
                    return null;
                }
            }
            return _instance;
        }
    }
    #endregion

    public AudioSource audioSource;
    public AudioClip[] clips;
    float volume = 1f;

    
    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    public void SetVolume(float level) {
        volume = level;
    }

    public void PlaySE(string name)
    {
        for(int i = 0; i < clips.Length; i++)
        {
            if(clips[i] == null)
            {
                continue;
            }
            if (clips[i].name == name) {
                audioSource.PlayOneShot(clips[i], volume);
                Debug.Log("SE => " + name);
            }
        }
    }
}
