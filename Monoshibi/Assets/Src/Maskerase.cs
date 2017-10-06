using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maskerase : MonoBehaviour {

    // Use this for initialization
    public void mask_display(bool display)
    {
        gameObject.SetActive(display);
    }
}
