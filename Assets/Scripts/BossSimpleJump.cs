using UnityEngine;

public class BossSimpleJump : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveTime = 2f;
    public float jumpPower = 7f;     // �W�����v�̋���
    public float jumpSpeed = 4f;     // �W�����v���̉����x
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
            case 0: // ����
                timer += Time.deltaTime;
                rb.velocity = new Vector2(moveSpeed * moveDir, rb.velocity.y);

                if (timer >= moveTime)
                {
                    timer = 0f;
                    state = 1; // �W�����v��
                    float dx = (sr != null ? sr.bounds.size.x * 5f : 5f);
                    jumpTargetX = transform.position.x + dx * moveDir;
                    // **�����x�͈��A����������W�����v�͂�������**
                    rb.velocity = new Vector2(jumpSpeed * moveDir, jumpPower);
                    isJumping = true;
                }
                break;

            case 1: // �W�����v��
                // **�W�����v���������x���Œ�i�K�v�Ȃ�j**
                if (isJumping)
                {
                    rb.velocity = new Vector2(jumpSpeed * moveDir, rb.velocity.y);
                    // **�W�����v�������B�`�F�b�N**
                    if ((moveDir == -1 && transform.position.x <= jumpTargetX) ||
                        (moveDir == 1 && transform.position.x >= jumpTargetX))
                    {
                        // �ڕW�ɓ��B������X���W��␳���Ē�~
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
                    state = 2; // ��~��Ԃ�
                }
                break;

            case 2: // ���n��1�b��~
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
