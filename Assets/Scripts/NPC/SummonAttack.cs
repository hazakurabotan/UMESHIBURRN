using UnityEngine;

public class SummonAttack : MonoBehaviour
{
    public int damage = 1; // 与えるダメージ

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // EnemyがEnemyControllerを持っている想定
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1, "summon");
            }
        }
    }
}
