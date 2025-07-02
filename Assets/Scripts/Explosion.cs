using UnityEngine;

// ---------------------------------------------------------
// ExplosionScript
// �����G�t�F�N�g���v���C���[�ɓ���������_���[�W��^���A
// 1�b��Ɏ����Ŕ�\���ɂ���X�N���v�g
// ---------------------------------------------------------
public class ExplosionScript : MonoBehaviour
{
    // �v���C���[�ɗ^����_���[�W�ʁiInspector�Œ����\�j
    public int damage = 1;

    // �����̓����蔻��Ɂu�����v�����������ɌĂ΂��
    void OnTriggerEnter2D(Collider2D other)
    {
        // ����̃^�O���uPlayer�v���ǂ�������
        if (other.CompareTag("Player"))
        {
            // �Ԃ����������PlayerController���t���Ă���Ύ擾
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // �v���C���[�Ƀ_���[�W��^����
                player.TakeDamage(damage);
            }
        }
    }

    // �����I�u�W�F�N�g���\�����ꂽ�Ƃ��iactive�ɂȂ������j�ɌĂ΂��
    void OnEnable()
    {
        // 1�b���HideSelf()���Ăяo���i�����ŏ�����^�C�}�[�j
        Invoke(nameof(HideSelf), 1f);
    }

    // �����I�u�W�F�N�g���̂��\���ɂ���֐�
    void HideSelf()
    {
        gameObject.SetActive(false);
    }
}
