using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;     // 発射する弾（プレハブ）
    public float delayTime = 3.0f;   // 弾を撃つまでの待機時間（秒）
    public float fireSpeed = 4.0f;   // 弾の発射速度
    public float length = 0.0f;      // プレイヤーを感知する範囲

    GameObject player;              // プレイヤー本体
    Transform gateTransform;        // 発射口（砲身の先などに空オブジェクトを設置）
    float passedTimes = 0;          // 経過時間（待機カウント用）

    // プレイヤーが砲台の感知範囲内にいるかチェック
    bool CheckLength(Vector2 targetPos)
    {
        float d = Vector2.Distance(transform.position, targetPos); // 距離測定
        return (length >= d); // 範囲内かどうかを返す
    }

    void Start()
    {
        // 発射口を取得（子オブジェクトに "gate" が必要）
        gateTransform = transform.Find("gate");

        // プレイヤーを取得（タグで検索）
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 時間カウント
        passedTimes += Time.deltaTime;

        // プレイヤーが範囲内にいる場合だけ発射判定
        if (CheckLength(player.transform.position))
        {
            // 待機時間を越えたら弾を発射
            if (passedTimes > delayTime)
            {
                passedTimes = 0; // カウントリセット

                // 発射位置を発射口に設定
                Vector2 pos = gateTransform.position;

                // 弾の生成
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);

                // 発射方向の計算（オブジェクトの角度からベクトルを算出）
                float angleZ = transform.localEulerAngles.z;
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
                Vector2 dir = new Vector2(x, y).normalized;

                // 弾に力を加えて発射
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                if (rbody != null)
                {
                    rbody.AddForce(dir * fireSpeed, ForceMode2D.Impulse);
                }
            }
        }
    }

    // エディタ上で範囲を可視化（選択中のみ）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
