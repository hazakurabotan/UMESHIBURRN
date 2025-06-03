using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // �e�̃v���n�u
    public Transform firePoint;      // ���ˈʒu
    public float bulletSpeed = 5f;   // �e�̑��x

    private float chargeTime = 0f;
    private bool isCharging = false;
    private float requiredCharge = 3.0f; // 3�b�ȏ�ŋ����e

    private SpriteRenderer sr; // �v���C���[��SpriteRenderer

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // X�{�^�������n��
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f;
        }

        // �����Ă�Ԃ̓J�E���g�{�_�ŉ��o
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime;

            // �_��
            if (sr != null)
            {
                float blink = Mathf.PingPong(Time.time * 10f, 1f);
                if (chargeTime >= requiredCharge)
                    sr.color = Color.red; // �����e�Ȃ�Ԃ��_��
                else
                    sr.color = Color.yellow;
            }
        }

        // �{�^�����������ɔ���
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            Shoot(chargeTime >= requiredCharge); // 3�b�ȏ�Ȃ狭���e
            isCharging = false;
            chargeTime = 0f;
            if (sr != null) sr.color = Color.white; // �F��߂�
        }
    }

    void Shoot(bool powered)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // �E���� or ������
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            if (powered)
            {
                pb.damage = 3; // �_���[�W3
                bullet.transform.localScale *= 3f; // �傫������
            }
            else
            {
                pb.damage = 1;
                // �ʏ�̑傫���i�O�̂��߃��Z�b�g��OK�j
                // bullet.transform.localScale = Vector3.one;
            }
        }

        Destroy(bullet, 3.0f);
    }
}
