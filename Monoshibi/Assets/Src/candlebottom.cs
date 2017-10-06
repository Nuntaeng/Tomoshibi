using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class candlebottom : MonoBehaviour {
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0.0f;
    }
    public void candlebottomui()
    {
        GetComponent<CanvasGroup>().alpha = 1.0f;
    }
}
