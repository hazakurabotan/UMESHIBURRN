using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������i���t�g��G���x�[�^�[�Ȃǁj�𐧌䂷��X�N���v�g
public class MovingBloc : MonoBehaviour
{
    public float moveX = 0.0f;   // X�����̈ړ�����
    public float moveY = 0.0f;   // Y�����̈ړ�����
    public float times = 0.0f;   // �Г��ɂ�����ړ����ԁi�b�j
    public float wait = 0.0f;    // ��~���ԁi�����̒[�ł̑҂����ԁj
    public bool isMoveWhenOn = false; // ������Ƃ����������Ȃ�ON
    public bool isCanMove = true;     // �������ݓ����邩�ǂ���

    Vector3 startPos;  // ���̏����ʒu
    Vector3 endPos;    // �ړ���̈ʒu
    bool isReverse = false; // �ړ��������t�]����t���O

    float movep = 0.0f; // Lerp��Ԓl�i0��1�j

    // ========== ������ ==========
    void Start()
    {
        startPos = transform.position; // �X�^�[�g�ʒu�L�^
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY); // �I�_���v�Z

        // ����������������ݒ�Ȃ�ŏ��͓������Ȃ�
        if (isMoveWhenOn)
        {
            isCanMove = false;
        }
    }

    // ========== ���t���[���Ă΂�� ==========
    void Update()
    {
        // �������ԂȂ�c
        if (isCanMove)
        {
            float distance = Vector2.Distance(startPos, endPos); // ���ړ�����
            float ds = distance / times;        // 1�b������̈ړ�����
            float df = ds * Time.deltaTime;     // 1�t���[���Ői�ދ���
            movep += df / distance;             // ��Ԓl��i�߂�i0��1�j

            if (isReverse)
            {
                // �I�_���n�_�ɖ߂�
                transform.position = Vector2.Lerp(endPos, startPos, movep);
            }
            else
            {
                // �n�_���I�_�֐i��
                transform.position = Vector2.Lerp(startPos, endPos, movep);
            }

            // �[�܂œ��B������
            if (movep >= 1.0f)
            {
                movep = 0.0f;             // ��Ԓl���Z�b�g
                isReverse = !isReverse;   // �ړ��������t�]
                isCanMove = false;        // �ꎞ��~

                if (!isMoveWhenOn)
                {
                    // �u����������������v�łȂ���΁Await�b��ɍĎn��
                    Invoke("Move", wait);
                }
            }
        }
    }

    // ========== �O������ړ��J�n����֐� ==========
    public void Move()
    {
        isCanMove = true;
    }

    // ========== �O������ړ���~����֐� ==========
    public void Stop()
    {
        isCanMove = false;
    }

    // ========== �v���C���[����������̏��� ==========
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �v���C���[�����̏��̎q�I�u�W�F�N�g�Ɂi�e�q�֌W�ɂ��Ĉꏏ�ɓ������j
            collision.transform.SetParent(transform);

            if (isMoveWhenOn)
            {
                // ����������������ꍇ�A�����ňړ��J�n
                isCanMove = true;
            }
        }
    }

    // ========== �v���C���[���~�肽���̏��� ==========
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // �v���C���[�����̐e�ɖ߂��i�e�q�֌W�����j
            collision.transform.SetParent(null);
        }
    }

    // ========== �G�f�B�^��ňړ��͈͂����� ==========
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }

        // �ړ����[�g�̐��i���S����ړ��ʂԂ�̃x�N�g���j
        Gizmos.DrawWireCube(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));

        // ���{�̂̃T�C�Y
        Vector2 size = GetComponent<SpriteRenderer>().size;
        // �����ʒu�̃��C���[�t���[��
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        // �ړ���̃��C���[�t���[��
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
