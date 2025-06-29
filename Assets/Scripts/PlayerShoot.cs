using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;

    [HideInInspector] public float chargeTime = 0f;
    [HideInInspector] public bool isCharging = false;
    public float requiredCharge = 2.0f;

    public int maxShots = 3;             // 連射できる回数
    public float reloadTime = 1.0f;      // クールタイム
    private int shotsFired = 0;
    private float lastFireTime = -99f;
    private bool isReloading = false;

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isReloading)
        {
            if (Time.time - lastFireTime > reloadTime)
            {
                shotsFired = 0;
                isReloading = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (shotsFired < maxShots)
            {
                isCharging = true;
                chargeTime = 0f;
            }
            else
            {
                isReloading = true;
                lastFireTime = Time.time;
                // ここでバテSE・演出いれても◎
            }
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
            shotsFired++;
            if (shotsFired >= maxShots)
            {
                isReloading = true;
                lastFireTime = Time.time;
            }
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
            if (playerController != null)
                baseDamage = playerController.bulletDamage;

            if (powered)
            {
                pb.damage = baseDamage + 2;
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
