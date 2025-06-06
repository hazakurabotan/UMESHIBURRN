using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // �ʏ�_���[�W��1�B�����e�Ȃ�3�ɕύX
    private bool hasHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �ǂ�ȑ���ł��A���łɃq�b�g�ς݂Ȃ牽�����Ȃ��I
        if (hasHit) return;

        Debug.Log($"�e���q�b�g�I{other.gameObject.name}�i�^�O�F{other.tag}�j at {Time.time}");

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;  // Enemy�ɂ��L���ɂ����ق������S
            Debug.Log("�e���G�ɓ��������I" + Time.time);
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            hasHit = true;
            Debug.Log("�e���{�X�ɓ��������I" + Time.time);
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
            Debug.Log("Destroy���O�I");
            Destroy(this.gameObject);
        }
    }
}
