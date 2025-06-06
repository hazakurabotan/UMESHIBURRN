using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // �ʏ�_���[�W��1�B�����e�Ȃ�3�ɕύX

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("�e���G�ɓ��������I" + Time.time);
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
        // ��������ǉ��I
        // �����Łu������������̃^�O��"Boss"�����v�Ɍ���
        else if (other.CompareTag("Boss"))
        {
            Debug.Log("�e���{�X�ɓ��������I" + Time.time);
            // �܂������A���ɐe�ɂ�BossSimpleJump�����邩�T��
            BossSimpleJump boss = other.GetComponent<BossSimpleJump>();
            if (boss == null) boss = other.GetComponentInParent<BossSimpleJump>();

            if (boss != null)
            {
                Debug.Log("TakeDamage�Ăяo���I");
                boss.TakeDamage(damage);
            }
            else
            {
                Debug.Log("BossSimpleJump��������Ȃ�");
            }
            Destroy(this.gameObject);
        }
    }
}
