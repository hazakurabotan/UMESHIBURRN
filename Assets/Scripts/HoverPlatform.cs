using UnityEngine;

public class HoverPlatform : MonoBehaviour
{
    public float hoverHeight = 1.2f;       // ����̏�ŕ��������������i�n�ʂ���̑��Βl�j
    public float hoverStrength = 20f;      // ������̕��́i�傫���قǃJ�`�b�Ǝ~�܂�j

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // �v���C���[�̌��݈ʒu�i����\�ʂ�Y���W�j
                float surfaceY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
                float diff = (surfaceY + hoverHeight) - collision.transform.position.y;

                // ���͂�^����i�o�l���ۂ�����j
                float force = diff * hoverStrength - rb.velocity.y * 8f;
                rb.AddForce(Vector2.up * force);

                // �I�v�V�����F�����łӂ�ӂ킳�������ꍇ�́��̐��l�����߂�
            }
        }
    }
}
