using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataInitialize : MonoBehaviour {

	
	void Start () {

        Itemsavedata.candle_save = 0;
        Itemsavedata.kokesi_save = 0;
        Itemsavedata.omamori_save = 0;
        Itemsavedata.flower_save = 0;
        Itemsavedata.kusi_save = 0;
        Itemsavedata.dendendram_save = 0;
        MapScriptTMX.savedMapNumber = -1;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
