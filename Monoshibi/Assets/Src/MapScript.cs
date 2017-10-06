using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MapScript : MonoBehaviour
{


    public GameObject prehub_Item;
    public int[] useMapChip;
    public SpawnManager spawnManager;
    MapScriptTMX tmxLoader;


    //TiledMap製マップの読み込み先
    int[,] map1;
    GameObject[,] obj;

    void Start()
    {
        tmxLoader = GetComponent<MapScriptTMX>();
        
        ChangeMap(MapScriptTMX.savedMapNumber);

        // map1 = tmxLoader.Map;
        // obj = tmxLoader.MapObject;
        // int maxX = tmxLoader.MapWidth;
        // int maxY = tmxLoader.MapHeight;

        // Test
        //map2 = generator.GetGeneratedMap();

        //LoadTiledMap("Test", ref map1);
        //int maxX = map2.GetLength(1);
        //int maxY = map2.GetLength(0);

        //マップオブジェクトのインスタンスを保存するために配列を確保


        //マップをセット
        //for (int y = 0; y < maxY; ++y)
        //{
        //    for (int x = 0; x < maxX; ++x)
        //    {
        //        clone = Instantiate(prehub[map1[y, x]], new Vector3(0, 0, 0), Quaternion.identity);
        //        obj[y, x] = clone;

        //        clone.transform.position = new Vector3((float)(x * 128), (float)(y * -128), -y + 1);
        //    }
        //}
        //アイテム位置の抽選
        //System.Random randX = new System.Random(map2.GetLength(1));
        //System.Random r = new System.Random();
        //int randX = 1;
        //int randY = 1;
        //do
        //{
        //    randX = r.Next(map2.GetLength(1));
        //    randY = r.Next(map2.GetLength(0));
        //} while (map2[randY, randX] != 1);
        //アイテムを置く
        ////test用
        //randX = 1;
        //randY = 3;
        //NewMethod(randX, randY);

        // プレイヤーの位置も歩けるタイルの上にする
        // Modifide by Heewon Kim 
        // 修正時間：2017/06/21 23:04
        //do
        //{
        //    randX = r.Next(map2.GetLength(1));
        //    randY = r.Next(map2.GetLength(0));
        //} while (map2[randY, randX] != 1);
        //playerTrans.position = new Vector3((float)(randX * 128), (float)(randY * -128), -randY + 1);
        //Debug.Log(randX);
        //Debug.Log(randY);



    }

    
    public void ChangeMap(int mapNumber) {
        tmxLoader.ClearMap();
        tmxLoader.mapNumber = mapNumber;
        MapScriptTMX.savedMapNumber = mapNumber;
        tmxLoader.usedMapIndex = useMapChip[mapNumber];
        tmxLoader.LoadMapFromTMXFile();
        tmxLoader.PrintCurrentMap();
        spawnManager.MapChange(mapNumber);

        map1 = tmxLoader.Map;
        obj = tmxLoader.MapObject;
        int maxX = tmxLoader.MapWidth;
        int maxY = tmxLoader.MapHeight;
    }

    private void NewMethod(int randX, int randY)
    {
        Instantiate(prehub_Item, new Vector3((randX * 128), (randY * -128), 10), Quaternion.identity);
    }

    /////////////////////////////
    //マップデータ読み込み、テキストファイル如きにノード処理まで積んであるとは驚きだ
    void LoadTiledMap(string name, ref int[,]mapArr)
    {

        TextAsset mapText = (TextAsset)Resources.Load("MapData/" + name, typeof(TextAsset));
        string data = mapText.text;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(mapText.text);
        XmlNodeList xmlList = xmlDoc.GetElementsByTagName("map");

        //xmlのノードをたどってマップ読み込み
        foreach(XmlNode map in xmlList)
        {
            XmlNodeList childList = map.ChildNodes;
            foreach(XmlNode childMap in childList)
            {
                //レイヤータグ以外は無視
                if(childMap.Name != "layer")
                {
                    continue;
                }
                XmlAttributeCollection attr = childMap.Attributes;
                //幅と高さを取得
                int width = int.Parse(attr.GetNamedItem("width").Value);
                int height = int.Parse(attr.GetNamedItem("height").Value);


                //layerタグの子ノードはdataのみらしい、というわけでdataタグの下にあるマップ情報を文字列として格納
                XmlNode mapValue = childMap.FirstChild.FirstChild;
                string mapData = mapValue.Value;

                //マップ配列の確保
                mapArr = new int[height, width];
                int y = -1;//何故か一行目が空白だったので調整でマイナス１にしてる
                foreach (string line in mapData.Split('\n'))
                {  
                    int x = 0;
                    foreach(string str in line.Split(','))
                    {
                        
                        int v = 0;
                        if(!int.TryParse(str, out v))
                        {
                            continue;
                        }
                        mapArr[y, x] = v;                      
                        ++x;
                    }
                    ++y;
                }
            }
        }
    }
    ///////////////////


    //外部にデータを渡す
    public int [, ] GetMap()
    {
        return map1;
    }
    public GameObject[,] GetMapObject()
    {
        return obj;
    }
}
