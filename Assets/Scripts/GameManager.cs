using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// �Q�[���S�̂̐i�s�E�Ǘ���S������N���X
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // �V���O���g���p

    public Sprite[] itemSprites;        // �A�C�e���X�v���C�g�ꗗ�i�S�V�[���ŋ��ʁj
    public int equippedItemId = -1;     // ���݂̑���ID�i-1�͖������j

    // ==== UI�֘A ====
    public GameObject mainImage;              // ���ʕ\���p�̉摜�i�����E������ʂȂǁj
    public Sprite gameOverSpr;                // �Q�[���I�[�o�[���ɕ\������摜
    public Sprite gameClearSpr;               // �Q�[���N���A���ɕ\������摜
    public GameObject panel;                  // �{�^����UI���܂Ƃ߂��p�l��
    public GameObject restartButton;          // ���X�^�[�g�i��蒼���j�{�^��
    public GameObject nextButton;             // ���̃X�e�[�W�֐i�ރ{�^��
    public GameObject cutInImage;
    public AudioSource cutInAudioSource; // �J�b�g�C���p��AudioSource
    public AudioClip cutInVoiceClip;     // �J�b�g�C�����ɗ����{�C�X
    public GameObject laserPrefab;
    public GameObject HpBarPanel;


    //�A�C�e���p�l���֘A
    public GameObject itemDisplayPanel;       //�Q�[������A�L�[�ŃA�C�e���p�l���̕\��/��\���v
    bool isItemPanelOpen = false;
    PlayerController playerController;
    EnemyController[] enemyControllers; // �G�X�N���v�g���͍��킹��
    BossSimpleJump[] BossControllers; // �G�X�N���v�g���͍��킹��
    BossSimpleJump boss; // ���ǉ�

    // ==== �X�e�[�W�J�� ====
    public static int currentStage = 1;      // ���݂̃X�e�[�W�ԍ��i�S�̂ŋ��L�j

    // ==== ���Ԋ֘A ====
    public GameObject timeBar;               // ���ԃo�[�i���Ԃ�����ꍇ�����\���j
    public TextMeshProUGUI timeText;         // �c�莞�Ԃ̐����\��
    TimeController timeCnt;                  // ���ԊǗ��p�X�N���v�g�i�����I�u�W�F�N�g�ɃA�^�b�`�j

    // ==== �X�R�A�֘A ====
    public TextMeshProUGUI scoreText;        // �X�R�A�\���p�e�L�X�g
    public static int totalScore = 0;        // �����X�R�A�i�X�e�[�W���܂����ň����p���j
    public int stageScore = 0;               // �X�e�[�W���Ƃ̃X�R�A�i�A�C�e���擾���ŉ��Z�j

    // ====== ���������� ======

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            List<Sprite> loaded = new List<Sprite>();
            for (int i = 0; i < 10; i++)
            {
                Sprite s = Resources.Load<Sprite>("ItemSprites/" + i);
                if (s != null)
                {
                    loaded.Add(s);
                    Debug.Log("�ǂݍ��ݐ���: " + i + " = " + s.name);
                }
                else
                {
                    Debug.LogWarning("�ǂݍ��ݎ��s: " + i);
                }
            }
            itemSprites = loaded.ToArray();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // �ŏ��̃V�[���i"Stage1"�j�Ȃ�X�e�[�W�ԍ���1�Ƀ��Z�b�g
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            currentStage = 1;
        }

        // �ŏ��̓��U���g�摜���ꎞ��\���A�p�l�����B��
        Invoke("InactiveImage", 1.0f);
        panel.SetActive(false);

        // ���ԊǗ��X�N���v�g�擾�i����GameObject�ɃA�^�b�`���Ă������ƁI�j
        timeCnt = GetComponent<TimeController>();

        // ���Ԑ�����������Ύ��ԃo�[���̂��\��
        if (timeCnt != null && timeCnt.gameTime == 0.0f)
        {
            timeBar.SetActive(false);
        }

        if (itemDisplayPanel != null) itemDisplayPanel.SetActive(false);

        // �X�R�AUI�������\��
        UpdateScore();

        playerController = FindObjectOfType<PlayerController>();
        enemyControllers = FindObjectsOfType<EnemyController>(); // �G���Ǘ��������ꍇ
        boss = FindObjectOfType<BossSimpleJump>(); // Boss���Ǘ��������ꍇ

    }

    // ====== ���t���[�����s�����i�s�Ǘ� ======
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // ==== BaseScene��p�F�A�C�e���p�l���̊J���� ====
        if (sceneName.Contains("Stage2")) // Stage2 �Ȃǂɂ��Ή�
        {
            if (!isItemPanelOpen && Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (itemDisplayPanel != null) itemDisplayPanel.SetActive(true);
                if (playerController != null) playerController.enabled = false;
                foreach (var enemy in enemyControllers) enemy.enabled = false;
                boss = FindObjectOfType<BossSimpleJump>();
                if (boss != null) boss.enabled = false;
                if (timeCnt != null) timeCnt.enabled = false;
                isItemPanelOpen = true;
            }
            else if (isItemPanelOpen && Input.GetKeyDown(KeyCode.X))
            {
                if (itemDisplayPanel != null) itemDisplayPanel.SetActive(false);
                if (playerController != null) playerController.enabled = true;
                foreach (var enemy in enemyControllers) enemy.enabled = true;
                if (boss != null) boss.enabled = true;
                if (timeCnt != null) timeCnt.enabled = true;
                isItemPanelOpen = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                itemDisplayPanel.SetActive(!itemDisplayPanel.activeSelf);

                if (itemDisplayPanel.activeSelf)
                {
                    ItemDisplayManager display = FindObjectOfType<ItemDisplayManager>();
                    if (display != null)
                    {
                        display.RefreshDisplay(); // ������ǉ��I
                    }
                }
            }

            if (isItemPanelOpen) return;
        }


        // --- �Q�[���N���A���̏��� ---
        if (PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);           // �N���A�摜��\��
            panel.SetActive(true);               // �p�l���i�{�^�����j��\��

            restartButton.GetComponent<Button>().interactable = false; // �N���A���̓��X�^�[�g�֎~
            mainImage.GetComponent<Image>().sprite = gameClearSpr;     // �摜���N���A�摜�ɐ؂�ւ�
            PlayerController.gameState = "gameend";                    // ��Ԃ��u�I���v��

            // �c�莞�Ԃ��X�R�A�ɉ��Z
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // ���ԃJ�E���g��~
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;   // �c�莞�ԁ~10�_�ŉ��Z
            }

            // �X�e�[�W�X�R�A�����Z�����Z�b�g
            totalScore += stageScore;
            stageScore = 0;

            UpdateScore(); // �X�R�AUI�X�V
        }
        // --- �Q�[���I�[�o�[���̏��� ---
        else if (PlayerController.gameState == "gameover")
        {
            mainImage.SetActive(true);
            panel.SetActive(true);

            nextButton.GetComponent<Button>().interactable = false; // �Q�[���I�[�o�[���͎��ɐi�߂Ȃ�
            mainImage.GetComponent<Image>().sprite = gameOverSpr;   // �摜���Q�[���I�[�o�[�摜�ɐ؂�ւ�
            PlayerController.gameState = "gameend";

            if (timeCnt != null)
                timeCnt.isTimeOver = true; // ���ԃJ�E���g��~
        }
        // --- �ʏ�v���C���̏��� ---
        else if (PlayerController.gameState == "playing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                PlayerController playerCnt = player.GetComponent<PlayerController>();

                // �c�莞�Ԃ̕\���E���Ԑ؂ꔻ��
                if (timeCnt != null && timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime;
                    timeText.text = time.ToString();

                    // ���Ԑ؂�ɂȂ�����Q�[���I�[�o�[
                    if (time <= 0)
                    {
                        playerCnt.GameOver(); // PlayerController��GameOver�֐����Ă�
                    }
                }

                // �X�R�A�A�C�e���擾�ȂǂŃX�R�A�������Ă�������Z
                if (playerCnt.score != 0)
                {
                    stageScore += playerCnt.score;
                    playerCnt.score = 0;    // ��x���Z�����烊�Z�b�g
                    UpdateScore();          // �X�R�A�\���X�V
                }
            }
            else
            {
                Debug.LogWarning("Player ���������Ă��܂���I");
            }
        }

        // �J�b�g�C��
        if (Input.GetKeyDown(KeyCode.S))
        {
            cutInImage.SetActive(true);
            if (cutInAudioSource != null && cutInVoiceClip != null)
            {
                cutInAudioSource.PlayOneShot(cutInVoiceClip);
            }
            Invoke(nameof(HideCutIn), 1.0f);
        }




    }
    void HideCutIn()
    {
        cutInImage.SetActive(false);

        // ���[�U�[�𔭎�
        FireLaser();
    }

    void FireLaser()
    {
        // �v���C���[�擾
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 pos = player.transform.position ;
        float dir = player.transform.localScale.x;

        // ���[���h���W��900�s�N�Z�����̒����iPPU=100�z��j
        float length = 1.3f;

        // ���[�U�[����
        GameObject laserObj = Instantiate(laserPrefab, pos + new Vector3(5f * dir, 0, 0), Quaternion.identity);


        // �X�P�[������
        Vector3 scale = laserObj.transform.localScale;
        scale.x = length;
        if (dir < 0) scale.x *= -1;
        laserObj.transform.localScale = scale;

        // ���Spivot�Ȃ̂ŁA�O���ɂ����o�������ꍇ�́u�����̔����v�����O��
        laserObj.transform.position += new Vector3((length / 2f) * dir, 0, 0);

        Destroy(laserObj, 0.6f);
    }


    // ====== ���C���摜�i���ʉ�ʁj�̈ꎞ��\���i�Q�[���J�n����̉��o�p�j ======
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // ====== �u���ցv�{�^���������̏��� ======
    public void OnNextButton()
    {
        currentStage++; // �X�e�[�W�ԍ��𑝂₷

        if (currentStage == 2)
        {
            // Stage1 �� Stage2 �֑J��
            SceneManager.LoadScene("BaseScene2");
        }
        else if (currentStage == 3)
        {
            // Stage2 �� ���U���g��ʂ�
            SceneManager.LoadScene("ResultScene");
        }
    }

    // ====== �X�R�AUI�̕\�����X�V���� ======
    void UpdateScore()
    {
        int score = stageScore + totalScore;

        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // �X�R�A�\��
        }
        else
        {
            Debug.LogWarning("scoreText �� Inspector �ŃA�T�C������Ă��܂���I");
        }
    }

    public Sprite GetEquippedSprite()
    {
        if (itemSprites != null && equippedItemId >= 0 && equippedItemId < itemSprites.Length)
        {
            return itemSprites[equippedItemId];
        }
        return null;
    }

    public bool IsItemPanelOpen()
    {
        return itemDisplayPanel != null && itemDisplayPanel.activeSelf;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (itemDisplayPanel != null)
            itemDisplayPanel.SetActive(false);
        isItemPanelOpen = false;

        // HP�o�[�̐���
        if (HpBarPanel == null)
        {
            HpBarPanel = GameObject.Find("HpBarPanel");
        }
        if (HpBarPanel != null)
        {
            if (scene.name.Contains("Stage") || scene.name.Contains("BaseScene"))
                HpBarPanel.SetActive(true);
            else
                HpBarPanel.SetActive(false);
        }

        // �^�C�}�[����FShop�V�[���ł͒�~�A����ȊO�ł͍ĊJ
        if (timeCnt == null)
        {
            timeCnt = GetComponent<TimeController>();
        }
        if (timeCnt != null)
        {
            if (scene.name.Contains("Shop"))
            {
                Debug.Log("Shop�V�[���Ȃ̂Ń^�C����~");
                timeCnt.enabled = false;
            }
            else
            {
                Debug.Log("�ʏ�V�[���Ȃ̂Ń^�C���ĊJ");
                timeCnt.enabled = true;
            }
        }
    }




}
