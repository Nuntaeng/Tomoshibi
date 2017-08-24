using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class EventFunctions : MonoBehaviour {


    //ゲーム内イベント用関数群、早い話がRPGツクールのアレ




    //プレイヤーキャラクターの移動、斜めはやらないほうが良いよ
    public IEnumerator PlayerMove(UnityAction<bool> result, GameObject obj, Vector2 direction, int distance, int speed)
    {
        result(true);
        int moveCnt = 0;
        direction = direction.normalized;
        int posX = (int)((obj.transform.position.x + 64.0f) / 128.0f);
        int posY = (int)((obj.transform.position.y - 64.0f) / 128.0f);


        //テスト用、たぶん消・・・さない///////////////////////////////////////////////
        Animator animator = obj.GetComponent<Animator>();

        yield return null;

        animator.SetFloat("Player_Input_Y", -direction.y);
        animator.SetFloat("Player_Input_X", direction.x);
        obj.GetComponent<PlayerStatus>().direction = (new Vector2(direction.x, -direction.y) * 100.0f).normalized;
        /////////////////////////////////////////////////////////////////////////

        while (moveCnt < distance)
        {
            //長さ１２８の分を移動する
            float i = 0;
            while(i < 128.0f)
            {
                obj.transform.position += new Vector3(direction.x, -direction.y, 0) * speed;
                i += speed;
                yield return null;
            }
            //移動を終えたらfloatの計算の誤差を修正、修正というかマス単位の座標にキャラを置きなおしてる
            //int moveX = (int)direction.x;
            //int moveY = (int)direction.y;
            //if (moveX != 0)
            //{
            //    posX += moveX;
            //    obj.transform.position = new Vector3(posX * 128, obj.transform.position.y, obj.transform.position.z);
            //}
            //else if(moveY != 0)
            //{
            //    posY -= moveY;
            //    obj.transform.position = new Vector3(obj.transform.position.x, posY * 128 + 64.0f, obj.transform.position.z);
            //}
            moveCnt++;
        }

        yield return null;
        animator.SetFloat("Player_Input_Y", 0.0f);
        animator.SetFloat("Player_Input_X", 0.0f);
        //イベントの終了を告げるfalseを返す
        result(false);
        yield return null;
    }

    //プレイヤーをマップ上のマスの中心へといざなう。なぜ初めから処理を分けておかんかったのか・・・・・・ｺﾚｶﾞﾜｶﾗﾅｲ
    public IEnumerator PlayerCenterMove(UnityAction<bool> result, GameObject obj)
    {
        result(true);
        int posX = (int)((obj.transform.position.x + 64.0f) / 128.0f);
        int posY = (int)((obj.transform.position.y - 64.0f) / 128.0f);


        //テスト用、たぶん消・・・さない///////////////////////////////////////////////
        Animator animator = obj.GetComponent<Animator>();

        //マスの真ん中に座標を補正(X→Yの順)
        //X方向
        float fixX = posX * 128.0f - obj.transform.position.x;
        Vector3 fixDir = new Vector3(fixX, 0.0f, 0.0f).normalized;

        //アニメーションとプレイヤーの向きを補正
        animator.SetFloat("Player_Input_Y", 0.0f);
        animator.SetFloat("Player_Input_X", fixDir.x);
        obj.GetComponent<PlayerStatus>().direction = new Vector2(fixDir.x, 0.0f);

        while (fixX > 1.0f || fixX < -1.0f)
        {
            obj.transform.localPosition += fixDir * 2;
            fixX = posX * 128.0f - obj.transform.position.x;
            yield return null;
        }
        obj.transform.position = new Vector3(posX * 128.0f, obj.transform.position.y, obj.transform.position.z);

        //Y方向
        float fixY = posY * 128.0f - obj.transform.position.y;
        fixDir = new Vector3(0.0f, fixY, 0.0f).normalized;
        //アニメーションとプレイヤーの向きを補正
        animator.SetFloat("Player_Input_Y", fixDir.y);
        animator.SetFloat("Player_Input_X", 0.0f);
        obj.GetComponent<PlayerStatus>().direction = new Vector2(0.0f, fixDir.y);


        while (fixY > 1.0f || fixY < -1.0f)
        {
            obj.transform.localPosition += fixDir * 2;
            fixY = posY * 128.0f - obj.transform.position.y;
            yield return null;
        }
        obj.transform.position = new Vector3(obj.transform.position.x, posY * 128.0f, obj.transform.position.z);

        //イベント終了を示すfalseを返す
        result(false);
        yield return null;
    }


    //ウェイト、地味に結構使うやつ
    public IEnumerator Wait(UnityAction<bool> result, int frame)
    {
        result(true);
        for (int i = 0; i < frame; ++i)
        {
            yield return null;
        }

        //イベントの終了を告げるfalseを返す
        result(false);
        yield return null;
    }


    //チェンジイメージとか言っておきながらやってることはプレハブを新たに生成して元のやつと入れ替えるという矛盾、リプレイスじゃねーか！
    public GameObject ChangeImage(GameObject obj, string name)
    {
        name = name.Replace("\n", "");
        string path = "Prehubs/" + name;
        GameObject prefab = (GameObject)Resources.Load(path, typeof(GameObject));
        //プレハブが見つからなかったらそのまま帰っていただく
        if (prefab == null)
        {
            return obj;
        }
        Debug.Log(path);

        //インスタンスの生成に成功した場合は引数のインスタンスを破棄して代わりに生成したインスタンスを返す
        GameObject clone = Instantiate(prefab, obj.transform.position, Quaternion.identity);
        if(clone == null)
        {
            return obj;
        }


        clone.transform.parent = obj.transform.parent;
        Destroy(obj);

        return clone;
    }

    //テキストを出す
    public IEnumerator SetTextWindow(UnityAction<bool> result, float wait, string text, UnityEngine.UI.Text textWindow, GameObject obj)
    {
        result(true);
        Animator animator = obj.GetComponent<Animator>();
        animator.SetFloat("Player_Input_Y", 0.0f);
        animator.SetFloat("Player_Input_X", 0.0f);

        for (int i = 0; i < text.Length; ++i)
        {
            textWindow.text = text.Substring(0, i + 1);
            //Debug.Log(i);
            yield return new WaitForSeconds(wait / 60.0f);
        }

        while(!Input.GetButtonDown("Fire1")){
            yield return null;
        }

        textWindow.text = "";
        result(false);
        yield return null;
    }

    //ライトをいじる
    public void SetLight(LightManager lightobj, int index, int x, int y, Color color, Color color2, float size)
    {
        lightobj.SetLight(index, x, y, color, color2, size);
    }
    //プレイヤーのライトをいじる
    public void SetPlayerLight(int set, GameObject player)
    {
        if (set == 0)
        {
            player.transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            player.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    public void Play3DSound(Speaker speaker, Vector2 pos, float volume, string name) {
        speaker.transform.localPosition = pos;
        speaker.mySource.volume = volume;
        speaker.PlayEffect(name);
        Debug.Log("Play! : " + name);
    }


    // BGM方のコード
    public void SoundFadeIn(AudioSource src, int time) {
        src.volume = 0f;
        StartCoroutine(SFadeIn(src, time));
    }

    public void SoundFadeOut(AudioSource src, int time) {
        src.volume = 1f;
        StartCoroutine(SFadeOut(src, time));
    }

    public void SoundChagne(AudioSource src, AudioClip clip, int time) {
        src.volume = 1f;
        StartCoroutine(SChange(src, clip, time));
    }

    IEnumerator SFadeIn(AudioSource src, int time) {
        for (int i = 1; i <= time; ++i)
        {
            src.volume = (float)i / (float)time;
            yield return null;
        }
    }
    IEnumerator SFadeOut(AudioSource src, int time) {
        for (int i = 1; i <= time; ++i)
        {
            src.volume = 1f - (float)i / (float)time;
            yield return null;
        }
        src.Stop();
    }
    IEnumerator SChange(AudioSource src, AudioClip clip, int time) {
        yield return StartCoroutine(SFadeOut(src, time));
        src.clip = clip;
        src.Play();
        yield return StartCoroutine(SFadeIn(src, time));
    }
}


