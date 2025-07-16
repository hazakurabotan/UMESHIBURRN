using UnityEngine;

public class SummonFastWalker : MonoBehaviour
{
    [Header("�p�g���[���ݒ�")]
    public float patrolWidth = 4f; // ���E�̈ړ��͈�
    public float speed = 20f;      // �ړ����x

    [Header("���s�A�j���̃p�����[�^��")]
    public string moveParamName = "isMoving";

    private Vector3 startPos;
    private int direction = 1;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        Destroy(gameObject, 3f);

        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float left = startPos.x - patrolWidth / 2f;
        float right = startPos.x + patrolWidth / 2f;

        if ((direction == 1 && transform.position.x >= right) ||
            (direction == -1 && transform.position.x <= left))
        {
            direction *= -1;
            // ������ς���
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
        }

        // X���������ړ�
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);

        // ���s�A�j���؂�ւ�
        if (animator != null && !string.IsNullOrEmpty(moveParamName))
        {
            animator.SetBool(moveParamName, true);
        }
    }
}
