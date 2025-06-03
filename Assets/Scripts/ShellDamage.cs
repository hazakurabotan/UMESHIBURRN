using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellDamage : MonoBehaviour
{
    public int damage = 1; // ���_���[�W�^���邩

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // PlayerController ���擾���ă_���[�W��^����
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // �e���g�͏�����
            Destroy(gameObject);
        }
        // �ǂ⑼�̂��̂ɓ�������������i�I�v�V�����j
        else if (other.CompareTag("Ground") || other.CompareTag("Block"))
        {
            Destroy(gameObject);
        }
    }
}
