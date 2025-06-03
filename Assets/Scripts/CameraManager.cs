using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // === カメラが移動できる範囲（XとY） ===
    public float leftLimit = -10f;     // カメラが行ける最も左
    public float rightLimit = 20f;     // カメラが行ける最も右
    public float topLimit = 5f;        // 一番上
    public float bottomLimit = -5f;    // 一番下（← f を忘れがちなので注意！）

    // サブスクリーン（例：ミニマップ・カメラ演出用）
    public GameObject subScreen;

    void Update()
    {
        // タグが "Player" のオブジェクトを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // プレイヤーが存在する場合のみ追従
        if (player != null)
        {
            // プレイヤーの現在位置を取得
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z; // カメラのZ軸はそのまま固定

            // === カメラの移動範囲を制限 ===
            x = Mathf.Clamp(x, leftLimit, rightLimit);
            y = Mathf.Clamp(y, bottomLimit, topLimit);

            // カメラの位置をプレイヤーに追従させる
            transform.position = new Vector3(x, y, z);

            // === サブスクリーンもカメラに連動して追従させる ===
            if (subScreen != null)
            {
                // メインカメラの現在位置を取得
                Vector3 camPos = Camera.main.transform.position;

                // サブスクリーンの表示オフセット位置（例：右下に表示）
                float offsetX = 3.5f;
                float offsetY = -2.5f;

                // サブスクリーンの位置を更新（Zは固定）
                subScreen.transform.position = new Vector3(
                    camPos.x + offsetX,
                    camPos.y + offsetY,
                    subScreen.transform.position.z
                );
            }
        }
        else
        {
            Debug.LogWarning("Player が見つかりませんでした。タグ 'Player' を確認してください。");
        }
    }
}
