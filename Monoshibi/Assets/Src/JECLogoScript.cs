using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JECLogoScript : MonoBehaviour {

    int classTimer;
    FadeScript fade;


    private void Start()
    {
        Cursor.visible = false;
        fade = this.GetComponent<FadeScript>();
        classTimer = 0;
    }

    // Update is called once per frame
    void Update () {
        CheckTimer();
        ++classTimer;
	}

    //時間管理関数
    void CheckTimer()
    {
        if (classTimer == 0)
        {
            fade.FadeOut(1);
        }
        if (classTimer == 50)
        {
            fade.FadeIn(100);
        }
        if(classTimer == 290)
        {
            fade.FadeOut(100);
        }
        if(classTimer == 420)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}
