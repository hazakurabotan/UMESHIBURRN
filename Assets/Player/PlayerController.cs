using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// �v���C���[�̍s����HP�Ǘ������郁�C���X�N���v�g
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; // �v���C���[�̕�������p
    float axisH = 0.0f; // ���E�L�[���͒l
    float axisV = 0.0f; // �㉺�L�[���͒l
    public float speed = 3.0f; // �����X�s�[�h
    public float jump = 9.0f;  // �W�����v��
    public LayerMask groundLayer; // �n�ʂ̔���p
    bool goJump = false; // �W�����v�v���t���O

    SpriteRenderer sr;   // �v���C���[�̉摜�`��R���|�[�l���g
    float invincibleTimer = 0f; // ���G���ԃJ�E���^
    public float invincibleTime = 3f; // �_���[�W��̖��G����

    public AudioSource audioSource;   // ���ʉ���炷AudioSource�iInspector�ŃZ�b�g�j
    public AudioClip deathClip;       // ���S���̌��ʉ��iInspector�ŃZ�b�g�j
    public HpBarController hpBar; // HP�o�[�p�X�N���v�g�iInspector�ŃZ�b�g�j

    public int maxHP = 3; // �ő�HP
    int currentHP;        // ���݂�HP

    public float dashSpeed = 6.0f;        // �_�b�V���X�s�[�h
    public float dashDoubleTapTime = 0.3f;// �_�u���^�b�v��t����
    float lastLeftTapTime = -1f;          // ���L�[�Ō�ɉ���������
    float lastRightTapTime = -1f;         // �E�L�[�Ō�ɉ���������
    bool isDashing = false;               // �_�b�V����Ԃ��ǂ���

    public GameObject afterImagePrefab;   // �c���G�t�F�N�g��Prefab�iInspector�ŃZ�b�g�j
    public float afterImageInterval = 0.05f; // �c�����o��Ԋu�i�b�j
    private float afterImageTimer = 0f;       // �c���^�C�}�[

    public TextMeshProUGUI hpText;           // HP�̃e�L�X�g�\��

    Animator animator;    // �A�j���[�^�[
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0; // �X�R�A

    public GameObject ropePrefab; // ���[�v�p�v���n�u
    public Transform ropeSpawnPoint; // ���[�v���ˈʒu
    float ropeCooldown = 1f; // ���[�v�A�˃N�[���^�C��
    bool canShootRope = true;

    bool onLadder = false; // �n�V�S��ɂ��邩
    public float climbSpeed = 3f; // �n�V�S�o��X�s�[�h

    public static string gameState = "playing"; // �Q�[����ԁistatic�j

    // --- �m�b�N�o�b�N�p�ϐ� ---
    Vector2 knockbackVelocity = Vector2.zero;
    float knockbackTimer = 0f;
    float knockbackDuration = 0.2f;

    // �Q�[���J�n���Ɉ�x�Ă΂��
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();      // Rigidbody2D���擾
        animator = GetComponent<Animator>();      // Animator���擾
        sr = GetComponent<SpriteRenderer>();      // SpriteRenderer���擾 �����ꂪnull���ƐF����ł��Ȃ�
        Debug.Log("SpriteRenderer: " + sr);       // �������Ǝ擾�ł��Ă邩�m�F

        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameState = "playing";

        currentHP = maxHP;     // HP�𖞃^����
        UpdateHpUI();          // HP�Q�[�W�X�V
    }

    void Update()
    {
        if (gameState != "playing") return; // �Q�[���v���C���ȊO�͓����Ȃ�

        // ���͎擾
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        // --- �_�b�V���i2�񉟂����o�j ---
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastLeftTapTime < dashDoubleTapTime)
                isDashing = true; // 2�񉟂��Ȃ�_�b�V��
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
        // �ǂ��炩�̕����L�[�𗣂�����_�b�V������
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            isDashing = false;

        // --- �c���G�t�F�N�g ---
        void CreateAfterImage()
        {
            var obj = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            var sr = obj.GetComponent<SpriteRenderer>();
            var mySr = GetComponent<SpriteRenderer>();
            sr.sprite = mySr.sprite;
            sr.flipX = mySr.flipX;
            sr.color = new Color(1f, 1f, 1f, 0.5f); // ������
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

        // �������]
        if (axisH > 0) transform.localScale = new Vector2(1, 1);
        else if (axisH < 0) transform.localScale = new Vector2(-1, 1);

        // �W�����v
        if (Input.GetKeyDown(KeyCode.Z)) Jump();

        // ���[�v����
        if (Input.GetKeyDown(KeyCode.C) && canShootRope)
        {
            StartCoroutine(ShootRopeWithCooldown());
        }

        // �n�V�S�̏�܂ł������玟�̃V�[��
        if (onLadder && transform.position.y >= 5.5f)
        {
            SceneManager.LoadScene("Stage2");
        }

        // --- ���G���̓_�ŉ��o�i�������d�v�I�j ---
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;

            // ������Animator�ꎞ��~
            animator.enabled = false;

            // �_�Łi���F�j
            float blink = Mathf.Repeat(invincibleTimer * 10f, 1f);
            if (blink < 0.5f)
                sr.color = Color.yellow;
            else
                sr.color = new Color(1, 1, 0, 0);
        }
        else
        {
            // ���G����������Animator����
            animator.enabled = true;
            sr.color = Color.white;
        }

    }

    // ���[�v�N�[���^�C��
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

        float currentSpeed = isDashing ? dashSpeed : speed; // �_�b�V�����ʏ킩�ŃX�s�[�h�ύX

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

    // �����ɓ��������Ƃ�
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
            // �m�b�N�o�b�N
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

        // ���S���ʉ����Đ�
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
    }

    void GameStop()
    {
        rbody.velocity = Vector2.zero;
    }

    // ���_���[�W���󂯂���HP���炷�{���G�^�C�}�[�Z�b�g
    public void TakeDamage(int damage)
    {
        if (gameState != "playing") return;
        if (invincibleTimer > 0f) return; // ���G���͖���
        Debug.Log("�_���[�W�󂯂��I");
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHpUI();
        if (currentHP <= 0) GameOver();

        invincibleTimer = invincibleTime; // ���G�X�^�[�g
    }

    void UpdateHpUI()
    {
        if (hpBar != null)
            hpBar.SetHp(currentHP);
    }
}