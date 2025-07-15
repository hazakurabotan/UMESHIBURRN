using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshPro（高機能なテキスト表示）を使うため

public class PlayerController : MonoBehaviour
{
    // Rigidbody2D（物理演算用コンポーネント）への参照
    Rigidbody2D rbody;


    // 横・縦方向の入力値（-1, 0, 1）
    float axisH = 0.0f;
    float axisV = 0.0f;

    // 基本の移動速度・ジャンプ力
    public float speed = 3.0f;
    public float jump = 9.0f;

    // 地面判定用Layer
    public LayerMask groundLayer;
    // ジャンプするかのフラグ
    bool goJump = false;

    // 壁ジャンプ用（横方向・上方向の力）
    public float wallJumpPowerX = 7.0f;
    public float wallJumpPowerY = 12.0f;

    // 壁ジャンプ後に一瞬操作受付を止めるタイマー
    float wallJumpLockTimer = 0f;
    public float wallJumpLockTime = 0.2f;

    // ---- ぶら下がり・振り子アクション関連 ----
    Vector2 hangPoint = Vector2.zero; // ロープの支点
    float swingAngle = 0;   // 振り子の角度（ラジアン）
    float swingSpeed = 0;   // 振り子の速さ
    float ropeLength = 2f;  // ロープの長さ

    // 壁判定用の位置（左右）、そのLayer
    public Transform wallCheckLeft, wallCheckRight;
    public LayerMask wallLayer;
    public float wallCheckRadius = 0.2f;

    // 壁に触れているか（左右）・壁スライド状態
    bool isTouchingWallLeft = false, isTouchingWallRight = false;
    bool isWallSliding = false;
    public float wallSlideSpeed = 2f;

    // スプライト（キャラ画像）・無敵用タイマー
    SpriteRenderer sr;
    float invincibleTimer = 0f;
    public float invincibleTime = 3f;

    // 死亡時の音・体力バーUI
    public AudioSource audioSource;
    public AudioClip deathClip;
    public HpBarController hpBar; // 体力バーUI（Inspectorでドラッグして割り当て）

    // プレイヤーの最大HPと現在HP
    public int maxHP = 3;
    public int currentHP;

    // ダッシュ関連
    public float dashSpeed = 6.0f;
    public float dashDoubleTapTime = 0.3f;
    float lastLeftTapTime = -1f;
    float lastRightTapTime = -1f;
    bool isDashing = false;

    // 残像エフェクト（高速移動時に分身っぽく残るやつ）
    public GameObject afterImagePrefab;
    public float afterImageInterval = 0.05f;
    private float afterImageTimer = 0f;

    // HPテキスト表示（未使用なら外してOK）
    public TextMeshProUGUI hpText;

    // アニメーション管理
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";
    public GameObject punchHitbox;


    public RuntimeAnimatorController PlayerAnime;

    // スコア・攻撃ダメージなど
    public int score = 0;
    public int bulletDamage = 1;

    // ロープアクション用
    public GameObject ropePrefab;
    public Transform ropeSpawnPoint;
    float ropeCooldown = 1f;
    bool canShootRope = true;

    // ハシゴ判定・登り速度
    bool onLadder = false;
    public float climbSpeed = 3f;

    // ゲーム状態（"playing", "gameclear", "gameover"など）
    public static string gameState = "playing";
    bool isHanging = false; // ぶら下がり中かどうか

    // ノックバック（ダメージ時の吹っ飛び演出）
    Vector2 knockbackVelocity = Vector2.zero;
    float knockbackTimer = 0f;
    float knockbackDuration = 0.2f;

    float maxSwingAngle = Mathf.PI / 2; // 振り子の最大角度（90度）

    // --- 初期化 ---
    void Start()
    {
        Debug.Log("PlayerController.Start呼ばれた hpBar=" + (hpBar == null ? "null" : "OK"));

        // SceneReload 時に Inspector のリンクが切れるので
        hpBar = FindObjectOfType<HpBarController>();
        currentHP = maxHP;
        hpBar?.SetHp(currentHP);
        UpdateHpUI();

        // 各種コンポーネント取得
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // アニメ最初は「待機」
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameState = "playing";

        // --- ここがポイント！ ---
        if (animator.runtimeAnimatorController == null && PlayerAnime != null)
        {
            animator.runtimeAnimatorController = PlayerAnime;
        }
    }

    // --- 毎フレーム呼ばれる処理（プレイヤーの操作など） ---
    void Update()
    {
        // アイテムパネルが開いていたら何もしない（操作ロック）
        if (GameManager.Instance != null && GameManager.Instance.IsItemPanelOpen())
            return;

        // 壁ジャンプ後のロックタイマー
        if (wallJumpLockTimer > 0f)
            wallJumpLockTimer -= Time.deltaTime;

        // 死亡・クリア時は操作できない
        if (gameState != "playing") return;

        // 入力取得
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        // ダッシュ判定（素早く2回押しで発動）
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastLeftTapTime < dashDoubleTapTime)
                isDashing = true;
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
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            isDashing = false;

        // --- 残像エフェクト（高速移動時に半透明分身を生成） ---
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

        // 地面判定
        CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
        Vector2 origin = new Vector2(transform.position.x, col.bounds.min.y - 0.05f);
        bool onGround = Physics2D.OverlapCircle(origin, 0.1f, groundLayer);

        // --- 壁判定（左右） ---
        isTouchingWallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, wallLayer);
        isTouchingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, wallLayer);

        // --- 壁スライド状態 ---
        isWallSliding = false;
        if ((isTouchingWallLeft || isTouchingWallRight) && !onGround && axisH != 0 && wallJumpLockTimer <= 0f)
        {
            isWallSliding = true;
            rbody.velocity = new Vector2(rbody.velocity.x, -wallSlideSpeed); // ゆっくり落下
        }

        // --- 壁ジャンプ ---
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (onGround)
            {
                Jump(); // 地面なら普通にジャンプ
            }
            else if (isWallSliding)
            {
                // どちらの壁に触れてるかでジャンプ方向決定
                float wallJumpDirection = 0;
                if (isTouchingWallRight && !isTouchingWallLeft)
                    wallJumpDirection = -1;
                else if (isTouchingWallLeft && !isTouchingWallRight)
                    wallJumpDirection = 1;
                else
                    wallJumpDirection = (axisH >= 0) ? 1 : -1;

                // 壁から大きく飛ぶ
                Vector2 jumpVelocity = new Vector2(wallJumpDirection * wallJumpPowerX, wallJumpPowerY);
                rbody.velocity = jumpVelocity;

                // 一定時間は操作受付を止める（暴発防止）
                wallJumpLockTimer = wallJumpLockTime;
                isWallSliding = false;

                // キャラの向きも壁から離す
                transform.localScale = new Vector2(wallJumpDirection, 1);
                transform.position += new Vector3(wallJumpDirection * 0.2f, 0, 0);
            }

          

        }

        // --- ぶら下がり振り子アクション中 ---
        if (isHanging)
        {
            // 左右入力で揺れを加える
            if (Input.GetKey(KeyCode.LeftArrow))
                swingSpeed -= 5.0f * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                swingSpeed += 5.0f * Time.deltaTime;

            // 重力シミュレーション
            float gravity = 12.8f;
            swingSpeed -= gravity / ropeLength * Mathf.Sin(swingAngle) * Time.deltaTime;
            swingSpeed *= 0.999f; // 減衰
            swingAngle += swingSpeed * Time.deltaTime;

            // 振り子の角度制限（90度）
            if (swingAngle > maxSwingAngle)
            {
                swingAngle = maxSwingAngle;
                if (swingSpeed > 0) swingSpeed *= -0.95f;
            }
            else if (swingAngle < -maxSwingAngle)
            {
                swingAngle = -maxSwingAngle;
                if (swingSpeed < 0) swingSpeed *= -0.95f;
            }

            // キャラの位置を計算
            Vector2 offset = new Vector2(
                Mathf.Sin(swingAngle),
                -Mathf.Cos(swingAngle)
            ) * ropeLength;
            transform.position = hangPoint + offset;

            // ジャンプで振り子解除・大ジャンプ
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isHanging = false;
                rbody.velocity = new Vector2(
                    swingSpeed * ropeLength * Mathf.Cos(swingAngle + Mathf.PI / 2),
                    swingSpeed * ropeLength * Mathf.Sin(swingAngle + Mathf.PI / 2) + jump
                );
            }
            return; // 振り子中はこれ以降の処理を行わない
        }

        // --- キャラ向きの反転 ---
        if (axisH > 0) transform.localScale = new Vector2(1, 1);
        else if (axisH < 0) transform.localScale = new Vector2(-1, 1);

        // --- ロープ発射（Cキー） ---
        if (Input.GetKeyDown(KeyCode.C) && canShootRope)
        {
            Vector2 ropeDir;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                ropeDir = Vector2.up;
            else if (transform.localScale.x > 0)
                ropeDir = Vector2.right;
            else
                ropeDir = Vector2.left;

            GameObject ropeObj = Instantiate(ropePrefab, ropeSpawnPoint.position, Quaternion.identity);
            Rope ropeScript = ropeObj.GetComponent<Rope>();
            if (ropeScript != null)
                ropeScript.SetDirection(ropeDir);

            StartCoroutine(ShootRopeWithCooldown());
        }

        // --- ハシゴでシーン移動 ---
        if (onLadder && transform.position.y >= 5.5f)
        {
            SceneManager.LoadScene("Stage2"); // 例：上に登りきったら次のシーンへ
        }

        // --- 無敵時間の減少 ---
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }

        // ここはデバッグ用なのでテスト後は消す
        if (animator != null)
            Debug.Log("毎フレーム Controller名: " + animator.runtimeAnimatorController.name);


        if (Input.GetKeyDown(KeyCode.V) && animator != null)
        {
            animator.SetTrigger("Punch");
            StartCoroutine(PunchAttack());
        }

        if (Input.GetKeyDown(KeyCode.F) && animator != null)
        {
            animator.SetTrigger("yowaPunch");
            StartCoroutine(PunchAttack());
        }

        if (Input.GetKeyDown(KeyCode.H) && animator != null)
        {
            animator.SetTrigger("UpaaaPunch");
            StartCoroutine(PunchAttack());
        }

        if (Input.GetKeyDown(KeyCode.G) && animator != null)
        {
            animator.SetTrigger("DunkPunch");
            StartCoroutine(PunchAttack());
        }






    }

    // --- ロープぶら下がり開始時に呼ぶ ---
    public void StartHangFromRope(Vector2 ropePoint)
    {
        isHanging = true;
        hangPoint = ropePoint;

        // 支点から自分までのベクトルで角度を求める
        Vector2 delta = transform.position - (Vector3)ropePoint;
        swingAngle = Mathf.Atan2(delta.y, delta.x);
        swingSpeed = 0;
    }

    // --- アニメや色変更などの描画系処理はLateUpdateで一括管理 ---
    void LateUpdate()
    {
        PlayerShoot shoot = GetComponent<PlayerShoot>();

        // 無敵時は点滅（黄色）
        if (invincibleTimer > 0f)
        {
            float blink = Mathf.Repeat(invincibleTimer * 10f, 1f);
            sr.color = (blink < 0.5f) ? Color.yellow : new Color(1, 1, 0, 0);
        }
        // 溜め撃ち時は赤や青で点滅
        else if (shoot != null && shoot.isCharging)
        {
            float blink = Mathf.PingPong(Time.time * 3f, 1f);
            if (shoot.chargeTime >= shoot.requiredCharge)
                sr.color = (blink < 0.5f) ? Color.red : new Color(1f, 1f, 1f, 0.4f);
            else
                sr.color = (blink < 0.5f) ? Color.blue : new Color(1f, 1f, 1f, 0.4f);
        }
        // 通常時は元の色
        else
        {
            sr.color = Color.white;
        }
    }

    // --- 無敵かどうかを外部から参照 ---
    public bool IsInvincible()
    {
        return invincibleTimer > 0f;
    }

    // --- ロープ連射の間隔を調整 ---
    IEnumerator ShootRopeWithCooldown()
    {
        canShootRope = false;

        Vector2 ropeDir;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            ropeDir = Vector2.up;
        else
            ropeDir = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;

        Vector3 spawnPos = ropeSpawnPoint != null ? ropeSpawnPoint.position : transform.position;
        GameObject ropeObj = Instantiate(ropePrefab, spawnPos, Quaternion.identity);
        Rope ropeScript = ropeObj.GetComponent<Rope>();
        if (ropeScript != null)
            ropeScript.SetDirection(ropeDir);

        yield return new WaitForSeconds(ropeCooldown);
        canShootRope = true;
    }

    // --- 物理計算（移動・ジャンプ） ---
    void FixedUpdate()
    {
        if (gameState != "playing") return;

        // ノックバック中は移動不能
        if (knockbackTimer > 0)
        {
            rbody.velocity = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        float inputX = axisH;

        // 壁ジャンプロック時は操作できない
        if (wallJumpLockTimer > 0)
        {
            wallJumpLockTimer -= Time.fixedDeltaTime;
            inputX = 0;
        }
        else
        {
            wallJumpLockTimer = 0;
        }

        // 壁スライド時は落下速度だけ制限
        if (isWallSliding)
        {
            rbody.velocity = new Vector2(
                rbody.velocity.x,
                Mathf.Max(rbody.velocity.y, -wallSlideSpeed)
            );
        }
        else
        {
            rbody.velocity = new Vector2(inputX * (isDashing ? dashSpeed : speed), rbody.velocity.y);
        }

        // ハシゴ移動時は重力ゼロで上下移動
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

            if (onGround && goJump)
            {
                rbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                goJump = false;
            }

            // 状態ごとにアニメ切り替え
            nowAnime = onGround ? (axisH == 0 ? stopAnime : moveAnime) : jumpAnime;
            if (nowAnime != oldAnime)
            {
                oldAnime = nowAnime;
                if (animator.HasState(0, Animator.StringToHash(nowAnime)))
                    animator.Play(nowAnime);
            }
        }
    }

    // --- アイテムや敵に触れた時の処理 ---
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
            Vector2 knockDir = (transform.position - col.transform.position).normalized;
            float knockPower = 8f;
            knockbackVelocity = knockDir * knockPower;
            knockbackTimer = knockbackDuration;

            TakeDamage(1); // 敵に当たったらダメージ
        }
    }

    // --- ハシゴから離れた時の処理 ---
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }

    // --- ジャンプ処理（他スクリプトから呼び出しもできる） ---
    public void Jump()
    {
        goJump = true;
    }

    // --- ゴール時の処理 ---
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
    }

    // --- 死亡時の処理 ---
    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // 元: revivedOverrideController（PlayerControllerのメンバ変数）
        // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        // 新: GameManager.Instance.revivedOverrideController
        if (
    GameManager.Instance != null &&
    animator.runtimeAnimatorController == GameManager.Instance.revivedOverrideController &&
    PlayerAnime != null)
        {
            animator.runtimeAnimatorController = PlayerAnime;
            Debug.Log("死亡したのでPlayerAnimeに戻したよ！");
        }

        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
    }


    // --- 死亡時の移動停止 ---
    void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }

    // --- ダメージを受けた時の処理 ---
    public void TakeDamage(int damage)
    {
        if (gameState != "playing") return;
        if (invincibleTimer > 0f) return;
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (hpBar != null) hpBar.SetHp(currentHP); // UI更新
        if (currentHP <= 0) GameOver();

        invincibleTimer = invincibleTime;
    }

    // --- 回復した時の処理 ---
    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // ★必ずUI更新
        if (hpBar == null)
            hpBar = FindObjectOfType<HpBarController>();
        if (hpBar != null) hpBar.SetHp(currentHP);
    }

    // --- HP UIを更新する関数 ---
    public void UpdateHpUI()
    {
        // hpBarがnullなら自動で探してセット（ここ超重要！）
        if (hpBar == null)
        {
            hpBar = FindObjectOfType<HpBarController>();
            if (hpBar == null)
            {
                Debug.LogWarning("HpBarControllerが見つかりませんでした");
                return;
            }
        }
        Debug.Log("UpdateHpUI呼ばれた currentHP=" + currentHP + " hpBar=" + (hpBar == null ? "null" : "OK"));
        hpBar.SetHp(currentHP, maxHP);
    }

    IEnumerator PunchAttack()
    {
        // パンチ中は無敵
        invincibleTimer = 0.2f;  // パンチアニメの長さ分
        punchHitbox.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.15f); // パンチの判定持続時間
        punchHitbox.GetComponent<Collider2D>().enabled = false;
        // パンチ終わったらinvincibleTimerが切れるまで自動で無敵解除
    }
}
