using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MapScriptTMX : MonoBehaviour {

    public GameObject[] mapChips;
    public string stageAddr;

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

    public void LoadMapFromTMXFile() {
        XmlReader xml = XmlReader.Create(stageAddr);
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
                mapObject[y, x] = Instantiate(mapChips[map[y, x]], 
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



    // ---------------------------------------------------
    private void Awake()
    {
        LoadMapFromTMXFile();
        PrintCurrentMap();
    }
}
