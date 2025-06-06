using UnityEngine;
using TMPro;
using System.Collections;

public class TypeWriterText : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float typeSpeed = 0.03f; // 一文字表示速度
    private string fullText;
    private bool isTyping = false;
    private bool skip = false;

    // 文字列セット＋開始
    public void ShowText(string text)
    {
        StopAllCoroutines();
        fullText = text;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        messageText.text = "";
        foreach (char c in fullText)
        {
            messageText.text += c;
            if (skip) break;
            yield return new WaitForSeconds(typeSpeed);
        }
        messageText.text = fullText;
        isTyping = false;
        skip = false;
    }

    // Zキーでスキップ
    void Update()
    {
        if (isTyping && Input.GetKeyDown(KeyCode.Z))
        {
            skip = true;
        }
    }
}
