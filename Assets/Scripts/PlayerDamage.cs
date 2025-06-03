using UnityEngine;

// このスクリプトは「Player」オブジェクトにアタッチします
// 敵（Enemyタグ）とぶつかった時に PlayerController 側のダメージ処理を呼び出します

public class PlayerDamage : MonoBehaviour
{
    private PlayerController player; // PlayerControllerスクリプトの参照

    // オブジェクト生成時に呼ばれる（最初に一回だけ実行される）
    void Awake()
    {
        // 同じオブジェクトに付いているPlayerControllerスクリプトを探す
        player = GetComponent<PlayerController>();

        // デバッグ用ログ
        Debug.Log("[PlayerDamage] アタッチされているオブジェクト: " + gameObject.name);
        Debug.Log("[PlayerDamage] PlayerController があるか？ → " + (player != null));

        // PlayerControllerが見つからなかったらエラー表示
        if (player == null)
        {
            Debug.LogError("PlayerController が見つかりません！（Awake）");
        }
    }

    // 何かとぶつかった時（2Dの当たり判定・IsTrigger用）
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 相手のタグが"Enemy"かどうか判定
        if (other.CompareTag("Enemy"))
        {
            // 相手オブジェクトにEnemyスクリプトが付いているかチェック
            Enemy enemy = other.GetComponent<Enemy>();

            // Enemyが見つかり、かつ enemy.isGrabbed が false の場合だけ
            if (enemy != null && !enemy.isGrabbed)
            {
                // プレイヤーコントローラーが見つかっていればダメージ処理を実行
                if (player != null)
                    player.TakeDamage(1);
            }
        }
    }
}
