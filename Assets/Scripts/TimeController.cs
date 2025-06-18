using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // ======= Shopシーンまたはパネル中は時間を止める =======
        if (GameManager.Instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name.ToLower();
            if (sceneName.Contains("shop") || GameManager.Instance.IsItemPanelOpen())
                return;
        }

        // ======= 時間が止まっている状態なら処理しない =======
        if (isTimeOver) return;

        // ======= 時間カウント処理開始 =======
        times += Time.deltaTime;

        if (isCountDown)
        {
            displayTime = gameTime - times;
            if (displayTime <= 0.0f)
            {
                displayTime = 0.0f;
                isTimeOver = true;
            }
        }
        else
        {
            displayTime = times;
            if (displayTime >= gameTime)
            {
                displayTime = gameTime;
                isTimeOver = true;
            }
        }
    }
}
