using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�L�����N�^�[�̈ړ��E���]�E����p�X�N���v�g
public class EnemyController : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float bulletSpeed = 6f;
    public float shootInterval = 2.0f;
    private float shootTimer = 0f;
    private Transform player;

    public Transform firePoint;  

    public float speed = 3.0f;           // �G�̈ړ����x
    public bool isToRight = false;       // true=�E�����Afalse=�������i�����l�j
    public float revTime = 0f;           // ��莞�Ԃ��ƂɎ����Ŕ��]���������̊Ԋu�i0�Ȃ玩�����]�Ȃ��j
    public LayerMask groundLayer;        // �n�ʔ���p���C���[�iInspector�Őݒ�j

    float time = 0f;                     // �o�ߎ��ԃJ�E���g�i�������]�p�j

    private Enemy enemy;                 // Enemy�iHP�Ǘ��Ⓤ������ԁj�X�N���v�g�̎Q��

    void Start()
    {
        // ���̃I�u�W�F�N�g�ɕt���Ă���Enemy�X�N���v�g���擾
        enemy = GetComponent<Enemy>();

        // �����̌����𔽉f�i�E�����Ȃ�x�X�P�[����-1�ɂ��Ĕ��]�j
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        // �v���C���[��Transform���擾
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

    }

    void Update()
    {
        // revTime��0���傫���������������]�������s��
        if (revTime > 0)
        {
            time += Time.deltaTime; // �o�ߎ��Ԃ����Z
            if (time >= revTime)
            {
                isToRight = !isToRight; // �����𔽓]
                time = 0f;              // �^�C�}�[���Z�b�g
                // �����ڂ����E���]�ilocalScale.x��؂�ւ��j
                transform.localScale = isToRight ? new Vector2(-1, 1) : new Vector2(1, 1);
            }
        }

        if (player != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                ShootToPlayer();
                shootTimer = 0f;
            }
        }


    }


    void ShootToPlayer()
    {
        Debug.Log("�G���e�𔭎˂��悤�Ƃ��Ă��܂��I");
        Vector2 dir = (player.position - transform.position).normalized;
        Vector3 shootPos = firePoint != null ? firePoint.position : transform.position; // �������I
        GameObject bullet = Instantiate(bulletPrefab, shootPos, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dir * bulletSpeed;
        }
    }

    void FixedUpdate()
    {
        // �������Ă�Œ��͎��͂œ����Ȃ�
        if (enemy != null && enemy.isFlying) return;

        // ===== �n�ʔ��� =====
        bool onGround = Physics2D.CircleCast(
            transform.position,   // ���S
            0.5f,                 // ���a
            Vector2.down,         // �������ɃT�[�`
            0.5f,                 // ����
            groundLayer           // �`�F�b�N���郌�C���[
        );

        if (onGround)
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (rbody != null)
            {
                // �����ɉ����č��E�ɑ��x���Z�b�g
                float moveX = isToRight ? speed : -speed;
                rbody.velocity = new Vector2(moveX, rbody.velocity.y);
            }
        }
    }

    // === �ǂ�g���K�[�ɂԂ��������ɔ��]�i�����ŌĂ΂��j ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight; // �������t��
        time = 0f;              // �^�C�}�[�����Z�b�g
        // �����ڂ����]
        transform.localScale = isToRight ? new Vector2(-1, 1) : new Vector2(1, 1);
    }
}
