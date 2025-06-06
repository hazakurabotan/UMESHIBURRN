using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public bool isGrabbed = false;
    public bool isFlying = false;

    private bool recentlyHit = false;  // ★追加

    public void TakeDamage(int amount)
    {
        // 連続ヒット防止
        if (recentlyHit) return;           // すでにヒット中なら無視
        recentlyHit = true;                // これ以上のヒットを防止
        Invoke(nameof(ResetHit), 0.05f);   // 0.05秒後に解除（適宜調整）

        // 掴まれてる最中だけ無敵
        if (isGrabbed && !isFlying) return;

        hp -= amount;
        Debug.Log("Enemyに " + amount + " ダメージ！ 現在HP: " + hp);


        if (hp <= 0)
        {
            Die();
        }
    }
    private void ResetHit()
    {
        recentlyHit = false;
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}
