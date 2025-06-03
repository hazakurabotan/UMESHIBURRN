using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// プレイヤーの行動やHP管理をするメインスクリプト
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; // プレイヤーの物理制御用
    float axisH = 0.0f; // 左右キー入力値
    float axisV = 0.0f; // 上下キー入力値
    public float speed = 3.0f; // 歩きスピード
    public float jump = 9.0f;  // ジャンプ力
    public LayerMask groundLayer; // 地面の判定用
    bool goJump = false; // ジャンプ要求フラグ

    SpriteRenderer sr;   // プレイヤーの画像描画コンポーネント
    float invincibleTimer = 0f; // 無敵時間カウンタ
    public float invincibleTime = 3f; // ダメージ後の無敵時間

    public AudioSource audioSource;   // 効果音を鳴らすAudioSource（Inspectorでセット）
    public AudioClip deathClip;       // 死亡時の効果音（Inspectorでセット）
    public HpBarController hpBar; // HPバー用スクリプト（Inspectorでセット）

    public int maxHP = 3; // 最大HP
    int currentHP;        // 現在のHP

    public float dashSpeed = 6.0f;        // ダッシュスピード
    public float dashDoubleTapTime = 0.3f;// ダブルタップ受付時間
    float lastLeftTapTime = -1f;          // 左キー最後に押した時刻
    float lastRightTapTime = -1f;         // 右キー最後に押した時刻
    bool isDashing = false;               // ダッシュ状態かどうか

    public GameObject afterImagePrefab;   // 残像エフェクトのPrefab（Inspectorでセット）
    public float afterImageInterval = 0.05f; // 残像が出る間隔（秒）
    private float afterImageTimer = 0f;       // 残像タイマー

    public TextMeshProUGUI hpText;           // HPのテキスト表示

    Animator animator;    // アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; // スコア

    public GameObject ropePrefab; // ロープ用プレハブ
    public Transform ropeSpawnPoint; // ロープ発射位置
    float ropeCooldown = 1f; // ロープ連射クールタイム
    bool canShootRope = true;

    bool onLadder = false; // ハシゴ上にいるか
    public float climbSpeed = 3f; // ハシゴ登るスピード

    public static string gameState = "playing"; // ゲーム状態（static）

    // --- ノックバック用変数 ---
    Vector2 knockbackVelocity = Vector2.zero;
    float knockbackTimer = 0f;
    float knockbackDuration = 0.2f;

    // ゲーム開始時に一度呼ばれる
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();      // Rigidbody2Dを取得
        animator = GetComponent<Animator>();      // Animatorを取得
        sr = GetComponent<SpriteRenderer>();      // SpriteRendererを取得 ★これがnullだと色操作できない
        Debug.Log("SpriteRenderer: " + sr);       // ★ちゃんと取得できてるか確認

        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameState = "playing";

        currentHP = maxHP;     // HPを満タンに
        UpdateHpUI();          // HPゲージ更新
    }

    void Update()
    {
        if (gameState != "playing") return; // ゲームプレイ中以外は動かない

        // 入力取得
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        // --- ダッシュ（2回押し検出） ---
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastLeftTapTime < dashDoubleTapTime)
                isDashing = true; // 2回押しならダッシュ
            else
                isDashing = false;
            lastLeftTapTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastRightTapTime < dashDoubleTapTime)
                isDashing = true;
            else
                isDashing = false;
            lastRightTapTime = Time.time;
        }
        // どちらかの方向キーを離したらダッシュ解除
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            isDashing = false;

        // --- 残像エフェクト ---
        void CreateAfterImage()
        {
            var obj = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            var sr = obj.GetComponent<SpriteRenderer>();
            var mySr = GetComponent<SpriteRenderer>();
            sr.sprite = mySr.sprite;
            sr.flipX = mySr.flipX;
            sr.color = new Color(1f, 1f, 1f, 0.5f); // 半透明
        }
        if (isDashing)
        {
            afterImageTimer -= Time.deltaTime;
            if (afterImageTimer <= 0f)
            {
                CreateAfterImage();
                afterImageTimer = afterImageInterval;
            }
        }
        else
        {
            afterImageTimer = 0f;
        }

        // 向き反転
        if (axisH > 0) transform.localScale = new Vector2(1, 1);
        else if (axisH < 0) transform.localScale = new Vector2(-1, 1);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Z)) Jump();

        // ロープ発射
        if (Input.GetKeyDown(KeyCode.C) && canShootRope)
        {
            StartCoroutine(ShootRopeWithCooldown());
        }

        // ハシゴの上までいったら次のシーン
        if (onLadder && transform.position.y >= 5.5f)
        {
            SceneManager.LoadScene("Stage2");
        }

        // --- 無敵時の点滅演出（ここが重要！） ---
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;

            // ここでAnimator一時停止
            animator.enabled = false;

            // 点滅（黄色）
            float blink = Mathf.Repeat(invincibleTimer * 10f, 1f);
            if (blink < 0.5f)
                sr.color = Color.yellow;
            else
                sr.color = new Color(1, 1, 0, 0);
        }
        else
        {
            // 無敵解除したらAnimator復活
            animator.enabled = true;
            sr.color = Color.white;
        }

    }

    // ロープクールタイム
    IEnumerator ShootRopeWithCooldown()
    {
        canShootRope = false;
        Vector3 spawnPos = ropeSpawnPoint != null ? ropeSpawnPoint.position : transform.position;
        Instantiate(ropePrefab, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(ropeCooldown);
        canShootRope = true;
    }

    void FixedUpdate()
    {
        if (gameState != "playing") return;

        if (knockbackTimer > 0)
        {
            rbody.velocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        float currentSpeed = isDashing ? dashSpeed : speed; // ダッシュか通常かでスピード変更

        if (onLadder)
        {
            rbody.velocity = new Vector2(0, axisV * climbSpeed);
            rbody.gravityScale = 0;
        }
        else
        {
            rbody.gravityScale = 1;
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            Vector2 origin = new Vector2(transform.position.x, col.bounds.min.y - 0.05f);
            bool onGround = Physics2D.OverlapCircle(origin, 0.1f, groundLayer);

            rbody.velocity = new Vector2(axisH * currentSpeed, rbody.velocity.y);

            if (onGround && goJump)
            {
                rbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                goJump = false;
            }

            nowAnime = onGround ? (axisH == 0 ? stopAnime : moveAnime) : jumpAnime;
            if (nowAnime != oldAnime)
            {
                oldAnime = nowAnime;
                if (animator.HasState(0, Animator.StringToHash(nowAnime)))
                    animator.Play(nowAnime);
            }
        }
    }

    // 何かに当たったとき
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            onLadder = true;
        }
        else if (col.CompareTag("Goal"))
        {
            Goal();
        }
        else if (col.CompareTag("Dead"))
        {
            GameOver();
        }
        else if (col.CompareTag("ScoreItem"))
        {
            var item = col.GetComponent<ItemData>();
            if (item != null)
            {
                score += item.value;
                Destroy(col.gameObject);
            }
        }
        else if (col.CompareTag("Enemy"))
        {
            // ノックバック
            Vector2 knockDir = (transform.position - col.transform.position).normalized;
            float knockPower = 8f;
            knockbackVelocity = knockDir * knockPower;
            knockbackTimer = knockbackDuration;

            TakeDamage(1);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }

    public void Jump()
    {
        goJump = true;
    }

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
    }

    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // 死亡効果音を再生
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
    }

    void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }

    // ★ダメージを受けたらHP減らす＋無敵タイマーセット
    public void TakeDamage(int damage)
    {
        if (gameState != "playing") return;
        if (invincibleTimer > 0f) return; // 無敵中は無視
        Debug.Log("ダメージ受けた！");
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHpUI();
        if (currentHP <= 0) GameOver();

        invincibleTimer = invincibleTime; // 無敵スタート
    }

    void UpdateHpUI()
    {
        if (hpBar != null)
            hpBar.SetHp(currentHP);
    }
}