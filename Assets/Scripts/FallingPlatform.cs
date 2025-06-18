using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 2f; // 乗ってから落ちるまでの秒数
    private Rigidbody2D rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            Invoke("DropPlatform", fallDelay);
        }
    }

    void DropPlatform()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, 2f); // ← さらに2秒後に床オブジェクトを消す
    }
}
