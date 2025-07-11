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
        // まだ発動していなくて、かつ「Player」タグのオブジェクトが触れた場合のみ発動
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;              // 2重発動防止のためフラグを立てる

            boss.isActive = false;            // ボスの行動を一時停止（会話イベント中に暴れないように）

            // プレイヤー操作禁止（会話中は動けなくする）
            FindObjectOfType<PlayerController>().enabled = false;

            // 会話イベント開始（DialogManagerに会話内容を渡す）
            dialogManager.StartDialog(dialogLines);

            Destroy(gameObject);              // このトリガーは一度使ったら消す（何度も発動しないように）
        }
    }
}
