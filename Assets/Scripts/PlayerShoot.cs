using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // 弾のプレハブ
    public Transform firePoint;      // 発射位置
    public float bulletSpeed = 5f;   // 弾の速度

    private float chargeTime = 0f;
    private bool isCharging = false;
    private float requiredCharge = 3.0f; // 3秒以上で強化弾

    private SpriteRenderer sr; // プレイヤーのSpriteRenderer

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Xボタン押し始め
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        // 押してる間はカウント＋点滅演出
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime;

            // 点滅
            if (sr != null)
            {
                float blink = Mathf.PingPong(Time.time * 10f, 1f);
                if (chargeTime >= requiredCharge)
                    sr.color = Color.red; // 強化弾なら赤く点滅
                else
                    sr.color = Color.yellow;
            }
        }

        // ボタン離した時に発射
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            Shoot(chargeTime >= requiredCharge); // 3秒以上なら強化弾
            isCharging = false;
            chargeTime = 0f;
            if (sr != null) sr.color = Color.white; // 色を戻す
        }
    }

    void Shoot(bool powered)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // 右向き or 左向き
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            if (powered)
            {
                pb.damage = 3; // ダメージ3
                bullet.transform.localScale *= 3f; // 大きくする
            }
            else
            {
                pb.damage = 1;
                // 通常の大きさ（念のためリセットもOK）
                // bullet.transform.localScale = Vector3.one;
            }
        }

        Destroy(bullet, 3.0f);
    }
}
