using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 2f; // ����Ă��痎����܂ł̕b��
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
        Destroy(gameObject, 2f); // �� �����2�b��ɏ��I�u�W�F�N�g������
    }
}
