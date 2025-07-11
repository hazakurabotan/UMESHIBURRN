using UnityEngine;

// ���̃X�N���v�g�́uPlayer�v�I�u�W�F�N�g�ɃA�^�b�`���܂�
// �G�iEnemy�^�O�j�ƂԂ��������� PlayerController ���̃_���[�W�������Ăяo���܂�

public class PlayerDamage : MonoBehaviour
{
    private PlayerController player; // PlayerController�X�N���v�g�̎Q��

    // ======= �I�u�W�F�N�g�������ɌĂ΂��i�ŏ���1�񂾂����s�����j=======
    void Awake()
    {
        // �����I�u�W�F�N�g�ɕt���Ă���PlayerController�X�N���v�g���擾
        player = GetComponent<PlayerController>();

        // �f�o�b�O�p���O
        Debug.Log("[PlayerDamage] �A�^�b�`����Ă���I�u�W�F�N�g: " + gameObject.name);
        Debug.Log("[PlayerDamage] PlayerController �����邩�H �� " + (player != null));

        // PlayerController��������Ȃ�������G���[�\��
        if (player == null)
        {
            Debug.LogError("PlayerController ��������܂���I�iAwake�j");
        }
    }

    // ======= 2D�g���K�[���m���Ԃ������Ƃ��ɌĂ΂�� =======
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����̃^�O��"Enemy"���ǂ�������
        if (other.CompareTag("Enemy"))
        {
            // ����I�u�W�F�N�g��Enemy�X�N���v�g���t���Ă��邩�`�F�b�N
            Enemy enemy = other.GetComponent<Enemy>();

            // Enemy�X�N���v�g������A���u���܂�Ă��Ȃ��v�G�����L��
            if (enemy != null && !enemy.isGrabbed)
            {
                // �v���C���[�R���g���[���[���������Ă���΃_���[�W���������s
                if (player != null)
                    player.TakeDamage(1); // �v���C���[��1�_���[�W
            }
        }
    }
}
