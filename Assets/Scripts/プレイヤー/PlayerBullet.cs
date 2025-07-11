using UnityEngine;

// �v���C���[�����e�iPlayerBullet�j�̏����X�N���v�g
public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // �e�̃_���[�W�ʁi�����e�Ȃ�3�ȂǂɕύX��OK�j
    private bool hasHit = false; // ���łɉ����ɓ����������ǂ����i���d�q�b�g�h�~�j

    public void SetDamage(int value)
    {
        damage = value;
    }

    // ====== ����Collider2D�Ɠ��������Ƃ��Ɏ����ŌĂ΂�� ======
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���łɃq�b�g�ς݂Ȃ牽�����Ȃ��i���d�q�b�g�h�~�j
        if (hasHit) return;

        Debug.Log($"�e���q�b�g�I{other.gameObject.name}�i�^�O�F{other.tag}�j at {Time.time}");

        // ====== �G�ɓ��������Ƃ� ======
        if (other.CompareTag("Enemy"))
        {
            hasHit = true;  // ����ȏ㓖����Ȃ��悤�Ƀt���O�𗧂Ă�
            Debug.Log("�e���G�ɓ��������I" + Time.time);
            Debug.Log($"�U����{damage}�Ŕ��ˁI");

            Enemy enemy = other.GetComponent<Enemy>(); // Enemy�X�N���v�g�擾
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // �G�Ƀ_���[�W��^����
            }

            Destroy(this.gameObject); // �e������
        }
        // ====== �{�X�ɓ��������Ƃ� ======
        else if (other.CompareTag("Boss"))
        {
            hasHit = true;
            Debug.Log("�e���{�X�ɓ��������I" + Time.time);
            Debug.Log($"�U����{damage}�Ŕ��ˁI");

            // �{�X�{�̂̃X�N���v�g�iBossSimpleJump�j���擾
            BossSimpleJump boss = other.GetComponent<BossSimpleJump>();
            if (boss == null)
            {
                // �e�I�u�W�F�N�g���ɕt���Ă���ꍇ���l��
                boss = other.GetComponentInParent<BossSimpleJump>();
            }

            if (boss != null)
            {
                Debug.Log("TakeDamage�Ăяo���I");
                boss.TakeDamage(damage); // �{�X�Ƀ_���[�W��^����
            }
            else
            {
                Debug.Log("BossSimpleJump��������Ȃ�");
            }

            Debug.Log("Destroy���O�I");
            Destroy(this.gameObject); // �e������
        }
    }
}
