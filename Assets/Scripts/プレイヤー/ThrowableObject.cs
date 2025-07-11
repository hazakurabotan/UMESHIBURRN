using UnityEngine;

// �v���C���[���������镨�́i���E��Ȃǁj�̃X�N���v�g
public class ThrowableObject : MonoBehaviour
{
    public int damage = 1;           // �������Ƃ��ɗ^����_���[�W
    private bool isThrown = false;   // ������ꒆ���ǂ����i�e��ԃt���O�j

    // ==== ������ꂽ�u�ԂɌĂ΂�郁�\�b�h ====
    public void ActivateAsProjectile()
    {
        isThrown = true;             // �u�e�v�Ƃ��ăA�N�e�B�u��
        Invoke("Deactivate", 1.0f);  // 1�b��Ɏ����Œe�t���OOFF
    }

    // ==== �e��Ԃ��I�����鏈�� ====
    void Deactivate()
    {
        isThrown = false;
    }

    // ==== �����ɂԂ��������ɌĂ΂��i�����Փ˔���j ====
    void OnCollisionEnter2D(Collision2D collision)
    {
        // �������Ă��Ȃ���ԂȂ牽�����Ȃ�
        if (!isThrown) return;

        // �G�iEnemy�^�O�j�ɓ��������ꍇ�����_���[�W��^����
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // �G�Ƀ_���[�W�I
            }
            isThrown = false; // 1�񓖂�����������_���[�W��^���Ȃ�
        }
    }
}
