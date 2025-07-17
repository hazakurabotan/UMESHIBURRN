using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// ====================
// 1�s�Ԃ�̉�b�f�[�^���܂Ƃ߂�N���X
// ====================
[System.Serializable]
public class DialogLine
{
    public string speakerName;   // �����Җ��i�N�̃Z���t���j
    public Sprite speakerIcon;   // �����҂̃A�C�R���摜�i��O�����j
    public string text;          // �Z���t�{��
}

// ====================
// ��b�E�B���h�E���Ǘ�����N���X
// ====================
public class DialogManager : MonoBehaviour
{
    // --- UI���i�iInspector�ŃA�T�C���j ---
    public GameObject dialogPanel;         // ��b�E�B���h�E�S�̂̃p�l��
    public TextMeshProUGUI dialogText;     // �Z���t�{���\���p
    public TextMeshProUGUI nameText;       // �����Җ��\���p
    public Image iconImage;                // �����҃A�C�R���p

    public TimeController timeController;

    // --- ��b�f�[�^ ---
    public DialogLine[] dialogLines;       // ��b���e�̔z��iInspector�ŕҏW��OK�j
    int currentSentence = 0;               // ���\�����Ă���Z���t�̔ԍ�
    bool isTalking = false;                // ��b�����ǂ���

    // --- ��b�I����ɕ\���E�N��������� ---
    public GameObject stage2WallLeft;            // �����̕ǁi�{�X��J�n�p�j
    public GameObject stage2WallRight;           // �E���̕�
    public GameObject BossHpBarPanel;         // �{�X��HP�o�[
    public TextMeshProUGUI bossHPText;     // �{�XHP�̐����\���i���g�p�ł�OK�j

    // --- ���t���[���Ă΂�� ---
    void Update()
    {
        // ��b���̂�Z�L�[�Ŏ��̃Z���t�ɐi��
        if (isTalking && Input.GetKeyDown(KeyCode.Z))
        {
            NextSentence();
        }
    }

    void Start()
    {
        Debug.Log("�yDialogManager Start�zScene���ɑ��݂���DialogManager�ꗗ��");
        foreach (var dm in FindObjectsOfType<DialogManager>())
        {
            Debug.Log("DialogManager: " + dm.gameObject.name);
        }

        var allDialogManagers = FindObjectsOfType<DialogManager>();
        Debug.Log($"�yDialogManager�`�F�b�N�zFindObjectsOfType<DialogManager>().Length = {allDialogManagers.Length}");
        foreach (var dm in allDialogManagers)
        {
            Debug.Log($"DialogManager��: {dm.name} / �e: {dm.transform.parent?.name}");
        }


        dialogPanel.SetActive(false); // �V�[���J�n����͔�\��

        // �Q�Ɛ؂�΍�: ���O�ŒT��
        if (stage2WallLeft == null) stage2WallLeft = GameObject.Find("WallLeft");
        if (stage2WallRight == null) stage2WallRight = GameObject.Find("WallRight");
        if (BossHpBarPanel == null) BossHpBarPanel = GameObject.Find("BossHPPanel");

        if (timeController == null)
            timeController = FindObjectOfType<TimeController>();
    }



    // === ��b�J�n�̊֐��i���X�N���v�g����Ă΂��j ===
    public void StartDialog(DialogLine[] lines)
    {
        // ���v���C���[�����S��~������
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {

            var rb = player.GetComponent<Rigidbody2D>();
            var pc = player.GetComponent<PlayerController>();
            if (rb != null) rb.velocity = Vector2.zero;   // �����X�g�b�v
            if (pc != null) pc.enabled = false;           // �v���C���[������~�߂�
        }

        if (timeController != null)
            timeController.enabled = false;

        dialogLines = lines;      // ��b�f�[�^���Z�b�g
        currentSentence = 0;     // �ŏ��̍s����
        dialogPanel.SetActive(true); // ��b�p�l����\��
        isTalking = true;        // ��b���t���OON
        ShowSentence();          // �ŏ��̃Z���t�\��
    }

    // === ���݂̃Z���t����ʂɕ\������ ===
    void ShowSentence()
    {
        // �܂��Z���t���c���Ă���΁c
        if (currentSentence < dialogLines.Length)
        {
            var line = dialogLines[currentSentence];

            dialogText.text = line.text;                 // �Z���t�{��
            nameText.text = line.speakerName;            // ���O
            iconImage.sprite = line.speakerIcon;         // �A�C�R��
            iconImage.enabled = (line.speakerIcon != null); // �A�C�R��������Ε\��
        }
        else
        {
            EndDialog(); // �S�Z���t�I������b�I��������
        }
    }

    // === Z�L�[�Ŏ��̃Z���t�ɐi�� ===
    void NextSentence()
    {
        currentSentence++;    // �ԍ���i�߂�
        ShowSentence();       // ���̃Z���t�\��
    }

    // === ��b�I������ ===
    void EndDialog()
    {
        dialogPanel.SetActive(false);   // ��b�E�B���h�E���B��
        isTalking = false;              // ��b���t���OOFF

        if (timeController != null)
            timeController.enabled = true;

        // �ǂ�{�XHP�o�[��\�����ă{�X��J�n����
        if (stage2WallLeft != null) stage2WallLeft.SetActive(true);
        if (stage2WallRight != null) stage2WallRight.SetActive(true);
        if (BossHpBarPanel != null) BossHpBarPanel.SetActive(true);



        // �{�X�𓮂����iisActive��true�ɂ���j
        FindObjectOfType<BossSimpleJump>().isActive = true;
        // �v���C���[������ĂїL����
        FindObjectOfType<PlayerController>().enabled = true;
    }
}


