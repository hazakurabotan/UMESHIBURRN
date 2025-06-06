using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 1行ぶんの情報をまとめるクラス
[System.Serializable]
public class DialogLine
{
    public string speakerName;   // 発言者名
    public Sprite speakerIcon;   // アイコン画像
    public string text;          // セリフ本文
}

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;
    public Image iconImage;

    public DialogLine[] dialogLines;  // Inspectorで編集
    int currentSentence = 0;
    bool isTalking = false;

    public GameObject wallLeft;
    public GameObject wallRight;
    public GameObject bossHPPanel;
    public TextMeshProUGUI bossHPText;

    void Update()
    {
        // 会話中のみZキーで次のセリフ
        if (isTalking && Input.GetKeyDown(KeyCode.Z))
        {
            NextSentence();
        }
    }

    // ここがメソッド宣言！！
    public void StartDialog(DialogLine[] lines)
    {
        // ★ここでプレイヤーを止める
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            var pc = player.GetComponent<PlayerController>();
            if (rb != null) rb.velocity = Vector2.zero;   // 慣性も完全ストップ！
            if (pc != null) pc.enabled = false;           // コントローラーも止める
        }

        dialogLines = lines;
        currentSentence = 0;
        dialogPanel.SetActive(true);
        isTalking = true;
        ShowSentence();
    }

    void ShowSentence()
    {
        if (currentSentence < dialogLines.Length)
        {
            var line = dialogLines[currentSentence];

            dialogText.text = line.text;
            nameText.text = line.speakerName;
            iconImage.sprite = line.speakerIcon;
            iconImage.enabled = (line.speakerIcon != null);
        }
        else
        {
            EndDialog();
        }
    }

    void NextSentence()
    {
        currentSentence++;
        ShowSentence();
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        isTalking = false;

        if (wallLeft != null) wallLeft.SetActive(true);
        if (wallRight != null) wallRight.SetActive(true);
        if (bossHPPanel != null) bossHPPanel.SetActive(true);

        FindObjectOfType<BossSimpleJump>().isActive = true;
        FindObjectOfType<PlayerController>().enabled = true;
    }
}
