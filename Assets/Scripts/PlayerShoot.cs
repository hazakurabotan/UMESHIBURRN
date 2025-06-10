using UnityEngine;

// プレイヤーの弾発射（ショット・チャージショット）の制御スクリプト
public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;    // 発射する弾のプレハブ（Inspectorで指定）
    public Transform firePoint;        // 弾の発射位置（空の子オブジェクトなどをセット）
    public float bulletSpeed = 5f;     // 弾の速度

    [HideInInspector] public float chargeTime = 0f;    // チャージ中の時間（他スクリプトが参照用）
    [HideInInspector] public bool isCharging = false;  // チャージ中かどうか（他スクリプトが参照用）
    public float requiredCharge = 2.0f;                // チャージ弾に必要な長押し時間（秒）

    void Update()
    {
        // 無敵中の色変化はPlayerController側で管理。このスクリプトではしない。

        // Xボタン押し始めたらチャージ開始
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f; // チャージ時間リセット
        }

        // チャージ中（Xキー長押し中）
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime; // 経過時間を加算
            // 色演出はPlayerController.LateUpdateで処理
        }

        // Xキーを離した瞬間に弾発射
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            // チャージ時間がrequiredCharge（例:2秒）以上なら強化弾
            Shoot(chargeTime >= requiredCharge);
            isCharging = false;
            chargeTime = 0f;
        }
    }

    // 弾を発射する処理
    void Shoot(bool powered)
    {
        // 弾プレハブを発射位置に生成
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Rigidbody2Dで物理的に飛ばす
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // 弾本体のスクリプト（PlayerBullet）を取得
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // プレイヤーの向きに合わせて発射方向（右向き=1, 左向き=-1）
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            if (powered)
            {
                pb.damage = 3; // 強化弾（チャージショット）
                bullet.transform.localScale *= 4f; // 弾を4倍大きく（見た目のみ）
            }
            else
            {
                pb.damage = 1; // 通常弾
            }
        }

        // 弾は3秒後に自動消滅（画面外処理用）
        Destroy(bullet, 3.0f);
    }
}
