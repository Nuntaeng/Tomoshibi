using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミーの状態構造体(Inspectorでは構造体が表示されないのでクラスで代用)
[System.Serializable]
public class LightData
{
    public int posX, posY;
    public float scale;
    public Color color1;
    public Color color2;

    //さすがにこれ公開するの不味いのでprivate
    private UnityEngine.GameObject cloneObject;
    //クローンのインスタンスのゲットとセット
    public void SetObj(GameObject obj)
    {
        cloneObject = obj;
    }
    public GameObject GetObj()
    {
        return cloneObject;
    }
}

//エネミーの位置や種類を管理してマップに配置するクラス
[System.Serializable]
public class LightManager : MonoBehaviour
{
    public LightData[] lightData;

    void Start()
    {
        //lightData = new LightData[100];
        for (int i = 0; i < lightData.Length; ++i)
        {
            UnityEngine.GameObject clone;
            GameObject lightObj = (GameObject)Resources.Load("prehubs/LightPrefab");
            clone = Instantiate(lightObj, new Vector3(lightData[i].posX * 128, -lightData[i].posY * 128, 0), Quaternion.identity, this.transform);
            clone.transform.localScale = new Vector3(lightData[i].scale * 128, lightData[i].scale * 128, 1.0f);
            clone.transform.GetChild(0).GetComponent<SpriteRenderer>().color = lightData[i].color1;
            clone.transform.GetChild(1).GetComponent<SpriteRenderer>().color = lightData[i].color2;
            lightData[i].SetObj(clone);
        }

    }

    //イベントあたりから呼び出してライトをセットするのだ、内容はパブリックの変数に状態を格納したのちにオブジェクトに反映させてるだけ
    public void SetLight(int index, int x, int y, Color color, Color color2, float size)
    {
        lightData[index].color1 = color;
        lightData[index].color2 = color2;
        lightData[index].posX = x;
        lightData[index].posY = y;
        lightData[index].scale = size;

        GameObject lightObj = lightData[index].GetObj();
        lightObj.transform.localScale = new Vector3(size * 128.0f, size * 128.0f, 1.0f);
        lightObj.transform.localPosition = new Vector3(x * 128.0f, -y * 128.0f);
        lightObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        lightObj.transform.GetChild(1).GetComponent<SpriteRenderer>().color = color2;
    }
}