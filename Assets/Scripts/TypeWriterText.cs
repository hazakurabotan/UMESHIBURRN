using UnityEngine;
using TMPro;
using System.Collections;

// 文字を1文字ずつ表示するタイプライター風のテキスト表示スクリプト
public class TypeWriterText : MonoBehaviour
{
    public TextMeshProUGUI messageText;  // 表示先のTextMeshProUGUI（UI TextをInspectorで登録）
    public float typeSpeed = 0.03f;      // 1文字ずつ表示するスピード（秒）
    private string fullText;             // 表示する全文
    private bool isTyping = false;       // 今タイプ中かどうか
    private bool skip = false;           // Zキーで全文表示にスキップするフラグ

    // === テキストをセット＆表示開始（外部から呼び出す用） ===
    public void ShowText(string text)
    {
        StopAllCoroutines(); // 前回のコルーチンが残っていれば停止
        fullText = text;
        StartCoroutine(TypeText()); // 文字送り処理開始
    }

    // === 1文字ずつ表示するコルーチン ===
    IEnumerator TypeText()
    {
        isTyping = true;
        messageText.text = ""; // 最初は空にする
        foreach (char c in fullText)
        {
            messageText.text += c; // 1文字ずつ足していく
            if (skip) break;       // スキップしたら全文表示へ
            yield return new WaitForSeconds(typeSpeed); // 指定時間待つ
        }
        messageText.text = fullText; // 最後は必ず全文表示
        isTyping = false;
        skip = false; // スキップフラグリセット
    }

    // === Zキーで全文表示にスキップする処理 ===
    void Update()
    {
        // タイプ中にZキーを押したら全文表示
        if (isTyping && Input.GetKeyDown(KeyCode.Z))
        {
            skip = true;
        }
    }
}
