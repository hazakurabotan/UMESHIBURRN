using UnityEngine;

// ----------------------------------------------
// WindPlatform
// �v���C���[�����Ɓu�ӂ���v�Ə㏸�����镗�̑���M�~�b�N
// ----------------------------------------------
public class WindPlatform : MonoBehaviour
{
    public float liftForce = 10f;     // �v���C���[�ɗ^�����������x�i�W�����v�́j
    public float liftDuration = 0.3f; // �������ԁi���g�p�p�����[�^�A�g���p�j

    // --- �v���C���[���ڐG�������Ă���ԁi���t���[���j�Ă΂�� ---
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �v���C���[�ƐڐG��������������
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // ������ or ��~���̂݁A��������x��^����
                if (playerRb.velocity.y <= 0.1f)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, liftForce);
                }
            }
        }
    }
}
