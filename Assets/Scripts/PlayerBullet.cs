using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 1;  // 通常ダメージは1。強化弾なら3に変更

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("弾が敵に当たった！" + Time.time);
            // Enemyスクリプト（仮）にダメージ処理
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 敵にダメージを与える（敵スクリプト要作成）
            }
            Destroy(this.gameObject); // 弾は消す
        }
        else if (!other.isTrigger && !other.CompareTag("Player"))
        {
            Destroy(this.gameObject); // 壁等に当たったら消す
        }
    }
}
