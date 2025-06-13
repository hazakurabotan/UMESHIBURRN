using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    float axisV = 0.0f;
    public float speed = 3.0f;
    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJump = false;

    SpriteRenderer sr;
    float invincibleTimer = 0f;
    public float invincibleTime = 3f;

    public AudioSource audioSource;
    public AudioClip deathClip;
    public HpBarController hpBar;

    public int maxHP = 3;
    int currentHP;

    public float dashSpeed = 6.0f;
    public float dashDoubleTapTime = 0.3f;
    float lastLeftTapTime = -1f;
    float lastRightTapTime = -1f;
    bool isDashing = false;

    public GameObject afterImagePrefab;
    public float afterImageInterval = 0.05f;
    private float afterImageTimer = 0f;
    public TextMeshProUGUI hpText;

    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0;
    public GameObject ropePrefab;
    public Transform ropeSpawnPoint;
    float ropeCooldown = 1f;
    bool canShootRope = true;

    bool onLadder = false;
    public float climbSpeed = 3f;

    public static string gameState = "playing";
    bool isHanging = false;
    Vector2 hangPoint = Vector2.zero;


    Vector2 knockbackVelocity = Vector2.zero;
    float knockbackTimer = 0f;
    float knockbackDuration = 0.2f;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        Debug.Log("SpriteRenderer: " + sr);

        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameState = "playing";

        currentHP = maxHP;
        UpdateHpUI();
    }

    void Update()
    {
        if (gameState != "playing") return;

        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        // ダッシュ
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

        // 残像エフェクト
        void CreateAfterImage()
        {
            var obj = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            var sr = obj.GetComponent<SpriteRenderer>();
            var mySr = GetComponent<SpriteRenderer>();
            sr.sprite = mySr.sprite;
            sr.flipX = mySr.flipX;
            sr.color = new Color(1f, 1f, 1f, 0.5f);
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

        if (isHanging)
        {
            // プレイヤーの位置をロープのぶら下がり位置に維持
            transform.position = hangPoint + new Vector2(0, -0.5f); // 少し下にずらす
                                                                    // ジャンプや特定キーで解除可能
            if (Input.GetKeyDown(KeyCode.Z)) // 例：ジャンプ解除
            {
                isHanging = false;
                // 普通の操作に戻る
            }
            return; // それ以外のUpdate処理はスキップ
        }

        // 向き反転
        if (axisH > 0) transform.localScale = new Vector2(1, 1);
        else if (axisH < 0) transform.localScale = new Vector2(-1, 1);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Z)) Jump();

        // ロープ発射
        if (Input.GetKeyDown(KeyCode.C) && canShootRope)
        {
            Vector2 ropeDir;
            // 上方向のみ
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                ropeDir = Vector2.up;
            // 下方向は撃てない！
            // else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            //     ropeDir = Vector2.down; // ←この行をコメントアウト/削除
            // 横方向
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


        // ハシゴでシーン移動
        if (onLadder && transform.position.y >= 5.5f)
        {
            SceneManager.LoadScene("Stage2");
        }

        // 無敵タイマー進行だけここで管理
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }

    }

    // LateUpdateで全ての色制御を一元化
    void LateUpdate()
    {
        PlayerShoot shoot = GetComponent<PlayerShoot>();

        if (invincibleTimer > 0f)
        {
            // 無敵中点滅（優先度最上位）
            float blink = Mathf.Repeat(invincibleTimer * 10f, 1f);
            sr.color = (blink < 0.5f) ? Color.yellow : new Color(1, 1, 0, 0);
        }
        else if (shoot != null && shoot.isCharging)
        {
            // チャージ中点滅（無敵でなければ）
            float blink = Mathf.PingPong(Time.time * 3f, 1f);
            if (shoot.chargeTime >= shoot.requiredCharge)
                sr.color = (blink < 0.5f) ? Color.red : new Color(1f, 1f, 1f, 0.4f);
            else
                sr.color = (blink < 0.5f) ? Color.blue : new Color(1f, 1f, 1f, 0.4f);
        }
        else
        {
            // 通常時
            sr.color = Color.white;
        }
    }

    public bool IsInvincible()
    {
        return invincibleTimer > 0f;
    }

    IEnumerator ShootRopeWithCooldown()
    {
        canShootRope = false;

        // 方向決定（上キー優先、それ以外は向いてる向き）
        Vector2 ropeDir;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            ropeDir = Vector2.up;
        else
            ropeDir = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;

        Vector3 spawnPos = ropeSpawnPoint != null ? ropeSpawnPoint.position : transform.position;
        GameObject ropeObj = Instantiate(ropePrefab, spawnPos, Quaternion.identity);

        // Ropeスクリプトに方向を渡す
        Rope ropeScript = ropeObj.GetComponent<Rope>();
        if (ropeScript != null)
            ropeScript.SetDirection(ropeDir);

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

        float currentSpeed = isDashing ? dashSpeed : speed;

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

        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
    }

    void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        if (gameState != "playing") return;
        if (invincibleTimer > 0f) return;
        Debug.Log("ダメージ受けた！");
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHpUI();
        if (currentHP <= 0) GameOver();

        invincibleTimer = invincibleTime;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHpUI();
        Debug.Log("回復！ 現在のHP: " + currentHP);
    }


    public void StartHangFromRope(Vector2 ropePoint)
    {
        // 例：プレイヤーの動きを止めて、その場所にぶら下げる
        rbody.velocity = Vector2.zero;
        // ぶら下がりフラグをON（Updateで特殊挙動をする用）
        isHanging = true;
        hangPoint = ropePoint;
    }


    void UpdateHpUI()
    {
        if (hpBar != null)
            hpBar.SetHp(currentHP);
    }
}
