using UnityEngine;

public class HoverPlatform : MonoBehaviour
{
    public float hoverHeight = 1.2f;       // 足場の上で浮かせたい高さ（地面からの相対値）
    public float hoverStrength = 20f;      // 上向きの浮力（大きいほどカチッと止まる）

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // プレイヤーの現在位置（足場表面のY座標）
                float surfaceY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
                float diff = (surfaceY + hoverHeight) - collision.transform.position.y;

                // 浮力を与える（バネっぽい制御）
                float force = diff * hoverStrength - rb.velocity.y * 8f;
                rb.AddForce(Vector2.up * force);

                // オプション：慣性でふわふわさせたい場合は↑の数値小さめに
            }
        }
    }
}
