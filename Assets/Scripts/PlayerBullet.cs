using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // 通常ダメージは1。強化弾なら3に変更
    private bool hasHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // どんな相手でも、すでにヒット済みなら何もしない！
        if (hasHit) return;

        Debug.Log($"弾がヒット！{other.gameObject.name}（タグ：{other.tag}） at {Time.time}");

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;  // Enemyにも有効にしたほうが安全
            Debug.Log("弾が敵に当たった！" + Time.time);
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
            Debug.Log("弾がボスに当たった！" + Time.time);
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
            Debug.Log("Destroy直前！");
            Destroy(this.gameObject);
        }
    }
}
