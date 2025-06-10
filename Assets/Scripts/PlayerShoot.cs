using UnityEngine;

// �v���C���[�̒e���ˁi�V���b�g�E�`���[�W�V���b�g�j�̐���X�N���v�g
public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;    // ���˂���e�̃v���n�u�iInspector�Ŏw��j
    public Transform firePoint;        // �e�̔��ˈʒu�i��̎q�I�u�W�F�N�g�Ȃǂ��Z�b�g�j
    public float bulletSpeed = 5f;     // �e�̑��x

    [HideInInspector] public float chargeTime = 0f;    // �`���[�W���̎��ԁi���X�N���v�g���Q�Ɨp�j
    [HideInInspector] public bool isCharging = false;  // �`���[�W�����ǂ����i���X�N���v�g���Q�Ɨp�j
    public float requiredCharge = 2.0f;                // �`���[�W�e�ɕK�v�Ȓ��������ԁi�b�j

    void Update()
    {
        // ���G���̐F�ω���PlayerController���ŊǗ��B���̃X�N���v�g�ł͂��Ȃ��B

        // X�{�^�������n�߂���`���[�W�J�n
        if (Input.GetKeyDown(KeyCode.X))
        {
            isCharging = true;
            chargeTime = 0f; // �`���[�W���ԃ��Z�b�g
        }

        // �`���[�W���iX�L�[���������j
        if (isCharging && Input.GetKey(KeyCode.X))
        {
            chargeTime += Time.deltaTime; // �o�ߎ��Ԃ����Z
            // �F���o��PlayerController.LateUpdate�ŏ���
        }

        // X�L�[�𗣂����u�Ԃɒe����
        if (isCharging && Input.GetKeyUp(KeyCode.X))
        {
            // �`���[�W���Ԃ�requiredCharge�i��:2�b�j�ȏ�Ȃ狭���e
            Shoot(chargeTime >= requiredCharge);
            isCharging = false;
            chargeTime = 0f;
        }
    }

    // �e�𔭎˂��鏈��
    void Shoot(bool powered)
    {
        // �e�v���n�u�𔭎ˈʒu�ɐ���
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Rigidbody2D�ŕ����I�ɔ�΂�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // �e�{�̂̃X�N���v�g�iPlayerBullet�j���擾
        PlayerBullet pb = bullet.GetComponent<PlayerBullet>();

        // �v���C���[�̌����ɍ��킹�Ĕ��˕����i�E����=1, ������=-1�j
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        if (rb != null)
            rb.velocity = new Vector2(direction * bulletSpeed, 0f);

        if (pb != null)
        {
            if (powered)
            {
                pb.damage = 3; // �����e�i�`���[�W�V���b�g�j
                bullet.transform.localScale *= 4f; // �e��4�{�傫���i�����ڂ̂݁j
            }
            else
            {
                pb.damage = 1; // �ʏ�e
            }
        }

        // �e��3�b��Ɏ������Łi��ʊO�����p�j
        Destroy(bullet, 3.0f);
    }
}
