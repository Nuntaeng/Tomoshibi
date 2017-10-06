using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameout : MonoBehaviour {

	
	void Start () {
        Cursor.visible = false;

    }
	
	
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetButton("gameout1") &&
            Input.GetButton("gameout2") &&
            Input.GetButton("gameout3") &&
            Input.GetButton("gameout4"))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
