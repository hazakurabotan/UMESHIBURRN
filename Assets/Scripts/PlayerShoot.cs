using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;    // 弾のプレハブ
    public Transform firePoint;        // 発射位置
    public float bulletSpeed = 5f;     // 弾の速度

    [HideInInspector] public float chargeTime = 0f; // チャージ中の時間（PlayerControllerが参照）
    [HideInInspector] public bool isCharging = false; // チャージ中かどうか（PlayerControllerが参照）
    public float requiredCharge = 2.0f; // 2秒以上で強化弾（PlayerControllerが参照）

    void Update()
    {
        // 無敵演出中も含めて色操作はPlayerControllerに任せる（ここではしない）

        // Xボタン押し始めたらチャージ開始
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        // チャージ中
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime;
            // 色はPlayerController.LateUpdateで点滅制御
        }

        // ボタン離した時に発射
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            Shoot(chargeTime >= requiredCharge); // 2秒以上なら強化弾
            isCharging = false;
            chargeTime = 0f;
            // 色もここでは触らない
        }
    }

    void Shoot(bool powered)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // プレイヤーの向きで左右に撃つ
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            if (powered)
            {
                pb.damage = 3; // 強化弾
                bullet.transform.localScale *= 4f;
            }
            else
            {
                pb.damage = 1; // 通常弾
            }
        }

        Destroy(bullet, 3.0f);
    }
}
