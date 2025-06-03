using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ==== UI関連 ====
    public GameObject mainImage;              // ゲーム結果画像（勝ち・負け）
    public Sprite gameOverSpr;               // ゲームオーバー画像
    public Sprite gameClearSpr;              // ゲームクリア画像
    public GameObject panel;                 // ボタン類を表示するパネル
    public GameObject restartButton;         // リスタートボタン
    public GameObject nextButton;            // 次のステージへ進むボタン

    // ==== ステージ遷移 ====
    public static int currentStage = 1;      // 現在のステージ番号（staticで共有）

    // ==== 時間関連 ====
    public GameObject timeBar;               // 時間バー（表示のON/OFF制御）
    public TextMeshProUGUI timeText;         // 残り時間の数字表示
    TimeController timeCnt;                  // TimeControllerスクリプト参照

    // ==== スコア関連 ====
    public TextMeshProUGUI scoreText;        // スコア表示用テキスト
    public static int totalScore = 0;        // 全体スコア（ステージをまたいで記録）
    public int stageScore = 0;               // ステージ内スコア（アイテム取得など）

    void Start()
    {
        // 最初のシーンだけでステージ番号を初期化
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            currentStage = 1;
        }

        // メイン画像を一時非表示にして、パネルも隠す
        Invoke("InactiveImage", 1.0f);
        panel.SetActive(false);

        // 時間制御スクリプトの取得
        timeCnt = GetComponent<TimeController>();

        // ゲーム時間が0なら時間バーを非表示にする
        if (timeCnt != null && timeCnt.gameTime == 0.0f)
        {
            timeBar.SetActive(false);
        }

        UpdateScore(); // スコア初期表示
    }

    void Update()
    {
        // === ゲームクリア処理 ===
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            // クリア時はリスタート禁止
            restartButton.GetComponent<Button>().interactable = false;

            // クリア画像に切り替え
            mainImage.GetComponent<Image>().sprite = gameClearSpr;

            // ゲーム状態を終了に設定
            PlayerController.gameState = "gameend";

            // 残り時間をスコア加算
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;
            }

            // ステージスコアを合計に加算
            totalScore += stageScore;
            stageScore = 0;

            UpdateScore(); // スコアUI更新
        }

        // === ゲームオーバー処理 ===
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            // ゲームオーバー時は次に進めない
            nextButton.GetComponent<Button>().interactable = false;

            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            if (timeCnt != null)
                timeCnt.isTimeOver = true;
        }

        // === 通常プレイ中 ===
        else if (PlayerController.gameState == "playing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                PlayerController playerCnt = player.GetComponent<PlayerController>();

                // 時間カウント＆表示
                if (timeCnt != null && timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime;
                    timeText.text = time.ToString();

                    if (time <= 0)
                    {
                        playerCnt.GameOver(); // 時間切れで強制敗北
                    }
                }

                // スコア加算（スコアアイテム取得など）
                if (playerCnt.score != 0)
                {
                    stageScore += playerCnt.score;
                    playerCnt.score = 0;
                    UpdateScore();
                }
            }
            else
            {
                Debug.LogWarning("Player が見つかっていません！");
            }
        }
    }

    // メイン画像を非表示にする（開始直後に1秒遅らせて実行）
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // 「次へ」ボタン押下時の処理
    public void OnNextButton()
    {
        currentStage++;

        if (currentStage == 2)
        {
            // Stage1 → Stage2 へ
            SceneManager.LoadScene("BaseScene2");
        }
        else if (currentStage == 3)
        {
            // Stage2 → リザルト画面へ
            SceneManager.LoadScene("ResultScene");
        }
    }

    // スコア表示を更新
    void UpdateScore()
    {
        int score = stageScore + totalScore;

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogWarning("scoreText が Inspector でアサインされていません！");
        }
    }
}
