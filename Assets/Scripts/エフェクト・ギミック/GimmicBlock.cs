using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �v���C���[���߂Â��Ɨ������A������ɏ�����M�~�b�N�u���b�N�̃X�N���v�g
public class GimmicBlock : MonoBehaviour
{
    public float length = 0.0f;       // �v���C���[�����m���鋗���i���͈̔͂ɓ���ƃu���b�N�������j
    public bool isDelete = false;     // ������ɏ��ł��邩�ǂ����iON�Ȃ������j
    public GameObject deadObj;        // ���S����p�I�u�W�F�N�g�i�u���b�N�������ɗL�����j

    bool isFell = false;              // �u���b�N�����������ǂ����̃t���O
    float fadeTime = 0.5f;            // �t�F�[�h�A�E�g���o�̎c�莞�ԁi�b�j

    // ===== �Q�[���J�n���Ɉ�x�����Ă΂�� =====
    void Start()
    {


        // Rigidbody2D�̕����������ꎞ��~�i�Î~��Ԃɂ���j
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;

        deadObj.SetActive(false); // ���S����p�I�u�W�F�N�g���\��
    }

    // ===== ���t���[���Ă΂�� =====
    void Update()
    {


        // �^�O"Player"�������I�u�W�F�N�g�i�v���C���[�j��T��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // �v���C���[�Ƃ̋����𑪒�
            float d = Vector2.Distance(transform.position, player.transform.position);

            // ������length�ȓ��Ȃ痎�����J�n
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)
                {
                    // Rigidbody2D�̕����������J�n�i���I�ɂ���j
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                    deadObj.SetActive(true); // ���S����p�I�u�W�F�N�g��\��
                }
            }
        }

        // ������̃t�F�[�h�A�E�g����
        if (isFell)
        {
            fadeTime -= Time.deltaTime; // �t�F�[�h���Ԃ����炵�Ă���
            Color col = GetComponent<SpriteRenderer>().color;
            // �A���t�@�l�i�����x�j��0��1�ɐ��K�����ď��X�ɏ����Ă���
            col.a = Mathf.Clamp01(fadeTime / 0.5f); // 0.5��0.0��
            GetComponent<SpriteRenderer>().color = col;

            // ���S�ɏ�������u���b�N���폜
            if (fadeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    // ===== ����Collider2D�ƂԂ������Ƃ��ɌĂ΂�� =====
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // isDelete��ON�Ȃ痎���t���OON�i�t�F�[�h�J�n�j
        if (isDelete)
        {
            isFell = true;
        }
    }

    // ===== �G�f�B�^��Ŕ͈͂���������i�I�𒆂̂ݕ\���j =====
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length); // �Ԃ����Ŕ͈͕\��
    }
}
