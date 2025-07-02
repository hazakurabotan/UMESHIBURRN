using UnityEngine;

// ----------------------------------------------
// PlayerShoot
// �v���C���[�̒e���ˁi�A�ˁE�`���[�W�E�����[�h�Ή��j���Ǘ�����X�N���v�g
// ----------------------------------------------
public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;    // �e�v���n�u�i�e�̌��ƂȂ�I�u�W�F�N�g�j
    public Transform firePoint;        // ���ˈʒu�iTransform�j
    public float bulletSpeed = 5f;     // �e�̑��x

    [HideInInspector] public float chargeTime = 0f;    // �`���[�W���ԁi���J�����C���X�y�N�^�[��\���j
    [HideInInspector] public bool isCharging = false;  // �`���[�W���t���O
    public float requiredCharge = 2.0f;                // �t���`���[�W�ɕK�v�ȕb��

    public int maxShots = 3;         // �A�˂ł���񐔁i�e�������j
    public float reloadTime = 1.0f;  // �e�؂��̃����[�h����
    private int shotsFired = 0;      // ���ݘA�˂�����
    private float lastFireTime = -99f;   // �Ō�Ɍ��������ԁi�����[�h����p�j
    private bool isReloading = false;    // �����[�h�����ǂ���

    PlayerController playerController;   // �v���C���[�{�̂̏��擾�p

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // �����[�h���̓����[�h�^�C�����o�߂���܂Ō��ĂȂ�
        if (isReloading)
        {
            if (Time.time - lastFireTime > reloadTime)
            {
                shotsFired = 0;      // �e�����Z�b�g
                isReloading = false; // �����[�h����
            }
            return; // �����I��
        }

        // X�L�[�����n�߂����Ƀ`���[�W�J�n or �e�؂�Ȃ烊���[�h��
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
                // �����Ńo�eSE�E�G�t�F�N�g���������
            }
        }

        // �`���[�W����X���������ςȂ��Ȃ玞�Ԃ����Z
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime;
        }

        // X�L�[�𗣂����甭��
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            Shoot(chargeTime >= requiredCharge); // �`���[�W�����܂��Ă�����p���[�V���b�g
            isCharging = false;
            chargeTime = 0f;
            shotsFired++; // 1������

            // �ő唭�ː��ɒB�����烊���[�h�J�n
            if (shotsFired >= maxShots)
            {
                isReloading = true;
                lastFireTime = Time.time;
            }
        }
    }

    // --- �e�𔭎˂��鏈���ipowered=true�Ńp���[�V���b�g�j---
    void Shoot(bool powered)
    {
        // �e�𔭐������ď����ʒu�E���x��^����
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // �v���C���[�̌����i���[�J���X�P�[��X�j�ō��E���ː؂�ւ�
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        // �_���[�W��傫�����؂�ւ�
        if (pb != null)
        {
            int baseDamage = 1;
            if (playerController != null)
                baseDamage = playerController.bulletDamage;

            if (powered)
            {
                pb.damage = baseDamage + 2;         // �p���[�V���b�g�́{�Q�_���[�W
                bullet.transform.localScale *= 4f;  // �T�C�Y���傫��
            }
            else
            {
                pb.damage = baseDamage;
            }
        }
        Destroy(bullet, 3.0f); // 3�b��ɒe�������i���������[�N�h�~�j
    }
}
