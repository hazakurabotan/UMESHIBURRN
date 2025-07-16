using UnityEngine;

// ボス戦イベント用トリガースクリプト
// プレイヤーがこのトリガーに触れると、イベント会話を開始してボス行動を一時停止させます
public class BossTrigger : MonoBehaviour
{
    public BossSimpleJump boss;               // 対象となるボスのスクリプト（Inspectorでアサイン）
    public DialogManager dialogManager;       // 会話管理スクリプト（Inspectorでアサイン）
    public DialogLine[] dialogLines;          // イベントで再生するセリフ一覧（Inspectorで登録）

    bool hasTriggered = false;                // 既に発動済みかどうかのフラグ（2重発動防止）

    // 他のコライダーがこのオブジェクトに入ったときに呼ばれる
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            boss.isActive = false;
            FindObjectOfType<PlayerController>().enabled = false;

            // ここで毎回DialogManagerを取得しなおす
            DialogManager manager = FindObjectOfType<DialogManager>();
            if (manager != null)
            {
                manager.StartDialog(dialogLines);
            }
            else
            {
                Debug.LogWarning("DialogManagerが見つかりませんでした！");
            }

            Destroy(gameObject);
        }
    }
}
