using UnityEngine;
using TMPro;
using System.Collections;

// ������1�������\������^�C�v���C�^�[���̃e�L�X�g�\���X�N���v�g
public class TypeWriterText : MonoBehaviour
{
    public TextMeshProUGUI messageText;  // �\�����TextMeshProUGUI�iUI Text��Inspector�œo�^�j
    public float typeSpeed = 0.03f;      // 1�������\������X�s�[�h�i�b�j
    private string fullText;             // �\������S��
    private bool isTyping = false;       // ���^�C�v�����ǂ���
    private bool skip = false;           // Z�L�[�őS���\���ɃX�L�b�v����t���O

    // === �e�L�X�g���Z�b�g���\���J�n�i�O������Ăяo���p�j ===
    public void ShowText(string text)
    {
        StopAllCoroutines(); // �O��̃R���[�`�����c���Ă���Β�~
        fullText = text;
        StartCoroutine(TypeText()); // �������菈���J�n
    }

    // === 1�������\������R���[�`�� ===
    IEnumerator TypeText()
    {
        isTyping = true;
        messageText.text = ""; // �ŏ��͋�ɂ���
        foreach (char c in fullText)
        {
            messageText.text += c; // 1�����������Ă���
            if (skip) break;       // �X�L�b�v������S���\����
            yield return new WaitForSeconds(typeSpeed); // �w�莞�ԑ҂�
        }
        messageText.text = fullText; // �Ō�͕K���S���\��
        isTyping = false;
        skip = false; // �X�L�b�v�t���O���Z�b�g
    }

    // === Z�L�[�őS���\���ɃX�L�b�v���鏈�� ===
    void Update()
    {
        // �^�C�v����Z�L�[����������S���\��
        if (isTyping && Input.GetKeyDown(KeyCode.Z))
        {
            skip = true;
        }
    }
}
