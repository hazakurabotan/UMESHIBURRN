using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ====================
// 1行ぶんの会話データをまとめるクラス
// ====================
[System.Serializable]
public class DialogLine
{
    public string speakerName;   // 発言者名（誰のセリフか）
    public Sprite speakerIcon;   // 発言者のアイコン画像（顔グラ等）
    public string text;          // セリフ本文
}

// ====================
// 会話ウィンドウを管理するクラス
// ====================
public class DialogManager : MonoBehaviour
{
    // --- UI部品（Inspectorでアサイン） ---
    public GameObject dialogPanel;         // 会話ウィンドウ全体のパネル
    public TextMeshProUGUI dialogText;     // セリフ本文表示用
    public TextMeshProUGUI nameText;       // 発言者名表示用
    public Image iconImage;                // 発言者アイコン用

    public TimeController timeController;

    // --- 会話データ ---
    public DialogLine[] dialogLines;       // 会話内容の配列（Inspectorで編集もOK）
    int currentSentence = 0;               // 今表示しているセリフの番号
    bool isTalking = false;                // 会話中かどうか

    // --- 会話終了後に表示・起動するもの ---
    public GameObject stage2WallLeft;            // 左側の壁（ボス戦開始用）
    public GameObject stage2WallRight;           // 右側の壁
    public GameObject BossHpBarPanel;         // ボスのHPバー
    public TextMeshProUGUI bossHPText;     // ボスHPの数字表示（未使用でもOK）

    // --- 毎フレーム呼ばれる ---
    void Update()
    {
        // 会話中のみZキーで次のセリフに進む
        if (isTalking && Input.GetKeyDown(KeyCode.Z))
        {
            NextSentence();
        }
    }

    void Start()
    {
        Debug.Log("【DialogManager Start】Scene内に存在するDialogManager一覧↓");
        foreach (var dm in FindObjectsOfType<DialogManager>())
        {
            Debug.Log("DialogManager: " + dm.gameObject.name);
        }

        var allDialogManagers = FindObjectsOfType<DialogManager>();
        Debug.Log($"【DialogManagerチェック】FindObjectsOfType<DialogManager>().Length = {allDialogManagers.Length}");
        foreach (var dm in allDialogManagers)
        {
            Debug.Log($"DialogManager名: {dm.name} / 親: {dm.transform.parent?.name}");
        }


        dialogPanel.SetActive(false); // シーン開始直後は非表示

        // 参照切れ対策: 名前で探す
        if (stage2WallLeft == null) stage2WallLeft = GameObject.Find("WallLeft");
        if (stage2WallRight == null) stage2WallRight = GameObject.Find("WallRight");
        if (BossHpBarPanel == null) BossHpBarPanel = GameObject.Find("BossHPPanel");

        if (timeController == null)
            timeController = FindObjectOfType<TimeController>();
    }



    // === 会話開始の関数（他スクリプトから呼ばれる） ===
    public void StartDialog(DialogLine[] lines)
    {
        // ★プレイヤーを完全停止させる
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {

            var rb = player.GetComponent<Rigidbody2D>();
            var pc = player.GetComponent<PlayerController>();
            if (rb != null) rb.velocity = Vector2.zero;   // 慣性ストップ
            if (pc != null) pc.enabled = false;           // プレイヤー操作を止める
        }

        if (timeController != null)
            timeController.enabled = false;

        dialogLines = lines;      // 会話データをセット
        currentSentence = 0;     // 最初の行から
        dialogPanel.SetActive(true); // 会話パネルを表示
        isTalking = true;        // 会話中フラグON
        ShowSentence();          // 最初のセリフ表示
    }

    // === 現在のセリフを画面に表示する ===
    void ShowSentence()
    {
        // まだセリフが残っていれば…
        if (currentSentence < dialogLines.Length)
        {
            var line = dialogLines[currentSentence];

            dialogText.text = line.text;                 // セリフ本文
            nameText.text = line.speakerName;            // 名前
            iconImage.sprite = line.speakerIcon;         // アイコン
            iconImage.enabled = (line.speakerIcon != null); // アイコンがあれば表示
        }
        else
        {
            EndDialog(); // 全セリフ終了→会話終了処理へ
        }
    }

    // === Zキーで次のセリフに進む ===
    void NextSentence()
    {
        currentSentence++;    // 番号を進めて
        ShowSentence();       // 次のセリフ表示
    }

    // === 会話終了処理 ===
    void EndDialog()
    {
        dialogPanel.SetActive(false);   // 会話ウィンドウを隠す
        isTalking = false;              // 会話中フラグOFF

        if (timeController != null)
            timeController.enabled = true;

        // 壁やボスHPバーを表示してボス戦開始準備
        if (stage2WallLeft != null) stage2WallLeft.SetActive(true);
        if (stage2WallRight != null) stage2WallRight.SetActive(true);
        if (BossHpBarPanel != null) BossHpBarPanel.SetActive(true);



        // ボスを動かす（isActiveをtrueにする）
        FindObjectOfType<BossSimpleJump>().isActive = true;
        // プレイヤー操作も再び有効化
        FindObjectOfType<PlayerController>().enabled = true;
    }
}


