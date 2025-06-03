using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ==== UI�֘A ====
    public GameObject mainImage;              // �Q�[�����ʉ摜�i�����E�����j
    public Sprite gameOverSpr;               // �Q�[���I�[�o�[�摜
    public Sprite gameClearSpr;              // �Q�[���N���A�摜
    public GameObject panel;                 // �{�^���ނ�\������p�l��
    public GameObject restartButton;         // ���X�^�[�g�{�^��
    public GameObject nextButton;            // ���̃X�e�[�W�֐i�ރ{�^��

    // ==== �X�e�[�W�J�� ====
    public static int currentStage = 1;      // ���݂̃X�e�[�W�ԍ��istatic�ŋ��L�j

    // ==== ���Ԋ֘A ====
    public GameObject timeBar;               // ���ԃo�[�i�\����ON/OFF����j
    public TextMeshProUGUI timeText;         // �c�莞�Ԃ̐����\��
    TimeController timeCnt;                  // TimeController�X�N���v�g�Q��

    // ==== �X�R�A�֘A ====
    public TextMeshProUGUI scoreText;        // �X�R�A�\���p�e�L�X�g
    public static int totalScore = 0;        // �S�̃X�R�A�i�X�e�[�W���܂����ŋL�^�j
    public int stageScore = 0;               // �X�e�[�W���X�R�A�i�A�C�e���擾�Ȃǁj

    void Start()
    {
        // �ŏ��̃V�[�������ŃX�e�[�W�ԍ���������
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            currentStage = 1;
        }

        // ���C���摜���ꎞ��\���ɂ��āA�p�l�����B��
        Invoke("InactiveImage", 1.0f);
        panel.SetActive(false);

        // ���Ԑ���X�N���v�g�̎擾
        timeCnt = GetComponent<TimeController>();

        // �Q�[�����Ԃ�0�Ȃ玞�ԃo�[���\���ɂ���
        if (timeCnt != null && timeCnt.gameTime == 0.0f)
        {
            timeBar.SetActive(false);
        }

        UpdateScore(); // �X�R�A�����\��
    }

    void Update()
    {
        // === �Q�[���N���A���� ===
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            // �N���A���̓��X�^�[�g�֎~
            restartButton.GetComponent<Button>().interactable = false;

            // �N���A�摜�ɐ؂�ւ�
            mainImage.GetComponent<Image>().sprite = gameClearSpr;

            // �Q�[����Ԃ��I���ɐݒ�
            PlayerController.gameState = "gameend";

            // �c�莞�Ԃ��X�R�A���Z
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;
            }

            // �X�e�[�W�X�R�A�����v�ɉ��Z
            totalScore += stageScore;
            stageScore = 0;

            UpdateScore(); // �X�R�AUI�X�V
        }

        // === �Q�[���I�[�o�[���� ===
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            // �Q�[���I�[�o�[���͎��ɐi�߂Ȃ�
            nextButton.GetComponent<Button>().interactable = false;

            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            if (timeCnt != null)
                timeCnt.isTimeOver = true;
        }

        // === �ʏ�v���C�� ===
        else if (PlayerController.gameState == "playing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                PlayerController playerCnt = player.GetComponent<PlayerController>();

                // ���ԃJ�E���g���\��
                if (timeCnt != null && timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime;
                    timeText.text = time.ToString();

                    if (time <= 0)
                    {
                        playerCnt.GameOver(); // ���Ԑ؂�ŋ����s�k
                    }
                }

                // �X�R�A���Z�i�X�R�A�A�C�e���擾�Ȃǁj
                if (playerCnt.score != 0)
                {
                    stageScore += playerCnt.score;
                    playerCnt.score = 0;
                    UpdateScore();
                }
            }
            else
            {
                Debug.LogWarning("Player ���������Ă��܂���I");
            }
        }
    }

    // ���C���摜���\���ɂ���i�J�n�����1�b�x�点�Ď��s�j
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // �u���ցv�{�^���������̏���
    public void OnNextButton()
    {
        currentStage++;

        if (currentStage == 2)
        {
            // Stage1 �� Stage2 ��
            SceneManager.LoadScene("BaseScene2");
        }
        else if (currentStage == 3)
        {
            // Stage2 �� ���U���g��ʂ�
            SceneManager.LoadScene("ResultScene");
        }
    }

    // �X�R�A�\�����X�V
    void UpdateScore()
    {
        int score = stageScore + totalScore;

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogWarning("scoreText �� Inspector �ŃA�T�C������Ă��܂���I");
        }
    }
}
