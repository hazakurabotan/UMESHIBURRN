using UnityEngine;

public class BossSimpleJump : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveTime = 2f;
    public float jumpPower = 7f;     // ジャンプの強さ
    public float jumpSpeed = 4f;     // ジャンプ中の横速度
    public float standTime = 1f;

    private int moveDir = -1;
    private float timer = 0f;
    private int state = 0;
    private float jumpTargetX;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        timer = 0f;
        state = 0;
    }

    void Update()
    {
        switch (state)
        {
            case 0: // 歩く
                timer += Time.deltaTime;
                rb.velocity = new Vector2(moveSpeed * moveDir, rb.velocity.y);

                if (timer >= moveTime)
                {
                    timer = 0f;
                    state = 1; // ジャンプへ
                    float dx = (sr != null ? sr.bounds.size.x * 5f : 5f);
                    jumpTargetX = transform.position.x + dx * moveDir;
                    // **横速度は一定、上方向だけジャンプ力を加える**
                    rb.velocity = new Vector2(jumpSpeed * moveDir, jumpPower);
                    isJumping = true;
                }
                break;

            case 1: // ジャンプ中
                // **ジャンプ中も横速度を固定（必要なら）**
                if (isJumping)
                {
                    rb.velocity = new Vector2(jumpSpeed * moveDir, rb.velocity.y);
                    // **ジャンプ距離到達チェック**
                    if ((moveDir == -1 && transform.position.x <= jumpTargetX) ||
                        (moveDir == 1 && transform.position.x >= jumpTargetX))
                    {
                        // 目標に到達したらX座標を補正して停止
                        Vector2 pos = transform.position;
                        pos.x = jumpTargetX;
                        transform.position = pos;
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }

                if (isJumping && IsOnGround())
                {
                    isJumping = false;
                    rb.velocity = Vector2.zero;
                    timer = 0f;
                    state = 2; // 停止状態へ
                }
                break;

            case 2: // 着地後1秒停止
                rb.velocity = Vector2.zero;
                timer += Time.deltaTime;
                if (timer >= standTime)
                {
                    timer = 0f;
                    moveDir *= -1;
                    state = 0;
                }
                break;
        }
    }

    bool IsOnGround()
    {
        return Mathf.Abs(rb.velocity.y) < 0.05f;
    }
}
