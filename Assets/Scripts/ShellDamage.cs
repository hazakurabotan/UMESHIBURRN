using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �V�F���i�G�e��g�Q�e�Ȃǁj�̃_���[�W�����X�N���v�g
public class ShellDamage : MonoBehaviour
{
    public int damage = 1; // �v���C���[�ɗ^����_���[�W�ʁiInspector�Œ����\�j

    // ����Collider2D�i�����蔻��j�ƂԂ��������ɌĂ΂��
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ===== �v���C���[�ɓ��������� =====
        if (other.CompareTag("Player"))
        {
            // �v���C���[��PlayerController�X�N���v�g���擾
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // �v���C���[�Ƀ_���[�W��^����
                player.TakeDamage(damage);
            }

            // �e���g�͏���
            Destroy(gameObject);
        }
        // ===== �n�ʂ�u���b�N�Ȃǂɓ��������������� =====
        else if (other.CompareTag("Ground") || other.CompareTag("Block"))
        {
            Destroy(gameObject);
        }
    }
}
