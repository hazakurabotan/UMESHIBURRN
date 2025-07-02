using UnityEngine;

// ---------------------------------------------------------
// HoverPlatform
// ���̃X�N���v�g��t�����I�u�W�F�N�g�́A
// �v���C���[����ɏ�����Ƃ��Ɂu�ӂ���v�ƕ��͂�^����
// �i��F�ӂ�ӂ푫��A�z�o�[�v���b�g�t�H�[���j
// ---------------------------------------------------------
public class HoverPlatform : MonoBehaviour
{
    public float hoverHeight = 1.2f;       // ���������������i����\�ʂ���̋����j
    public float hoverStrength = 20f;      // ���͂̋����i�傫���قǃr�^�~�܂�j

    // �v���C���[�����̑���ɏ���Ă�ԁA���t���[���Ă΂��
    void OnCollisionStay2D(Collision2D collision)
    {
        // ��������Ă�̂�Player�^�O�t���I�u�W�F�N�g�Ȃ�
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[��Rigidbody2D�i��������j���擾
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // ����\�ʂ�Y���W�����߂�i�����̈ʒu�{�R���C�_�[�����̍����j
                float surfaceY = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;

                // �ڕW�̕��������������Ƃ̍������v�Z
                float diff = (surfaceY + hoverHeight) - collision.transform.position.y;

                // ���͂��v�Z
                // �ڕW�����܂ŉ����グ��� �| ���݂̗������x�ɔ�������i�����I�j
                float force = diff * hoverStrength - rb.velocity.y * 8f;

                // ������ɗ͂�������i�o�l�̂悤�ȃC���[�W�j
                rb.AddForce(Vector2.up * force);

                // ���ӂ�ӂ튴��J�`�b�Ǝ~�߂����ꍇ��hoverStrength�⑬�x�ւ̌����l�𒲐��I
            }
        }
    }
}
