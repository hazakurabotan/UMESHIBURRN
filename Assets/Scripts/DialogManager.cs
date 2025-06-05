using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; // Panel�ւ̎Q��
    public TextMeshProUGUI dialogText; // �e�L�X�g
    public string[] sentences; // �\����������b��
    int currentSentence = 0;
    bool isTalking = false;

    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Z)) // Z�L�[�Ői�߂��
        {
            NextSentence();
        }
    }

    // ������������StartDialog��1�c���I
    public void StartDialog(string[] lines)
    {
        // �v���C���[�����S��~������
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // �������x���[���ɁI
                rb.angularVelocity = 0f;    // ��]���~�߂����ꍇ
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
        // �����Łu�{�X�N���v�ȂǃC�x���g���s
        FindObjectOfType<BossSimpleJump>().isActive = true;
        FindObjectOfType<PlayerController>().enabled = true;
    }
}
