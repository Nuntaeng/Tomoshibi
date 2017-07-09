using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGet : MonoBehaviour {

    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Item")
       {
            // このコンポーネントを持つGameObjectを破棄する
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
