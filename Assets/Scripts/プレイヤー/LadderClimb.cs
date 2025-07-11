using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �͂�����o���悤�ɂ��邽�߂̃X�N���v�g
public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 3f;       // �͂�����o�鑬��
    private bool onLadder = false;      // ���͂����̏�ɂ��邩�ǂ���
    private Rigidbody2D rb;             // �������Z�i�ړ��E�W�����v���j�p�̃R���|�[�l���g

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D���擾
    }

    void Update()
    {
        // �͂����ɐG��Ă���Ԃ����o��~��ł���
        if (onLadder)
        {
            // �㉺�L�[�̓��́i�ぁ1�A����-1�A���������Ȃ���0�j
            float vertical = Input.GetAxisRaw("Vertical");

            // �d�͂�0�ɂ��āA�͂����̏㉺�ړ��݂̂ɂ���
            rb.gravityScale = 0f;

            // �㉺�ړ��������x���Z�b�g�i���͂��̂܂܁j
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }
    }

    // �͂����iLadder�^�O�t���j�ɓ������u�ԂɌĂ΂��
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = true; // �͂�����t���OON
        }
    }

    // �͂�������o���u�ԂɌĂ΂��
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = false;    // �͂�����t���OOFF
            rb.gravityScale = 1f; // �d�͂����ɖ߂��i���ʂ̃W�����v�◎���ɖ߂�j
        }
    }
}
