using UnityEngine;

// ----------------------------------------------------
// FallingPlatform
// プレイヤーが乗るとしばらくして落下し、最後は消える足場のスクリプト
// ----------------------------------------------------
public class FallingPlatform : MonoBehaviour
{
    // プレイヤーが乗ってから落ちるまでの待ち時間（秒）
    public float fallDelay = 2f;

    // この足場のRigidbody2D（物理挙動用）
    private Rigidbody2D rb;

    // すでに落下開始しているかのフラグ
    private bool isFalling = false;

    // ゲーム開始時に一度だけ実行
    void Start()
    {
        // このオブジェクトに付いているRigidbody2Dを取得
        rb = GetComponent<Rigidbody2D>();
    }

    // 何かとぶつかった時に呼ばれる（2D物理用）
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 相手が「Player」タグのオブジェクト、かつまだ落下していなければ
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true; // 一度だけ反応
            // fallDelay秒後にDropPlatform()を呼ぶ（タイマーセット）
            Invoke("DropPlatform", fallDelay);
        }
    }

    // 足場を落下させる処理
    void DropPlatform()
    {
        // Rigidbody2DをDynamic（重力に従う）に変更→落下開始
        rb.bodyType = RigidbodyType2D.Dynamic;
        // 足場オブジェクト自体を2秒後に消す（落下後の後始末）
        Destroy(gameObject, 2f);
    }
}
