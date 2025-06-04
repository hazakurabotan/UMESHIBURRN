using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;    // �e�̃v���n�u
    public Transform firePoint;        // ���ˈʒu
    public float bulletSpeed = 5f;     // �e�̑��x

    [HideInInspector] public float chargeTime = 0f; // �`���[�W���̎��ԁiPlayerController���Q�Ɓj
    [HideInInspector] public bool isCharging = false; // �`���[�W�����ǂ����iPlayerController���Q�Ɓj
    public float requiredCharge = 2.0f; // 2�b�ȏ�ŋ����e�iPlayerController���Q�Ɓj

    void Update()
    {
        // ���G���o�����܂߂ĐF�����PlayerController�ɔC����i�����ł͂��Ȃ��j

        // X�{�^�������n�߂���`���[�W�J�n
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        // �`���[�W��
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime;
            // �F��PlayerController.LateUpdate�œ_�Ő���
        }

        // �{�^�����������ɔ���
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            Shoot(chargeTime >= requiredCharge); // 2�b�ȏ�Ȃ狭���e
            isCharging = false;
            chargeTime = 0f;
            // �F�������ł͐G��Ȃ�
        }
    }

    void Shoot(bool powered)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // �v���C���[�̌����ō��E�Ɍ���
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            if (powered)
            {
                pb.damage = 3; // �����e
                bullet.transform.localScale *= 4f;
            }
            else
            {
                pb.damage = 1; // �ʏ�e
            }
        }

        Destroy(bullet, 3.0f);
    }
}
