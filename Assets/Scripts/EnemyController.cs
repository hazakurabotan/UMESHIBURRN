using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;           // 移動速度
    public bool isToRight = false;       // true=右向き、false=左向き
    public float revTime = 0f;           // 自動反転するまでの時間（0なら反転なし）
    public LayerMask groundLayer;        // 地面のレイヤー

    float time = 0f;                     // 経過時間（反転用）

    private Enemy enemy;                // 敵スクリプト参照

    void Start()
    {
        enemy = GetComponent<Enemy>(); // ← Enemy.cs を取得
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
        // 敵が投げられてる最中は自力で動かない！
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
