using UnityEngine;

// �G�L�����N�^�[�p�̊�{�X�N���v�g
public class Enemy : MonoBehaviour
{
    public int hp = 3;               // �G�̗̑́iHP�j
    public bool isGrabbed = false;   // �͂܂�Ă����Ԃ��i���݃A�N�V�����p�t���O�j
    public bool isFlying = false;    // ���ł����Ԃ��i������ꒆ�Ȃǁj

    private bool recentlyHit = false;  // �A���q�b�g�h�~�p�t���O

    // ======= �_���[�W���� =======
    public void TakeDamage(int amount)
    {
        // �A���q�b�g�h�~�i�U���̓����蔻�����u�����������j
        if (recentlyHit) return;               // ���łɃq�b�g���蒆�Ȃ牽�����Ȃ�
        recentlyHit = true;                    // �q�b�g�t���OON
        Invoke(nameof(ResetHit), 0.05f);       // 0.05�b��Ƀt���O�����i�����\�j

        // �͂܂�Ă���Œ��͖��G�i�������u���ł���v�ꍇ�̓_���[�W�L���j
        if (isGrabbed && !isFlying) return;

        // HP�����炷
        hp -= amount;
        Debug.Log("Enemy�� " + amount + " �_���[�W�I ����HP: " + hp);

        // HP��0�ȉ��ɂȂ����玀�S����
        if (hp <= 0)
        {
            Die();
        }
    }

    // ======= �A���q�b�g�����p�֐� =======
    private void ResetHit()
    {
        recentlyHit = false; // �ĂэU�����󂯕t����
    }

    // ======= ���S���� =======
    private void Die()
    {
        Destroy(gameObject); // ���̃I�u�W�F�N�g�i�G�j������
    }
}
