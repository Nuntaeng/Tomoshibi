using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndImage : MonoBehaviour {
    public Sprite BackImage;
    public Image backimage;
    private bool switch_ = false;
    private float time = 0.0f;
    public void endImage(string name)
    {
        backimage = GetComponent<Image>();
        BackImage = Resources.Load<Sprite>("Sprite/" + name + ".png");
        backimage.sprite = BackImage;
        switch_ = true; 
    }
    private void Update()
    {
        if (switch_)
        {
            if (time <= 1)
            {
                time += 0.02f;
                var color = backimage.color;
                color.a = time;
                backimage.color = color;
            }
            else
            {
                switch_ = false;
            }
        }
    }
}
