using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class MapChip
{
    public GameObject[] mapChips;
}


public class MapScriptTMX : MonoBehaviour {
    //シーン間でのマップ番号を保持するリトライ用の変数
    public static int savedMapNumber = -1;


    public int mapNumber;
    public string[] stageAddr;
    public MapChip[] mapChipsArray;
    [HideInInspector]
    public int usedMapIndex;

    int[,] map;
    GameObject[,] mapObject;

    int mapWidth = 0;
    int mapHeight = 0;

    /// <summary>
    /// マップデータ
    /// </summary>
    public int[,] Map {
        get { return map; }
    }

    public GameObject[,] MapObject {
        get { return mapObject; }
    }

    public int MapWidth {
        get { return mapWidth; }
    }

    public int MapHeight {
        get { return mapHeight; }
    }

    void Awake() {
        if(SystemInfo.operatingSystem.Contains("Mac OS X")) {
            Debug.Log("OS X Detected, Change Path...");
            for(int i = 0; i < stageAddr.Length; ++i) {
                stageAddr[i] = stageAddr[i].Replace('\\', '/');
                Debug.Log(stageAddr[i]);
            }
        }
        if (savedMapNumber == -1)
        {
            savedMapNumber = mapNumber;
        }
    }

    public void LoadMapFromTMXFile() {
        XmlReader xml = XmlReader.Create(stageAddr[mapNumber]);
        while (xml.Read()) {

            // マップの大きさをファイルから読み取る
            if (xml.NodeType.Equals(XmlNodeType.Element) &&
                xml.Name.Equals("map"))
            {
                map = new int[int.Parse(xml.GetAttribute("height")),
                              int.Parse(xml.GetAttribute("width"))];
                mapWidth = map.GetLength(1);
                mapHeight = map.GetLength(0);
                mapObject = new GameObject[mapHeight, mapWidth];
            }
            
            // マップを読み込んで代入します
            if (xml.NodeType.Equals(XmlNodeType.Text))
            {
                xml.Value.Replace('\n', ' ');
                string[] chipType = xml.Value.Split(',');
                for (int y = 0; y < mapHeight; ++y)
                    for (int x = 0; x < mapWidth; ++x)
                    {
                        map[y, x] = int.Parse(chipType[y * mapWidth + x]);
                    }
            }
        }
    }

    /// <summary>
    /// 完成したマップを画面上に出力する
    /// </summary>
    public void PrintCurrentMap() {
        for (int y = 0; y < mapHeight; ++y) 
            for (int x = 0; x < mapWidth; ++x)
            {
                mapObject[y, x] = Instantiate(mapChipsArray[usedMapIndex].mapChips[map[y, x]], 
                    new Vector3((float)(x * 128), (float)(y * -128), -y + 1),
                    Quaternion.identity);
                mapObject[y, x].transform.parent = this.transform;
            }
    }


    /// <summary>
    /// 今出力されているマップを全部消す
    /// </summary>
    public void ClearMap() {
        for (int i = 0; i < this.transform.childCount; ++i)
            Destroy(this.transform.GetChild(i).gameObject);
    }
}
