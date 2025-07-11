using UnityEngine;

// ----------------------------------------------
// Spike
// �g�Q���ȂǂɃA�^�b�`���āu�G�ꂽ��_���[�W�v����������
// ----------------------------------------------
public class Spike : MonoBehaviour
{
    public int damage = 1; // �󂯂�_���[�W�ʁi�C���X�y�N�^�[�ŕύX�\�j

    // --- �������g���K�[�ɓ������Ƃ������ŌĂ΂�� ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�������ꍇ�̂ݏ���
        if (other.CompareTag("Player"))
        {
            // PlayerController�X�N���v�g���擾
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // �w��_���[�W�Ԃ񌸂炷�iTakeDamage��Player���̊֐��j
                player.TakeDamage(damage);
            }
        }
    }
}
