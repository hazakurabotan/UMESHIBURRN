using UnityEngine;

// �v���C���[���u�ォ��v������Ƃ����������鑫��
public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 2f;
    private Rigidbody2D rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Player�ƐڐG���܂������Ă��Ȃ�
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            // Player�̒��SY���W�Ƒ���̏��Y���W���r
            float playerBottomY = collision.collider.bounds.min.y + 0.05f; // �����]�T����������
            float platformTopY = GetComponent<Collider2D>().bounds.max.y - 0.05f;

            // �v���C���[�̑�������́u������v�Ȃ�A������Ƃ݂Ȃ��I
            if (playerBottomY > platformTopY)
            {
                isFalling = true;
                Invoke("DropPlatform", fallDelay);
            }
        }
    }

    void DropPlatform()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, 2f);
    }
}
