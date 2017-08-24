using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIdraw : MonoBehaviour {

    public void uidraw()
    {
        GetComponent<CanvasGroup>().alpha = 0.0f;
    }
    public void finalize()
    {
        GetComponent<CanvasGroup>().alpha = 1.0f;
    }
}
