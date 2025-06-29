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

    public RuntimeAnimatorController revivedOverrideController;

    public static bool fromRestart = false;

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


    public int killCount = 0;                // 撃破数カウント
    public int bulletLevel = 1;              // 弾レベル（1なら3発、2なら4発）
    public GameObject levelUpPanel;          // レベルアップ用パネル（Canvas内のPanelを割当）
    public PlayerShoot playerShoot;          // プレイヤーの弾撃ちスクリプトへの参照

    private bool isReviving = false;

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
        else if (Instance != this)
        {
            Destroy(this.gameObject); // 2個目以降は自動で削除
        }
    }

    // ---------------------- Start ----------------------
    void Start()
    {
        ResetAllUI();

        StartCoroutine(InitAfterFrame());


        // --- 動画再生まわりを確実にOFF ---
        if (videoCanvas != null) videoCanvas.SetActive(false); // Canvas非表示
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
        }

        // 一時的にリスタートかどうかで復活処理を無効化
        if (fromRestart)
        {
            triedRevival = true;
            fromRestart = false; // 忘れずリセット！
        }

        //
        //if (player != null)
        //{
        //    PlayerController pc = player.GetComponent<PlayerController>();
        //    if (pc != null)
        //    {
        //        pc.Heal(pc.maxHP);
        //        pc.UpdateHpUI();
        //    }
        //}


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


        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);

    }


    IEnumerator InitAfterFrame()
    {
        yield return null;

        if (fromRestart)
        {
            triedRevival = true;
            fromRestart = false;

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj;
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.Heal(pc.maxHP);

                    // HPバーの参照を明示的にリセット
                    if (pc.hpBar == null)
                        pc.hpBar = FindObjectOfType<HpBarController>();

                    yield return null; // 1フレーム待つとUIリンクが安定することがある
                    pc.UpdateHpUI();

                    Debug.Log($"[RESTART] Player HP: {pc.currentHP} / Max: {pc.maxHP}, hpBar:{(pc.hpBar == null ? "NULL" : "OK")}");
                }
            }

            // 演出キャンバスをもう一度隠す（念のため）
            if (videoCanvas != null)
                videoCanvas.SetActive(false);

            if (videoPlayer != null)
            {
                videoPlayer.Stop();
                videoPlayer.frame = 0;
            }

            PlayerController.gameState = "playing";
        }
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
        else if (PlayerController.gameState == "gameover" && !triedRevival && !isReviving)
        {
            triedRevival = true;

            if (Random.value < revivalChance)
            {
                StartCoroutine(PlayRevivalSequence());
            }
            else
            {
                // 復活に失敗したときだけ表示！
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
            if (isReviving) return; // ★復活中なら何もしない！

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

        if (levelUpPanel != null && levelUpPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                CloseLevelUpPanel();
            }
        }


    }

    public void AddKill()
    {
        killCount++;
        if (killCount == 10 && bulletLevel == 1)
        {
            bulletLevel = 2;
            var playerShoot = player.GetComponent<PlayerShoot>();
            if (playerShoot != null)
            {
                playerShoot.maxShots = 4;
            }
            if (levelUpPanel != null)
                levelUpPanel.SetActive(true);

            // ★ゲーム停止
            Time.timeScale = 0f;
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
        ResetAllUI();

        fromRestart = true;

        triedRevival = true;

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
        ResetAllUI();

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
        ResetAllUI();

        triedRevival = false;
        isReviving = false;


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

        // プレイヤー取得して体力全回復
        if (fromRestart)
        {
            triedRevival = true;
            fromRestart = false;

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj;
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.hpBar = FindObjectOfType<HpBarController>();

                    pc.Heal(pc.maxHP);
                    pc.UpdateHpUI();

                    Debug.Log($"[Restart後] currentHP={pc.currentHP}, hpBar={(pc.hpBar == null ? "NULL" : "OK")}");
                }
            }

            // videoCanvas/videoPlayerの再取得（万一に備えて）
            if (videoCanvas == null)
                videoCanvas = GameObject.Find("videoCanvas"); // 名前は正確に

            videoCanvas?.SetActive(false);

            if (videoPlayer == null)
                videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();

            videoPlayer?.Stop();
            videoPlayer.frame = 0;

            // 明示的に gameState を playing にしておく（念のため）
            PlayerController.gameState = "playing";
        }

    }

    IEnumerator PlayRevivalSequence()
    {
        isReviving = true;

        // 1. 復活演出前にplayerとVideoCanvasを必ず再取得
        player = GameObject.FindGameObjectWithTag("Player");
        if (videoCanvas == null)
            videoCanvas = GameObject.Find("VideoCanvas");
        if (videoPlayer == null)
            videoPlayer = FindObjectOfType<UnityEngine.Video.VideoPlayer>();

        if (player == null)
        {
            Debug.LogError("復活演出時にplayerが見つからない！");
            yield break;
        }

        // 2. playerの物理とコライダーを停止・一時非表示
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Collider2D col = player.GetComponent<Collider2D>();
        if (rb != null) rb.simulated = false;
        if (col != null) col.enabled = false;
        player.SetActive(false);

        // 3. VideoCanvasを表示（ここから演出）
        if (videoCanvas != null) videoCanvas.SetActive(true);

        // 4. VideoPlayerで動画再生
        if (videoPlayer != null)
        {
            // 動画クリップが設定されてるか要確認
            if (videoPlayer.clip == null)
            {
                Debug.LogError("[復活] VideoPlayerのVideoClipが設定されていません！");
            }
            else
            {
                videoPlayer.Stop();
                videoPlayer.frame = 0;
                videoPlayer.Prepare();
                // 準備できるまで待つ
                while (!videoPlayer.isPrepared) yield return null;
                videoPlayer.Play();
                // 再生が終わるまで待つ
                while (videoPlayer.isPlaying) yield return null;
            }
        }
        else
        {
            Debug.LogError("[復活] VideoPlayerが見つかりません！");
        }

        // 5. 動画が終わったらVideoCanvasを隠す
        if (videoCanvas != null) videoCanvas.SetActive(false);

        // 6. キャラ復活処理
        player.transform.position = new Vector3(-8.97f, 0.0f, 0f); // 必要に応じて座標調整

        yield return new WaitForFixedUpdate();

        // 物理＆コライダー再有効化
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = true;
        }
        if (col != null) col.enabled = true;

        player.SetActive(true);
        yield return null;  // UI安定化のため

        // 復活時スプライト切替
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null && revivedSprite != null)
            sr.sprite = revivedSprite;

        if (timeCnt != null) timeCnt.isTimeOver = false;
        PlayerController.gameState = "playing";

        if (restartButton != null) restartButton.SetActive(false);

        // HP回復・UI更新
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Heal(pc.maxHP);
            pc.UpdateHpUI();
        }

        // アニメーションコントローラ切替
        var animator = player.GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("[復活] animatorがnullです！");
        if (revivedOverrideController == null)
            Debug.LogWarning("[復活] revivedOverrideControllerがnullです！");

        if (animator != null && revivedOverrideController != null)
        {
            Debug.Log("[復活] 切り替え前 Controller名: " + animator.runtimeAnimatorController?.name);
            animator.runtimeAnimatorController = revivedOverrideController;
            Debug.Log("[復活] 切り替え後 Controller名: " + animator.runtimeAnimatorController?.name);
            yield return null;
            Debug.Log("[復活] 1フレーム後 Controller名: " + animator.runtimeAnimatorController?.name);
            animator.Play("RePlayerMove");
        }
        else
        {
            Debug.LogWarning("[復活] animator or revivedOverrideController がnullなので切り替えスキップ！");
        }

        isReviving = false;
    }

    void ResetAllUI()
    {
        if (videoCanvas != null) videoCanvas.SetActive(false);
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.frame = 0;
        }
        if (mainImage != null) mainImage.SetActive(false);
        if (panel != null) panel.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (nextButton != null) nextButton.SetActive(false);
        // ...他にもリセットしたいUIがあればここで追加
    }

    public void CloseLevelUpPanel()
    {
        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);

        // ★ゲーム再開
        Time.timeScale = 1f;
    }



}