using UnityEngine;
using TMPro;
using System.Collections;

// ������1�������\������^�C�v���C�^�[���̃e�L�X�g�\���X�N���v�g
public class TypeWriterText : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float typeSpeed = 0.03f;

    private string fullText;
    private bool isTyping = false;
    private bool skip = false;

    public bool isCompleted { get; private set; } // ���S���\���ς݂��ǂ���

    public void ShowText(string text)
    {
        StopAllCoroutines();
        fullText = text;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        isCompleted = false;   // �ǉ�: �J�n���͂܂�����
        messageText.text = "";
        foreach (char c in fullText)
        {
            messageText.text += c;
            if (skip) break;
            yield return new WaitForSeconds(typeSpeed);
        }
        messageText.text = fullText;
        isTyping = false;
        isCompleted = true;    // �ǉ�: �S���\��������true
        skip = false;
    }

    void Update()
    {
        if (isTyping && Input.GetKeyDown(KeyCode.Z))
        {
            skip = true;
        }
    }
}