using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;

    [HideInInspector] public float chargeTime = 0f;
    [HideInInspector] public bool isCharging = false;
    public float requiredCharge = 2.0f;

    // ここでPlayerController参照を保持
    PlayerController playerController;

    void Start()
    {
        // 最初にキャッシュ（同じオブジェクトについてる前提）
        playerController = GetComponent<PlayerController>();
        // もし別オブジェクトの場合はFindObjectOfType<PlayerController>()でも可
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime;
        }

        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            Shoot(chargeTime >= requiredCharge);
            isCharging = false;
            chargeTime = 0f;
        }
    }

    void Shoot(bool powered)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            int baseDamage = 1;
            // プレイヤーの bulletDamage を参照
            if (playerController != null)
                baseDamage = playerController.bulletDamage;

            if (powered)
            {
                pb.damage = baseDamage + 2; // チャージ時は+2
                bullet.transform.localScale *= 4f;
            }
            else
            {
                pb.damage = baseDamage;
            }
        }

        Destroy(bullet, 3.0f);
    }
}
