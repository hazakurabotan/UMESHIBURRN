using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Sprite[] itemSprites;
    public int equippedItemId = -1;

    // ==== UI関連 ====
    public GameObject mainImage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;
    public GameObject cutInImage;
    public AudioSource cutInAudioSource;
    public AudioClip cutInVoiceClip;
    public GameObject laserPrefab;

    // アイテムパネル関連
    public GameObject itemDisplayPanel;
    bool isItemPanelOpen = false;
    PlayerController playerController;
    EnemyController[] enemyControllers;
    BossSimpleJump[] BossControllers;
    BossSimpleJump boss;

    public static int currentStage = 1;

    public GameObject timeBar;
    public TextMeshProUGUI timeText;
    TimeController timeCnt;

    public TextMeshProUGUI scoreText;
    public static int totalScore = 0;
    public int stageScore = 0;

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
                if (s != null) loaded.Add(s);
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
        if (SceneManager.GetActiveScene().name == "Stage1") currentStage = 1;
        Invoke("InactiveImage", 1.0f);
        panel.SetActive(false);
        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null && timeCnt.gameTime == 0.0f) timeBar.SetActive(false);
        if (itemDisplayPanel != null) itemDisplayPanel.SetActive(false);
        UpdateScore();

        playerController = FindObjectOfType<PlayerController>();
        enemyControllers = FindObjectsOfType<EnemyController>();
        boss = FindObjectOfType<BossSimpleJump>();
    }

    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        // アイテムパネル関連などの既存処理...（省略）

        // カットイン演出...
        if (Input.GetKeyDown(KeyCode.S))
        {
            cutInImage.SetActive(true);
            if (cutInAudioSource != null && cutInVoiceClip != null)
            {
                cutInAudioSource.PlayOneShot(cutInVoiceClip);
            }
            Invoke(nameof(HideCutIn), 1.0f);
        }

        // ...ゲームクリアやゲームオーバー等、既存のUI制御のみ残す
    }

    void HideCutIn()
    {
        cutInImage.SetActive(false);
        FireLaser();
    }

    void FireLaser()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        Vector3 pos = player.transform.position;
        float dir = player.transform.localScale.x;
        float length = 1.3f;
        GameObject laserObj = Instantiate(laserPrefab, pos + new Vector3(5f * dir, 0, 0), Quaternion.identity);
        Vector3 scale = laserObj.transform.localScale;
        scale.x = length;
        if (dir < 0) scale.x *= -1;
        laserObj.transform.localScale = scale;
        laserObj.transform.position += new Vector3((length / 2f) * dir, 0, 0);
        Destroy(laserObj, 0.6f);
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    public void OnNextButton()
    {
        currentStage++;
        if (currentStage == 2)
            SceneManager.LoadScene("BaseScene2");
        else if (currentStage == 3)
            SceneManager.LoadScene("ResultScene");
    }

    void UpdateScore()
    {
        int score = stageScore + totalScore;
        if (scoreText != null) scoreText.text = score.ToString();
    }

    public Sprite GetEquippedSprite()
    {
        if (itemSprites != null && equippedItemId >= 0 && equippedItemId < itemSprites.Length)
            return itemSprites[equippedItemId];
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

        // ここにHPバー関連の記述は**もう入れない！**
        // タイマー制御や他UI関連のみ
        if (timeCnt == null)
        {
            timeCnt = GetComponent<TimeController>();
        }
        if (timeCnt != null)
        {
            if (scene.name.Contains("Shop"))
                timeCnt.enabled = false;
            else
                timeCnt.enabled = true;
        }
    }
}
