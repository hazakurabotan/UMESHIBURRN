using UnityEngine;

/// <summary>
/// プレイヤーから発射されるロープの挙動管理
/// - 出現時にアニメーションしながら伸びていく
/// - 敵や箱に当たったら引き寄せ or 投げる
/// </summary>
public class Rope : MonoBehaviour
{
    public float extendSpeed = 8f;   // ロープが伸びる速さ
    public float maxLength = 4f;     // ロープの最大長さ

    public float pullSpeed = 5f;     // 引き寄せ速度
    public float throwForce = 55f;   // 投げる力

    private float currentLength = 0.1f; // 今の長さ
    private bool extending = true;      // 伸びている途中か

    private GameObject target;          // 掴んだ対象
    private Transform player;           // プレイヤー参照
    private bool holding = false;       // 掴んでるか
    private bool grabbed = false;       // 掴めたか

    void Start()
    {
        // プレイヤー取得
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // プレイヤーの向きでロープの位置・向きを少し前方にずらす
        float direction = Mathf.Sign(player.localScale.x); // 1=右, -1=左

        // プレイヤーの少し前に配置
        transform.position = player.position + new Vector3(0.5f * direction, 0, 0);

        // 向き調整
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;

        // --- 伸び始めの初期化 ---
        // 最初は短くしておく（中心Pivotの場合は0.1fが無難）
        scale = transform.localScale;
        scale.x = 0.1f * direction;
        transform.localScale = scale;
        currentLength = 0.1f;
        extending = true;

        // 掴めなかったら1秒で自動消去
        Invoke(nameof(CheckIfNotGrabbed), 1f);
    }

    void Update()
    {
        // --- ロープを伸ばす処理 ---
        if (extending)
        {
            currentLength += extendSpeed * Time.deltaTime;
            if (currentLength >= maxLength)
            {
                currentLength = maxLength;
                extending = false; // 伸びきった
            }
            Vector3 scale = transform.localScale;
            scale.x = currentLength * Mathf.Sign(scale.x);
            transform.localScale = scale;
        }

        // --- 何かを掴んでいる時の処理 ---
        if (holding && target != null)
        {
            // 対象をプレイヤーに引き寄せる
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
    /// 掴んだ対象を投げる
    /// </summary>
    void Throw()
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // プレイヤーの向きに投げる
            Vector2 dir = new Vector2(Mathf.Sign(player.localScale.x), 0).normalized;
            rb.velocity = dir * throwForce;

            // 敵だった場合の追加処理
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = false;
                enemy.isFlying = true;
            }

            // 投げられるオブジェクトだった場合の追加処理
            ThrowableObject to = target.GetComponent<ThrowableObject>();
            if (to != null) to.ActivateAsProjectile();
        }

        // ロープを消す
        Destroy(gameObject);
    }

    /// <summary>
    /// ロープの先端が何かに当たったとき
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null) return; // 既に掴んでいれば無視

        if (other.CompareTag("Enemy") || other.CompareTag("Throwable"))
        {
            target = other.gameObject;
            holding = true;
            grabbed = true;
            extending = false; // 掴んだら伸ばし処理終了

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            // 敵なら掴まれ状態にする
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = true;
            }
        }
    }

    /// <summary>
    /// 何も掴めなかった時の自動消去
    /// </summary>
    void CheckIfNotGrabbed()
    {
        if (!grabbed)
        {
            Destroy(gameObject);
        }
    }
}
