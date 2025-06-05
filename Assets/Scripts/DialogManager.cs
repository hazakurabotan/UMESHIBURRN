using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; // Panelへの参照
    public TextMeshProUGUI dialogText; // テキスト
    public string[] sentences; // 表示したい会話文
    int currentSentence = 0;
    bool isTalking = false;
    public GameObject wallLeft;
    public GameObject wallRight;
    public GameObject bossHPPanel; // HP表示UIパネル
    public TextMeshProUGUI bossHPText;


    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Z)) // Zキーで進める例
        {
            NextSentence();
        }
    }

    // ←ここだけにStartDialogを1つ残す！
    public void StartDialog(string[] lines)
    {
        // プレイヤーを完全停止させる
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // 物理速度をゼロに！
                rb.angularVelocity = 0f;    // 回転も止めたい場合
            }
        }

        sentences = lines;
        currentSentence = 0;
        dialogPanel.SetActive(true);
        isTalking = true;
        ShowSentence();
    }

    void ShowSentence()
    {
        if (currentSentence < sentences.Length)
        {
            dialogText.text = sentences[currentSentence];
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

        // 壁を出す
        if (wallLeft != null) wallLeft.SetActive(true);
        if (wallRight != null) wallRight.SetActive(true);

        // ボスHPパネルを表示
        if (bossHPPanel != null) bossHPPanel.SetActive(true);

        FindObjectOfType<BossSimpleJump>().isActive = true;
        FindObjectOfType<PlayerController>().enabled = true;
    }

}
