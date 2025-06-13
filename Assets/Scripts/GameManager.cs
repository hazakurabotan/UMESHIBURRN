using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// ゲーム全体の進行・管理を担当するクラス
public class GameManager : MonoBehaviour
{
    // ==== UI関連 ====
    public GameObject mainImage;              // 結果表示用の画像（勝ち・負け画面など）
    public Sprite gameOverSpr;                // ゲームオーバー時に表示する画像
    public Sprite gameClearSpr;               // ゲームクリア時に表示する画像
    public GameObject panel;                  // ボタンやUIをまとめたパネル
    public GameObject restartButton;          // リスタート（やり直し）ボタン
    public GameObject nextButton;             // 次のステージへ進むボタン


    //アイテムパネル関連
    public GameObject itemDisplayPanel;       //ゲーム中にAキーでアイテムパネルの表示/非表示」
    bool isItemPanelOpen = false;
    PlayerController playerController;
    EnemyController[] enemyControllers; // 敵スクリプト名は合わせて
    BossSimpleJump[] BossControllers; // 敵スクリプト名は合わせて
    BossSimpleJump boss; // ←追加

    // ==== ステージ遷移 ====
    public static int currentStage = 1;      // 現在のステージ番号（全体で共有）

    // ==== 時間関連 ====
    public GameObject timeBar;               // 時間バー（時間がある場合だけ表示）
    public TextMeshProUGUI timeText;         // 残り時間の数字表示
    TimeController timeCnt;                  // 時間管理用スクリプト（同じオブジェクトにアタッチ）

    // ==== スコア関連 ====
    public TextMeshProUGUI scoreText;        // スコア表示用テキスト
    public static int totalScore = 0;        // 総合スコア（ステージをまたいで引き継ぎ）
    public int stageScore = 0;               // ステージごとのスコア（アイテム取得等で加算）

    // ====== 初期化処理 ======
    void Start()
    {
        // 最初のシーン（"Stage1"）ならステージ番号を1にリセット
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            currentStage = 1;
        }

        // 最初はリザルト画像を一時非表示、パネルも隠す
        Invoke("InactiveImage", 1.0f);
        panel.SetActive(false);

        // 時間管理スクリプト取得（同じGameObjectにアタッチしておくこと！）
        timeCnt = GetComponent<TimeController>();

        // 時間制限が無ければ時間バー自体を非表示
        if (timeCnt != null && timeCnt.gameTime == 0.0f)
        {
            timeBar.SetActive(false);
        }

        if (itemDisplayPanel != null) itemDisplayPanel.SetActive(false);

        // スコアUIを初期表示
        UpdateScore();

        playerController = FindObjectOfType<PlayerController>();
        enemyControllers = FindObjectsOfType<EnemyController>(); // 敵も管理したい場合
        boss = FindObjectOfType<BossSimpleJump>(); // Bossも管理したい場合

    }

    // ====== 毎フレーム実行される進行管理 ======
    void Update()
    {
        // --- アイテムパネルの開閉処理だけ分かりやすく抜粋 ---
        if (!isItemPanelOpen && Input.GetKeyDown(KeyCode.A))
        {
            // Aでパネルを開く（ON）
            if (itemDisplayPanel != null) itemDisplayPanel.SetActive(true);
            if (playerController != null) playerController.enabled = false;
            foreach (var enemy in enemyControllers) enemy.enabled = false;
            boss = FindObjectOfType<BossSimpleJump>(); // Bossも管理したい場合
            if (timeCnt != null) timeCnt.enabled = false;
            isItemPanelOpen = true;
        }
        else if (isItemPanelOpen && Input.GetKeyDown(KeyCode.X))
        {
            // Xで閉じる（OFF）
            if (itemDisplayPanel != null) itemDisplayPanel.SetActive(false);
            if (playerController != null) playerController.enabled = true;
            foreach (var enemy in enemyControllers) enemy.enabled = true;
            if (boss != null) boss.enabled = true;
            if (timeCnt != null) timeCnt.enabled = true;
            isItemPanelOpen = false;
        }

        // パネル開いてる間は他の処理はスキップ
        if (isItemPanelOpen) return;

        // --- ゲームクリア時の処理 ---
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);           // クリア画像を表示
            panel.SetActive(true);               // パネル（ボタン等）を表示

            restartButton.GetComponent<Button>().interactable = false; // クリア時はリスタート禁止
            mainImage.GetComponent<Image>().sprite = gameClearSpr;     // 画像をクリア画像に切り替え
            PlayerController.gameState = "gameend";                    // 状態を「終了」に

            // 残り時間をスコアに加算
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 時間カウント停止
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;   // 残り時間×10点で加算
            }

            // ステージスコアも加算しリセット
            totalScore += stageScore;
            stageScore = 0;

            UpdateScore(); // スコアUI更新
        }
        // --- ゲームオーバー時の処理 ---
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            nextButton.GetComponent<Button>().interactable = false; // ゲームオーバー時は次に進めない
            mainImage.GetComponent<Image>().sprite = gameOverSpr;   // 画像をゲームオーバー画像に切り替え
            PlayerController.gameState = "gameend";

            if (timeCnt != null)
                timeCnt.isTimeOver = true; // 時間カウント停止
        }
        // --- 通常プレイ中の処理 ---
        else if (PlayerController.gameState == "playing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                PlayerController playerCnt = player.GetComponent<PlayerController>();

                // 残り時間の表示・時間切れ判定
                if (timeCnt != null && timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime;
                    timeText.text = time.ToString();

                    // 時間切れになったらゲームオーバー
                    if (time <= 0)
                    {
                        playerCnt.GameOver(); // PlayerControllerのGameOver関数を呼ぶ
                    }
                }

                // スコアアイテム取得などでスコアが増えていたら加算
                if (playerCnt.score != 0)
                {
                    stageScore += playerCnt.score;
                    playerCnt.score = 0;    // 一度加算したらリセット
                    UpdateScore();          // スコア表示更新
                }
            }
            else
            {
                Debug.LogWarning("Player が見つかっていません！");
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            itemDisplayPanel.SetActive(!itemDisplayPanel.activeSelf);
        }

    }

    // ====== メイン画像（結果画面）の一時非表示（ゲーム開始直後の演出用） ======
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // ====== 「次へ」ボタン押下時の処理 ======
    public void OnNextButton()
    {
        currentStage++; // ステージ番号を増やす

        if (currentStage == 2)
        {
            // Stage1 → Stage2 へ遷移
            SceneManager.LoadScene("BaseScene2");
        }
        else if (currentStage == 3)
        {
            // Stage2 → リザルト画面へ
            SceneManager.LoadScene("ResultScene");
        }
    }

    // ====== スコアUIの表示を更新する ======
    void UpdateScore()
    {
        int score = stageScore + totalScore;

        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // スコア表示
        }
        else
        {
            Debug.LogWarning("scoreText が Inspector でアサインされていません！");
        }
    }
}
