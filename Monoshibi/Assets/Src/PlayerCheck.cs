using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//調べるコマンドのためのコライダー判定
public class PlayerCheck : MonoBehaviour {
    PlayerStatus status;
    public Text textbox;
    public string currentObjName = "";
    Collider2D colli;

    public GameObject player;
    public GameObject checkIcon;
    public string[] dynamicObjNames;

    //押してる時間
    int pushingCount = 0;
    bool isPushing = false;
    public bool IsPushing {
        get { return isPushing; }
    }

	void Start () {
        status = this.transform.parent.GetComponent<PlayerStatus>();
        colli = this.GetComponent<Collider2D>();

        checkIcon.SetActive(false);
    }
	
	void Update () {
        //プレイヤーの向きに沿って判定を移動
        colli.offset = new Vector2(status.direction.x, status.direction.y + 0.5f) * 0.4f;

    }


    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            ++pushingCount;
        }
        else
        {
            pushingCount = 0;
        }

        //イベント中は文字をいじらない
        if (status.state != PlayerState.Idle)
        {
            textbox.text = "";
        }
        else
        {
            pushingCount = 100;
        }
    }


    void OnTriggerEnter2D(Collider2D collider) {
        currentObjName = collider.name;
    }

    //判定に入ったら調べられる
    private void OnTriggerStay2D(Collider2D collision)
    {
        isPushing = false;
        //イベント中は判定しない
        if(status.state == PlayerState.Idle)
        {
            return;
        }
        
        //開く
        //if (collision.gameObject.tag == "Door")
        //{
        //    textbox.text = "Ctl:開く";
        //}
        ////調べる
        //if (collision.gameObject.tag == "MapObject")
        //{
        //    textbox.text = "Ctl:調べる";
        //}

        if(collision.gameObject.tag == "Door" || collision.gameObject.tag == "MapObject")
        {
            checkIcon.SetActive(true);
        }

        //調べるボタンが押されたときに判定に仕事してもらう
        if (pushingCount < 2 && pushingCount > 0)
        {
            isPushing = true;
            //ドアはイベントじゃなくてタグで感知
            if (collision.gameObject.tag == "Door")
            {
                GameObject door = collision.transform.GetChild(0).gameObject;
                //左右のドアを開くコルーチンを再生
                StartCoroutine(DoorOpen(20, -2.0f, door.transform.GetChild(0).gameObject));
                StartCoroutine(DoorOpen(20, 2.0f, door.transform.GetChild(1).gameObject));

                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

                //SEManager.Instance.PlaySE("to_build_a_bridge");
                SEManager.Instance.PlaySE("Sliding_door");
                textbox.text = "";

                //ドア開けたらcheckIcon非表示
                checkIcon.SetActive(false);
            }
            //イベントのオブジェクトさん、調べたフラグと共に座標をセット
            if (collision.gameObject.tag == "MapObject")
            {
                bool isDynamicObject = false;
                foreach (var d in dynamicObjNames)
                    if (collision.name == d) {
                        status.checkPos = collision.transform.position;
                        isDynamicObject = true;
                        Debug.Log(d);
                    }
                if (!isDynamicObject)
                    status.checkPos = new Vector2(collision.gameObject.transform.position.x / 128.0f, collision.gameObject.transform.position.y / -128.0f);

                textbox.text = "";

                //調べたらCheckIcon非表示
                checkIcon.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //判定外ではCheckIconのアクティブをfalseに
        checkIcon.SetActive(false);
        Debug.Log("IconFalse");

        currentObjName = "";
    }

    IEnumerator DoorOpen(int time, float move, GameObject sprite)
    {
        SpriteRenderer spRend = sprite.GetComponent<SpriteRenderer>();
        sprite.gameObject.transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        for(int i = 0; i <= time; ++i)
        {
            sprite.transform.position += new Vector3(move, 0.0f, 0.0f);
            spRend.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (float)i / time);
            yield return null;
        }
    }
}
