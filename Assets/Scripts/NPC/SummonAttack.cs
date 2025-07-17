using UnityEngine;

public class SummonAttack : MonoBehaviour
{
    public int damage = 1; // �^����_���[�W

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Enemy��EnemyController�������Ă���z��
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1, "summon");
            }
        }
    }
}
