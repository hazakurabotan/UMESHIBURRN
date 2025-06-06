using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // 通常ダメージは1。強化弾なら3に変更

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("弾が敵に当たった！" + Time.time);
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
        // ▼ここを追加！
        // ここで「当たった相手のタグが"Boss"だけ」に限定
        else if (other.CompareTag("Boss"))
        {
            Debug.Log("弾がボスに当たった！" + Time.time);
            // まず自分、次に親にもBossSimpleJumpがあるか探す
            BossSimpleJump boss = other.GetComponent<BossSimpleJump>();
            if (boss == null) boss = other.GetComponentInParent<BossSimpleJump>();

            if (boss != null)
            {
                Debug.Log("TakeDamage呼び出し！");
                boss.TakeDamage(damage);
            }
            else
            {
                Debug.Log("BossSimpleJumpが見つからない");
            }
            Destroy(this.gameObject);
        }
    }
}
