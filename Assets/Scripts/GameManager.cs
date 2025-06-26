using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // シーン切り替え用
using UnityEngine.UI;              // UI制御用
using TMPro;                       // TextMeshPro用

// ゲーム全体の状態・UI・スコア・アイテム管理などを担う
public class GameManager : MonoBehaviour
{

    // 復活演出関連
    public GameObject player;
    public Sprite normalSprite;
    public Sprite revivedSprite;

    public GameObject videoCanvas;         // 動画表示用Canvas
    public UnityEngine.Video.VideoPlayer videoPlayer;
    public float revivalChance = 0.4f;     // 復活確率

    private bool triedRevival = false;


    // --- シングルトンパターン用 ---
    public static GameManager Instance;

    // --- アイテム画像や状態の管理 ---
    public Sprite[] itemSprites;        // アイテム画像リスト
    public int equippedItemId = -1;     // 今装備中のアイテム番号

    // ==== UI関連 ====
    public GameObject mainImage;        // メイン画像（勝ち・負け等）
    public Sprite gameOverSpr;          // ゲームオーバー画像
    public Sprite gameClearSpr;         // クリア画像
    public GameObject panel;            // ボタンなどをまとめたパネル
    public GameObject restartButton;    // リスタート用ボタン
    public GameObject nextButton;       // ネクスト用ボタン
    public GameObject cutInImage;       // カットイン演出画像
    public AudioSource cutInAudioSource;// カットイン音用
    public AudioClip cutInVoiceClip;    // カットイン声
    public GameObject laserPrefab;      // レーザープレハブ

    // アイテムパネル関連
    public GameObject itemDisplayPanel;
    bool isItemPanelOpen = false;

    // ステージ番号
    public static int currentStage = 1;

    // タイマー関連
    public GameObject timeBar;
    public TextMeshProUGUI timeText;
    TimeController timeCnt; // タイマー制御用スクリプト

    // スコア管理
    public TextMeshProUGUI scoreText;
    public static int totalScore = 0; // 全体合計
    public int stageScore = 0;        // ステージごと

    // ---------------------- Awake ----------------------
    // シングルトン初期化＆リソースのロード（1回のみ実行）
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  // ゲーム中1個だけに
            DontDestroyOnLoad(this.gameObject); // シーンまたいでも消えない

            // --- アイテム画像（Resourcesフォルダ）読み込み ---
            List<Sprite> loaded = new List<Sprite>();
            for (int i = 0; i < 10; i++)
            {
                Sprite s = Resources.Load<Sprite>("ItemSprites/" + i);
                if (s != null) loaded.Add(s);
            }
            itemSprites = loaded.ToArray();
        }
        else
        {
            Destroy(gameObject); // 2個目以降は自動で削除
        }
    }

    // ---------------------- Start ----------------------
    void Start()
    {

        // --- 動画再生まわりを確実にOFF ---
        if (videoCanvas != null) videoCanvas.SetActive(false); // Canvas非表示
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
        }


        // ステージ1ならcurrentStageを1に初期化
        if (SceneManager.GetActiveScene().name == "Stage1") currentStage = 1;

        InactiveImage();         // mainImageを非表示
        panel.SetActive(false);  // ボタンパネル非表示

        // タイマー制御
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null && timeCnt.gameTime == 0.0f) timeBar.SetActive(false);

        // アイテムパネル非表示
        if (itemDisplayPanel != null) itemDisplayPanel.SetActive(false);
        UpdateScore();   // スコアUI初期化

        // ボタン類も非表示
        if (restartButton != null) restartButton.SetActive(false);
        if (nextButton != null) nextButton.SetActive(false);
    }

    // ---------------------- Update ----------------------
    void Update()
    {
        // --- タイマーのUI表示 ---
        if (timeCnt != null && timeText != null)
        {
            timeText.text = Mathf.CeilToInt(timeCnt.displayTime).ToString("D3");
        }

        // --- ゲーム進行状況に応じたUI制御 ---
        if (PlayerController.gameState == "gameclear")
        {
            // クリア時のボタン制御
            mainImage.SetActive(false);
            panel.SetActive(true);
            if (restartButton != null) restartButton.SetActive(false);
            if (nextButton != null) nextButton.SetActive(true);

            // タイマー停止＆スコア加算
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10; // タイムボーナス
            }
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

            // 状態遷移
            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "gameover" && !triedRevival)
        {
            // --- 復活判定処理 ---
            triedRevival = true;

            if (Random.value < revivalChance)
            {
                StartCoroutine(PlayRevivalSequence());
            }
            else
            {
                // 通常のゲームオーバー演出
                mainImage.SetActive(false);
                panel.SetActive(true);
                if (restartButton != null) restartButton.SetActive(true);
                if (nextButton != null) nextButton.SetActive(false);
                if (timeCnt != null) timeCnt.isTimeOver = true;

                PlayerController.gameState = "gameend";
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            // 既に復活試行済みでゲームオーバーのままなら通常UI表示
            mainImage.SetActive(false);
            panel.SetActive(true);
            if (restartButton != null) restartButton.SetActive(true);
            if (nextButton != null) nextButton.SetActive(false);
            if (timeCnt != null) timeCnt.isTimeOver = true;

            PlayerController.gameState = "gameend";
        }
        else if (PlayerController.gameState == "playing")
        {
            // プレイヤーのスコア加算処理
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerController playerCnt = player.GetComponent<PlayerController>();
                if (playerCnt.score != 0)
                {
                    stageScore += playerCnt.score;
                    playerCnt.score = 0;
                    UpdateScore();
                }
            }
        }

        // --- アイテムパネルの開閉 ---
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (itemDisplayPanel != null)
            {
                itemDisplayPanel.SetActive(true);
                isItemPanelOpen = true;
            }
        }
        else if (isItemPanelOpen && Input.GetKeyDown(KeyCode.X))
        {
            if (itemDisplayPanel != null)
            {
                itemDisplayPanel.SetActive(false);
                isItemPanelOpen = false;
            }
        }

        // --- カットイン（Sキー） ---
        if (Input.GetKeyDown(KeyCode.S))
        {
            cutInImage.SetActive(true);
            if (cutInAudioSource != null && cutInVoiceClip != null)
            {
                cutInAudioSource.PlayOneShot(cutInVoiceClip);
            }
            Invoke(nameof(HideCutIn), 1.0f); // 1秒後にHideCutInを呼ぶ
        }
    }

    // --- カットイン画像を隠し、レーザー演出発射 ---
    void HideCutIn()
    {
        cutInImage.SetActive(false);
        FireLaser();
    }

    // --- プレイヤーの位置からレーザーを発射 ---
    void FireLaser()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        Vector3 pos = player.transform.position;
        float dir = player.transform.localScale.x;
        float length = 1.3f;
        GameObject laserObj = Instantiate(laserPrefab, pos + new Vector3(5f * dir, 0, 0), Quaternion.identity);
        Vector3 scale = laserObj.transform.localScale;
        scale.x = length;
        if (dir < 0) scale.x *= -1;
        laserObj.transform.localScale = scale;
        laserObj.transform.position += new Vector3((length / 2f) * dir, 0, 0);
        Destroy(laserObj, 0.6f); // 0.6秒で削除
    }

    // --- mainImageを隠すだけ ---
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // --- リスタートボタン処理 ---
    public void OnRestartButton()
    {
        triedRevival = false;

        // 1. UI全部非表示
        if (panel != null) panel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (nextButton != null) nextButton.SetActive(false);
        if (mainImage != null) mainImage.SetActive(false);

        // 2. タイマーリセット
        if (timeCnt != null)
        {
            timeCnt.ResetTimer();
        }

        // 3. シーン再読込でプレイヤーなどもリセット
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // --- ネクストボタン処理（次ステージへ） ---
    public void OnNextButton()
    {
        if (panel != null) panel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (nextButton != null) nextButton.SetActive(false);
        if (mainImage != null) mainImage.SetActive(false);

        // タイマーリセット
        if (timeCnt != null)
        {
            timeCnt.ResetTimer();
        }

        currentStage++;
        if (currentStage == 2)
            SceneManager.LoadScene("BaseScene2");
        else if (currentStage == 3)
            SceneManager.LoadScene("ResultScene");
    }

    // --- スコアUIの更新 ---
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        if (scoreText != null) scoreText.text = score.ToString();
    }

    // --- 今装備してるアイテムのスプライトを取得 ---
    public Sprite GetEquippedSprite()
    {
        if (itemSprites != null && equippedItemId >= 0 && equippedItemId < itemSprites.Length)
            return itemSprites[equippedItemId];
        return null;
    }

    // --- アイテムパネルが開いているかどうかを返す ---
    public bool IsItemPanelOpen()
    {
        return itemDisplayPanel != null && itemDisplayPanel.activeSelf;
    }

    // --- シーン切替時のイベント登録 ---
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // --- シーン切替時にUI等をリセット ---
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // アイテムパネルなど全部非表示
        if (itemDisplayPanel != null)
            itemDisplayPanel.SetActive(false);
        isItemPanelOpen = false;
        if (panel != null) panel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (nextButton != null) nextButton.SetActive(false);
        if (mainImage != null) mainImage.SetActive(false);

        // タイマーのリセット
        if (timeCnt == null)
            timeCnt = GetComponent<TimeController>();

        if (timeCnt != null)
        {
            timeCnt.ResetTimer();
            timeCnt.isTimeOver = false;
            // ショップシーンのみタイマーを止める（例：scene名に"Shop"が入ってたら）
            if (scene.name.Contains("Shop"))
                timeCnt.enabled = false;
            else
                timeCnt.enabled = true;
        }
    }

    IEnumerator PlayRevivalSequence()
    {
        if (player != null) player.SetActive(false);

        if (videoCanvas != null) videoCanvas.SetActive(true);
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared) yield return null;
            videoPlayer.Play();
        }

        while (videoPlayer != null && videoPlayer.isPlaying)
            yield return null;

        if (videoCanvas != null) videoCanvas.SetActive(false);

        if (player != null)
        {
            // プレイヤーの位置をセット
            player.transform.position = new Vector3(-8.97f, -0.29f, 0f);

            // 一旦無効→次のフレームで有効にして物理演算を安定させる
            yield return new WaitForFixedUpdate(); // ← これがポイント
            player.SetActive(true);

            // スプライト差し替え
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null && revivedSprite != null)
                sr.sprite = revivedSprite;
        }

        // 状態復帰
        if (timeCnt != null) timeCnt.isTimeOver = false;
        PlayerController.gameState = "playing";
    }



}