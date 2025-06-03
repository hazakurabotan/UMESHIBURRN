using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // �ʏ�_���[�W��1�B�����e�Ȃ�3�ɕύX

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("�e���G�ɓ��������I" + Time.time);
            // Enemy�X�N���v�g�i���j�Ƀ_���[�W����
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // �G�Ƀ_���[�W��^����i�G�X�N���v�g�v�쐬�j
            }
            Destroy(this.gameObject); // �e�͏���
        }
        else if (!other.isTrigger && !other.CompareTag("Player"))
        {
            Destroy(this.gameObject); // �Ǔ��ɓ������������
        }
    }
}
