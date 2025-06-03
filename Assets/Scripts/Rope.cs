using UnityEngine;

public class Rope : MonoBehaviour
{
    public float pullSpeed = 5f;     // 引き寄せ速度
    public float throwForce = 55f;   // 投げる力

    private GameObject target;       // 掴んだ対象
    private Transform player;        // プレイヤー参照
    private bool holding = false;    // 掴んでるか
    private bool grabbed = false;    // 掴めたかどうか（new!）

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // プレイヤーの向いてる方向でロープの位置を少し前方にずらす
        float direction = Mathf.Sign(player.localScale.x); // 1.0f = 右, -1.0f = 左

        // ロープの位置をプレイヤーの前に少しずらす
        transform.position = player.position + new Vector3(0.5f * direction, 0, 0);

        // ロープの向きを調整（例えば、SpriteRendererの向きを変えたいなら）
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;

        // 掴めなかったら1秒で消える
        Invoke(nameof(CheckIfNotGrabbed), 1f);
    }

    void Update()
    {
        if (holding && target != null)
        {
            // プレイヤーに引き寄せる
            target.transform.position = Vector2.MoveTowards(
                target.transform.position,
                player.position,
                pullSpeed * Time.deltaTime
            );

            // 離したら投げる！
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
            // 💡ここで「現在の向き」で投げ方向を決める！
            Vector2 dir = new Vector2(Mathf.Sign(player.localScale.x), 0).normalized;

            Debug.Log("[Rope] 投げ方向: " + dir);
            rb.velocity = dir * throwForce;

            Debug.Log("[Rope] velocity 設定完了");

            // 投げフラグON（敵なら）
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = false;
                enemy.isFlying = true;
            }

            // 投げられる箱など
            ThrowableObject to = target.GetComponent<ThrowableObject>();
            if (to != null) to.ActivateAsProjectile();
        }

        Destroy(gameObject); // ロープを消す
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null) return;

        if (other.CompareTag("Enemy") || other.CompareTag("Throwable"))
        {
            target = other.gameObject;
            holding = true;
            grabbed = true; // ← 掴めた！

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            // 敵なら掴まれ状態にする
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isGrabbed = true;
                Debug.Log("[Rope] 敵を掴んだ！");
            }
        }
    }

    void CheckIfNotGrabbed()
    {
        if (!grabbed)
        {
            Debug.Log("[Rope] 掴めなかったので自動削除");
            Destroy(gameObject);
        }
    }
}
