using UnityEngine;

/// <summary>
/// プレイヤーから発射されるロープの挙動管理
/// - 出現時にアニメーションしながら伸びていく
/// - 敵や箱に当たったら引き寄せ or 投げる
/// </summary>
public class Rope : MonoBehaviour
{
    public float extendSpeed = 8f;    // ロープが伸びる速さ
    public float maxLength = 4f;      // ロープの最大長さ

    public float pullSpeed = 5f;      // 引き寄せる速さ
    public float throwForce = 55f;    // 投げる時の力

    private float currentLength = 0.1f; // 今の長さ
    private bool extending = true;      // 伸びている途中かどうか

    private GameObject target;          // 掴んだ対象（敵や箱）
    private Transform player;           // プレイヤーの参照
    private bool holding = false;       // 今つかんでいるか
    private bool grabbed = false;       // 掴めたか（成功フラグ）

    void Start()
    {
        // プレイヤーをタグで探して取得
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // プレイヤーの向き（localScale.x）で右向き=1, 左向き=-1
        float direction = Mathf.Sign(player.localScale.x);

        // プレイヤーの少し前にロープを配置
        transform.position = player.position + new Vector3(0.5f * direction, 0, 0);

        // 向き調整（スプライトが左右どちらでも正しく伸びるように）
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;

        // 伸ばし始めの初期化（最初は細い状態で作る）
        scale = transform.localScale;
        scale.x = 0.1f * direction;
        transform.localScale = scale;
        currentLength = 0.1f;
        extending = true;

        // 掴めなかった場合は1秒後に自動消去
        Invoke(nameof(CheckIfNotGrabbed), 1f);
    }

    void Update()
    {
        // === ロープを伸ばす処理 ===
        if (extending)
        {
            currentLength += extendSpeed * Time.deltaTime; // 伸ばす
            if (currentLength >= maxLength)
            {
                currentLength = maxLength;
                extending = false; // 最大まで伸ばしたらストップ
            }
            Vector3 scale = transform.localScale;
            scale.x = currentLength * Mathf.Sign(scale.x); // 伸びた長さに調整
            transform.localScale = scale;
        }

        // === 何かを掴んでいる時 ===
        if (holding && target != null)
        {
            // 対象をプレイヤーの方へ移動させる（引き寄せ）
            target.transform.position = Vector2.MoveTowards(
                target.transform.position,
                player.position,
                pullSpeed * Time.deltaTime
            );

            // Cキーを離したら投げる
            if (Input.GetKeyUp(KeyCode.C))
            {
                Throw();
            }
        }
    }

    /// <summary>
    /// 掴んだ対象を投げる処理
    /// </summary>
    void Throw()
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // プレイヤーの向きに投げる（右向き=1, 左向き=-1）
            Vector2 dir = new Vector2(Mathf.Sign(player.localScale.x), 0).normalized;
            rb.velocity = dir * throwForce;

            // 敵だった場合は「掴まれ解除」「投げられ中」フラグを立てる
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = false;
                enemy.isFlying = true;
            }

            // 投げオブジェクトの場合は特別な処理
            ThrowableObject to = target.GetComponent<ThrowableObject>();
            if (to != null) to.ActivateAsProjectile();
        }

        // ロープ本体は消す
        Destroy(gameObject);
    }

    /// <summary>
    /// ロープの先端が何かに当たった時の処理
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 既に何か掴んでいれば何もしない
        if (target != null) return;

        // 敵か「投げられる箱」に当たった場合
        if (other.CompareTag("Enemy") || other.CompareTag("Throwable"))
        {
            target = other.gameObject;
            holding = true;
            grabbed = true;
            extending = false; // もう伸ばさない

            // 当たった物体の動きを止める
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            // 敵なら「掴まれ」フラグON
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = true;
            }
        }
    }

    /// <summary>
    /// 何も掴めなかった時は自動消滅（生成1秒後に判定）
    /// </summary>
    void CheckIfNotGrabbed()
    {
        if (!grabbed)
        {
            Destroy(gameObject);
        }
    }
}
