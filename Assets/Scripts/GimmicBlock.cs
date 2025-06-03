using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicBlock : MonoBehaviour
{
    public float length = 0.0f; //自動落下検知距離
    public bool isDelete = false; //落下後に消滅するフラグ
    public GameObject deadObj; //死亡当たり判定

    bool isFell = false; //落下フラグ
    float fadeTime = 0.5f; //フェードアウト時間

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("GimmicBlock is alive! " + gameObject.name);

        //Rigidbody2Dの物理挙動を停止
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        deadObj.SetActive(false); //死亡当たりを非表示

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("GimmicBlock is alive! " + gameObject.name);

        GameObject player =
        GameObject.FindGameObjectWithTag("Player"); //プレイヤーを探す
        if (player != null)
        {
            //プレイヤーとの距離測定
            float d = Vector2.Distance(
                transform.position, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)
                {
                    //Rigidbody2Dの物理挙動の開始
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                    deadObj.SetActive(true); //死亡当たり判定を表示
                }
            }
        }
        if (isFell)
        {
            fadeTime -= Time.deltaTime; // ←ここ！減らしていく
            Color col = GetComponent<SpriteRenderer>().color;
            col.a = Mathf.Clamp01(fadeTime / 0.5f); // 最初0.5→0.0に向かうなら0～1に正規化
            GetComponent<SpriteRenderer>().color = col;
            if (fadeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }


    }

    //接触開始
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);


        if (isDelete)
        {
            isFell = true;//落下フラグオン
        }
    }

    //範囲表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }

}

