using UnityEngine;

// 敵キャラクター用の基本スクリプト
public class Enemy : MonoBehaviour
{
    public int hp = 3;               // 敵の体力（HP）
    public GameObject[] dropItemPrefabs;
    public bool isGrabbed = false;   // 掴まれている状態か（つかみアクション用フラグ）
    public bool isFlying = false;    // 飛んでいる状態か（投げられ中など）

    private bool recentlyHit = false;  // 連続ヒット防止用フラグ

    // ======= ダメージ処理 =======
    public void TakeDamage(int amount)
    {
        // 連続ヒット防止（攻撃の当たり判定を一瞬だけ無効化）
        if (recentlyHit) return;               // すでにヒット判定中なら何もしない
        recentlyHit = true;                    // ヒットフラグON
        Invoke(nameof(ResetHit), 0.05f);       // 0.05秒後にフラグ解除（調整可能）

        // 掴まれている最中は無敵（ただし「飛んでいる」場合はダメージ有効）
        if (isGrabbed && !isFlying) return;

        // HPを減らす
        hp -= amount;
        Debug.Log("Enemyに " + amount + " ダメージ！ 現在HP: " + hp);

        // HPが0以下になったら死亡処理
        if (hp <= 0)
        {
            Die();
        }
    }




    // ======= 連続ヒット解除用関数 =======
    private void ResetHit()
    {
        recentlyHit = false; // 再び攻撃を受け付ける
    }

    // ======= 死亡処理 =======
    private void Die()
    {
        // 1. ドロップ用：アイテムプレハブ3つ登録用の配列を用意
        // （Inspectorからセットする。publicにしておく）
        // public GameObject[] dropItemPrefabs; ← 上に追記する

        // 2. 50%確率でランダムドロップ
        if (dropItemPrefabs != null && dropItemPrefabs.Length > 0)
        {
            if (Random.value < 0.5f)
            {
                int itemType = Random.Range(0, dropItemPrefabs.Length); // 0,1,2どれか
                Instantiate(dropItemPrefabs[itemType], transform.position, Quaternion.identity);
            }
        }

        // 3. 既存の「キル数加算」
        if (GameManager.Instance != null)
            GameManager.Instance.AddKill();

        Destroy(gameObject); // このオブジェクト（敵）を消す
    }
}
