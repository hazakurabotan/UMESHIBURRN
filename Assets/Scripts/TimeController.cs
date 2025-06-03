using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // === 設定用パラメータ ===
    public bool isCountDown = true; // trueならカウントダウン、falseならカウントアップ
    public float gameTime = 0; // ゲームの最大時間（秒数で設定）
    public bool isTimeOver = false; // trueならタイマー停止（時間切れ）
    public float displayTime = 0; // 表示用の残り時間または経過時間（UI表示用）

    float times = 0; // 内部で使う経過時間カウント用（Updateで加算）

    void Start()
    {
        if (isCountDown)
        {
            // カウントダウンの場合は、最初は最大時間からスタート
            displayTime = gameTime;
        }
    }

    void Update()
    {
        // 時間計測が止まっていなければ処理する
        if (!isTimeOver)
        {
            times += Time.deltaTime; // 1フレーム分の時間を加算（秒単位）

            if (isCountDown)
            {
                // === カウントダウン処理 ===
                displayTime = gameTime - times; // 残り時間を計算
                // Debug.Log("カウントダウン中: " + displayTime); // デバッグ出力

                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f; // 負の時間にならないように補正
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

               //  Debug.Log("TIMES: " + displayTime); // デバッグ出力
            }
        }
    }
}
