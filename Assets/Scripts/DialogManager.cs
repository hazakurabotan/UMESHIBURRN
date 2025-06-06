using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// 1�s�Ԃ�̏����܂Ƃ߂�N���X
[System.Serializable]
public class DialogLine
{
    public string speakerName;   // �����Җ�
    public Sprite speakerIcon;   // �A�C�R���摜
    public string text;          // �Z���t�{��
}

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;
    public Image iconImage;

    public DialogLine[] dialogLines;  // Inspector�ŕҏW
    int currentSentence = 0;
    bool isTalking = false;

    public GameObject wallLeft;
    public GameObject wallRight;
    public GameObject bossHPPanel;
    public TextMeshProUGUI bossHPText;

    void Update()
    {
        // ��b���̂�Z�L�[�Ŏ��̃Z���t
        if (isTalking && Input.GetKeyDown(KeyCode.Z))
        {
            NextSentence();
        }
    }

    // ���������\�b�h�錾�I�I
    public void StartDialog(DialogLine[] lines)
    {
        // �������Ńv���C���[���~�߂�
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            var pc = player.GetComponent<PlayerController>();
            if (rb != null) rb.velocity = Vector2.zero;   // ���������S�X�g�b�v�I
            if (pc != null) pc.enabled = false;           // �R���g���[���[���~�߂�
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
