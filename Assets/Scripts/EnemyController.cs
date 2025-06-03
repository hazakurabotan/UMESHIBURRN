using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;           // �ړ����x
    public bool isToRight = false;       // true=�E�����Afalse=������
    public float revTime = 0f;           // �������]����܂ł̎��ԁi0�Ȃ甽�]�Ȃ��j
    public LayerMask groundLayer;        // �n�ʂ̃��C���[

    float time = 0f;                     // �o�ߎ��ԁi���]�p�j

    private Enemy enemy;                // �G�X�N���v�g�Q��

    void Start()
    {
        enemy = GetComponent<Enemy>(); // �� Enemy.cs ���擾
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void Update()
    {
        if (revTime > 0)
        {
            time += Time.deltaTime;
            if (time >= revTime)
            {
                isToRight = !isToRight;
                time = 0f;
                transform.localScale = isToRight ? new Vector2(-1, 1) : new Vector2(1, 1);
            }
        }
    }

    void FixedUpdate()
    {
        // �G���������Ă�Œ��͎��͂œ����Ȃ��I
        if (enemy != null && enemy.isFlying) return;

        bool onGround = Physics2D.CircleCast(
            transform.position,
            0.5f,
            Vector2.down,
            0.5f,
            groundLayer
        );

        if (onGround)
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (rbody != null)
            {
                float moveX = isToRight ? speed : -speed;
                rbody.velocity = new Vector2(moveX, rbody.velocity.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight;
        time = 0f;
        transform.localScale = isToRight ? new Vector2(-1, 1) : new Vector2(1, 1);
    }
}
