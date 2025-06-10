using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム中の制限時間や経過時間を管理するタイマースクリプト
public class TimeController : MonoBehaviour
{
    // === 設定用パラメータ ===
    public bool isCountDown = true; // trueならカウントダウン、falseならカウントアップ
    public float gameTime = 0;      // ゲームの最大時間（秒数で設定）
    public bool isTimeOver = false; // trueならタイマー停止（時間切れや目標達成）
    public float displayTime = 0;   // UI表示用の残り時間または経過時間

    float times = 0; // 内部で使う経過時間カウント用（毎フレーム加算）

    // ====== ゲーム開始時に1回だけ呼ばれる ======
    void Start()
    {
        if (isCountDown)
        {
            // カウントダウンの場合は最初に最大時間からスタート
            displayTime = gameTime;
        }
        // カウントアップ時は0から自動スタートなので何もしなくてOK
    }

    // ====== 毎フレーム呼ばれる ======
    void Update()
    {
        // タイマーが止まっていなければ（まだ終了していなければ）
        if (!isTimeOver)
        {
            times += Time.deltaTime; // 1フレームぶんの時間を加算（秒単位）

            if (isCountDown)
            {
                // === カウントダウン処理 ===
                displayTime = gameTime - times; // 残り時間を計算
                // Debug.Log("カウントダウン中: " + displayTime);

                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f; // 0未満にならないように補正
                    isTimeOver = true;  // タイマー終了
                }
            }
            else
            {
                // === カウントアップ処理 ===
                displayTime = times; // 経過時間を表示用に格納

                if (displayTime >= gameTime)
                {
                    displayTime = gameTime; // 最大時間以上にならないように補正
                    isTimeOver = true;      // タイマー終了
                }

                // Debug.Log("経過時間: " + displayTime);
            }
        }
    }
}
