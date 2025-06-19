using UnityEngine;

public class WindPlatform : MonoBehaviour
{
    public float liftForce = 10f;  // 上向きに与える力
    public float liftDuration = 0.3f; // 持続時間（秒）

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // プレイヤーが下向きに速度を持っている時だけ強制的に上方向へ
                if (playerRb.velocity.y <= 0.1f)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, liftForce);
                }
            }
        }
    }
}
