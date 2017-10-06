using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/**
 *  @author Heewon Kim (nabicore@icloud.com) 
 *  @brief  竹とんぼの動作をしていします。
 * */
public class TakeTonbo : MonoBehaviour
{
    delegate bool Judgement();
    [System.Serializable]
    public class Node {
        public Vector2 point;
        public Vector2 direction;
        public float F;
        [HideInInspector]
        public Node prevNode = null;
    }
    readonly Vector2[] DIRECTIONS = {
        Vector2.left,   Vector2.right,
        Vector2.up,     Vector2.down
    };


    public float speed = 300f;
    public float detectRange = 500f;

    public Node[] idlePath;
    public Node[] runPoint;

    Transform playerTrans = null;
    SpriteRenderer myRenderer = null;
    int[,] map = null;
    int mapWidth = 0, mapHeight = 0;
    Vector2 moveDirection = Vector2.zero;
    IEnumerator idleWalking;


    void Start() {
        playerTrans = GameObject.Find("Player").transform;
        myRenderer = this.GetComponent<SpriteRenderer>();
        var mapParser = GameObject.Find("MapManager").GetComponent<MapScriptTMX>();
        map = mapParser.Map;
        mapWidth = mapParser.MapWidth;
        mapHeight = mapParser.MapHeight;

        for (int i = 0; i < idlePath.Length; ++i)
            idlePath[i].point = ToMapCoord(idlePath[i].point);

        //StartCoroutine(FollowPath(SearchPath(GetFarRunPoint()), false));
        StartCoroutine(IdleWalk());
    }

    void Update() {
        var curPos = this.transform.position;
        this.transform.position = new Vector3(curPos.x + (moveDirection.x * speed * Time.deltaTime),
                                              curPos.y + (moveDirection.y * speed * Time.deltaTime), 
                                              (curPos.y + 64.0f) / 128.0f);
    }

    /**
        @brief  현제 위치에서 가장 먼 도망갈 장소를 가져옵니다.
        @return 말 그대로.
     */
    Vector2 GetFarRunPoint() {
        float maxDist = 0f;
        Vector2 ret = Vector2.zero;
        foreach (var p in runPoint) {
            float tmpDist = Vector2.Distance(this.transform.position, p.point);
            if (maxDist < tmpDist) {
                maxDist = tmpDist;
                ret = p.point;
            }
        }
        return ret;
    }

    /**
        @brief  목적지 까지의 경로를 찾습니다.
        @param  목적지 좌표, ToMapCoord로 변환한 좌표만 사용하시오.
        @return 목적지까지의 최단 경로
     */
    Node[] SearchPath(Vector2 dest) {
        List<Node> openList      = new List<Node>();
        List<Node> closeList     = new List<Node>();

        Node startPos = new Node() { point = ToMapCoord(this.transform.position), F = 0f };
        Node currentPos = startPos;
        float G = 0f, H = 0f;
        
        while (!currentPos.point.Equals(dest)) {

            foreach (var dir in DIRECTIONS) {
                Node tmpCurrentPos = new Node() { point = currentPos.point + dir, direction = dir, prevNode = currentPos, F = 0f };
                if (isOnRoad(tmpCurrentPos.point)) {
                    G = Vector2.Distance(tmpCurrentPos.point, startPos.point);
                    H = Vector2.Distance(tmpCurrentPos.point, dest);
                    tmpCurrentPos.F = G + H;
                    Judgement isAddListSuccess = () => {
                        foreach (var cl in closeList)
                            if (tmpCurrentPos.point.Equals(cl.point))
                                return false;
                        foreach (var ol in openList) 
                            if (tmpCurrentPos.point.Equals(ol.point) && 
                                ol.F > tmpCurrentPos.F) {
                                    openList.Remove(ol);
                                    openList.Add(tmpCurrentPos);
                                    return false;
                                }
                        openList.Add(tmpCurrentPos);
                        return true;
                    };
                    isAddListSuccess();
                }
            }
                                
            if (openList.Count != 0) {
                Node bestNode = openList[0];
                for (int i = 0; i < openList.Count; ++i)
                        if (openList[i].F < bestNode.F)
                        bestNode = openList[i];
                openList.Remove(bestNode);
                closeList.Add(bestNode);
                currentPos = bestNode;
            }
        }
        
        List<Node> path = new List<Node>();
        while (currentPos.prevNode != null) {
            if (currentPos.direction == Vector2.up ||
                currentPos.direction == Vector2.down)
                currentPos.direction *= -1f;
            path.Add(currentPos);
            currentPos = currentPos.prevNode;
        }
        path.Reverse();
        return path.ToArray();
    }

    /**
        @brief  배열에 있는 경로따라 이동합니다.
     */
    IEnumerator FollowPath(Node[] path, bool isLoop) {
        int curPathIdx = 0;

        moveDirection = path[curPathIdx].direction;
        Vector2 currentPos = ToMapCoord(this.transform.position);

        for (int i = 0; i < path.Length; ++i)
            Debug.Log(i + " / " + path[i].point.ToString() + " / " + path[i].direction.ToString());

        while (curPathIdx < path.Length - 1) {
            currentPos = ToMapCoord(this.transform.position);
            if (currentPos.Equals(path[curPathIdx].point)) {
                curPathIdx += 1;
                moveDirection = path[curPathIdx].direction;
                Debug.Log(curPathIdx + "/" + path.Length + "  " + moveDirection.ToString() + "  == CHANGED!");
            }
            yield return null;
        }

        yield return StartCoroutine(Respawn());
    }

    /**
        @brief  타케톤보를 일반 위치로 재소환시킵니다.
     */
    IEnumerator Respawn() {
        while (myRenderer.color.a > 0) {
            myRenderer.color = new Color(myRenderer.color.r,
                                         myRenderer.color.g,
                                         myRenderer.color.b,
                                         myRenderer.color.a - 0.05f);
            yield return null;
        }
        this.transform.position = ToUnityCoord(idlePath[idlePath.Length - 1].point);
        moveDirection = idlePath[0].direction;
        while (myRenderer.color.a < 1f) {
            myRenderer.color = new Color(myRenderer.color.r,
                                         myRenderer.color.g,
                                         myRenderer.color.b,
                                         myRenderer.color.a + 0.05f);
            yield return null;
        }
        StartCoroutine(IdleWalk());
    }

    IEnumerator IdleWalk() {
        idleWalking = FollowPath(idlePath, true);
        StartCoroutine(idleWalking);
        while (true) {
            if (Vector2.Distance(this.transform.position, playerTrans.position) < detectRange) {
                Debug.Log("Detected!");
                StopCoroutine(idleWalking);
                yield return StartCoroutine(FollowPath(SearchPath(ToMapCoord(GetFarRunPoint())), false));
                StopCoroutine("IdleWalk");
            }
            yield return null;
        }
    }

    Vector2 ToMapCoord(Vector3 origin) {
        return new Vector2((int)(origin.x / 128.0f), (int)(origin.y / -128.0f));
    }

    Vector2 ToUnityCoord(Vector2 origin) {
        return new Vector2(origin.x * 128f, origin.y * -128f);
    }

    Vector2 ToVector2(Vector3 origin) {
        return new Vector2(origin.x, origin.y);
    }

    bool isOnRoad(Vector2 coord) {
        return
            coord.x >= 0 && coord.x < mapWidth && coord.y >= 0 && coord.y < mapHeight &&
            map[(int)coord.y, (int)coord.x] == 1 ;
    }
}