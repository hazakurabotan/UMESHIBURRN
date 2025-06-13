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

    // ====== �Ԃ牺����U��q�p ======
    Vector2 hangPoint = Vector2.zero;      // �x�_
    float swingAngle = 0;   // ���݂̊p�x�i���W�A���j
    float swingSpeed = 0;   // �h��̑��x
    float ropeLength = 2f;  // ���[�v�̒���

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
   


    Vector2 knockbackVelocity = Vector2.zero;
    float knockbackTimer = 0f;
    float knockbackDuration = 0.2f;

    float maxSwingAngle = Mathf.PI / 2; // 90�x�i���W�A���j



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

        // �_�b�V��
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

        // �c���G�t�F�N�g
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
            // --- �U��q�̉^������ ---
            // ��: ���E�L�[���͂�swingSpeed�ɉ���
            if (Input.GetKey(KeyCode.LeftArrow))
                swingSpeed -= 5.0f * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                swingSpeed += 5.0f * Time.deltaTime;

            // �d�͌��ʂɂ�����
            float gravity = 12.8f;
            swingSpeed -= gravity / ropeLength * Mathf.Sin(swingAngle) * Time.deltaTime;

            // �p���x����
            swingSpeed *= 0.999f;

            // �p�x�X�V
            swingAngle += swingSpeed * Time.deltaTime;

            // === �U��q�̐U�����}90�x�ɐ��� ===
            if (swingAngle > maxSwingAngle)
            {
                swingAngle = maxSwingAngle;
                if (swingSpeed > 0) swingSpeed *= -0.95f; // ���˕Ԃ茸���i�K�v�Ȃ�j
            }
            else if (swingAngle < -maxSwingAngle)
            {
                swingAngle = -maxSwingAngle;
                if (swingSpeed < 0) swingSpeed *= -0.95f; // ���˕Ԃ茸���i�K�v�Ȃ�j
            }

            // �v���C���[���W���x�_����̋ɍ��W�Ō���
            Vector2 offset = new Vector2(
                Mathf.Sin(swingAngle),
                -Mathf.Cos(swingAngle)
            ) * ropeLength;
            transform.position = hangPoint + offset;

            // �W�����v�ŉ����i�����t�^���j
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isHanging = false;
                // �����W�����v�i�����^���ɏ�����������j
                rbody.velocity = new Vector2(
                    swingSpeed * ropeLength * Mathf.Cos(swingAngle + Mathf.PI / 2),
                    swingSpeed * ropeLength * Mathf.Sin(swingAngle + Mathf.PI / 2) + jump
                );
            }
            return;
        }

        // �������]
        if (axisH > 0) transform.localScale = new Vector2(1, 1);
        else if (axisH < 0) transform.localScale = new Vector2(-1, 1);

        // �W�����v
        if (Input.GetKeyDown(KeyCode.Z)) Jump();

        // ���[�v����
        if (Input.GetKeyDown(KeyCode.C) && canShootRope)
        {
            Vector2 ropeDir;
            // ������̂�
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                ropeDir = Vector2.up;
            // �������͌��ĂȂ��I
            // else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            //     ropeDir = Vector2.down; // �����̍s���R�����g�A�E�g/�폜
            // ������
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


        // �n�V�S�ŃV�[���ړ�
        if (onLadder && transform.position.y >= 5.5f)
        {
            SceneManager.LoadScene("Stage2");
        }

        // ���G�^�C�}�[�i�s���������ŊǗ�
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }

    }

    public void StartHangFromRope(Vector2 ropePoint)
    {
        isHanging = true;
        hangPoint = ropePoint;

        // �x�_���獡�̎����ւ̃x�N�g���ŏ����p�x
        Vector2 delta = transform.position - (Vector3)ropePoint;
        swingAngle = Mathf.Atan2(delta.y, delta.x);
        swingSpeed = 0;
    }



    // LateUpdate�őS�Ă̐F������ꌳ��
    void LateUpdate()
    {
        PlayerShoot shoot = GetComponent<PlayerShoot>();

        if (invincibleTimer > 0f)
        {
            // ���G���_�Łi�D��x�ŏ�ʁj
            float blink = Mathf.Repeat(invincibleTimer * 10f, 1f);
            sr.color = (blink < 0.5f) ? Color.yellow : new Color(1, 1, 0, 0);
        }
        else if (shoot != null && shoot.isCharging)
        {
            // �`���[�W���_�Łi���G�łȂ���΁j
            float blink = Mathf.PingPong(Time.time * 3f, 1f);
            if (shoot.chargeTime >= shoot.requiredCharge)
                sr.color = (blink < 0.5f) ? Color.red : new Color(1f, 1f, 1f, 0.4f);
            else
                sr.color = (blink < 0.5f) ? Color.blue : new Color(1f, 1f, 1f, 0.4f);
        }
        else
        {
            // �ʏ펞
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

        // ��������i��L�[�D��A����ȊO�͌����Ă�����j
        Vector2 ropeDir;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            ropeDir = Vector2.up;
        else
            ropeDir = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;

        Vector3 spawnPos = ropeSpawnPoint != null ? ropeSpawnPoint.position : transform.position;
        GameObject ropeObj = Instantiate(ropePrefab, spawnPos, Quaternion.identity);

        // Rope�X�N���v�g�ɕ�����n��
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
        Debug.Log("�_���[�W�󂯂��I");
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
        Debug.Log("�񕜁I ���݂�HP: " + currentHP);
    }


    void UpdateHpUI()
    {
        if (hpBar != null)
            hpBar.SetHp(currentHP);
    }
}
