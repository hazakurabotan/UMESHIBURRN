using UnityEngine;

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

    Vector2 moveDirection = Vector2.right; // デフォルト

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        float direction = Mathf.Sign(player.localScale.x);

        // プレイヤーの少し前
        transform.position = player.position + new Vector3(0.5f * direction, 0, 0);

        // 初期向きでscale,rotationセット
        SetDirection(moveDirection);

        // 伸ばし始め
        Vector3 scale = transform.localScale;
        scale.x = 0.1f;
        transform.localScale = scale;
        currentLength = 0.1f;
        extending = true;

        Invoke(nameof(CheckIfNotGrabbed), 1f);
    }

    // 呼び出し時に進行方向と見た目を指定
    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
        // スプライトの向きも回転
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        // スプライトが任意方向に伸びる
        if (extending)
        {
            currentLength += extendSpeed * Time.deltaTime;
            if (currentLength >= maxLength)
            {
                currentLength = maxLength;
                extending = false;
            }
            Vector3 scale = transform.localScale;
            scale.x = currentLength;
            transform.localScale = scale;
        }

        // ロープ自体を移動
        transform.position += (Vector3)(moveDirection * extendSpeed * Time.deltaTime);

        // 引っ張り中
        if (holding && target != null)
        {
            target.transform.position = Vector2.MoveTowards(
                target.transform.position,
                player.position,
                pullSpeed * Time.deltaTime
            );

            if (Input.GetKeyUp(KeyCode.C))
            {
                Throw();
            }
        }
    }

    void Throw()
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = moveDirection;
            rb.velocity = dir.normalized * throwForce;

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = false;
                enemy.isFlying = true;
            }

            ThrowableObject to = target.GetComponent<ThrowableObject>();
            if (to != null) to.ActivateAsProjectile();
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null) return;

        if (other.CompareTag("Block"))
        {
            // ロープの現在座標をぶら下がりポイントにする
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (player != null)
            {
                player.StartHangFromRope(transform.position);
            }
            // ロープ本体を消す
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy") || other.CompareTag("Throwable"))
        {
            target = other.gameObject;
            holding = true;
            grabbed = true;
            extending = false;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null) enemy.isGrabbed = true;
        }
    }

    void CheckIfNotGrabbed()
    {
        if (!grabbed)
        {
            Destroy(gameObject);
        }
    }
}
