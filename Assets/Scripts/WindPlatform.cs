using UnityEngine;

public class WindPlatform : MonoBehaviour
{
    public float liftForce = 10f;  // ������ɗ^�����
    public float liftDuration = 0.3f; // �������ԁi�b�j

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // �v���C���[���������ɑ��x�������Ă��鎞���������I�ɏ������
                if (playerRb.velocity.y <= 0.1f)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, liftForce);
                }
            }
        }
    }
}
