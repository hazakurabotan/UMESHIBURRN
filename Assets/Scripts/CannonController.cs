using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �C��i�L���m���j���v���C���[�Ɍ������Ēe�����X�N���v�g
public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;     // ���˂���e�̃v���n�u�iInspector�Ŏw��j
    public float delayTime = 3.0f;   // �e�����܂ł̑ҋ@���ԁi�b�P�ʁj
    public float fireSpeed = 4.0f;   // �e�̔��ˑ��x�i�傫���قǑ����j
    public float length = 0.0f;      // �v���C���[�����m���鋗���i�͈́j

    GameObject player;               // �v���C���[�{�́i���I�Ɏ擾�j
    Transform gateTransform;         // �C��̔��ˌ��̈ʒu�i"gate"�Ƃ����q�I�u�W�F�N�g���쐬�j
    float passedTimes = 0;           // �o�ߎ��Ԃ��J�E���g����ϐ�

    // --- �v���C���[���C��̊��m�͈͓��ɂ��邩���肷��֐� ---
    bool CheckLength(Vector2 targetPos)
    {
        // �C��̈ʒu�ƃv���C���[�ʒu�̋������v�Z
        float d = Vector2.Distance(transform.position, targetPos);

        // length�ȏ㗣��Ă����false�A�͈͓��Ȃ�true
        return (length >= d);
    }

    void Start()
    {
        // ���ˌ��I�u�W�F�N�g�i"gate"�Ƃ������O�̎q�I�u�W�F�N�g�j���擾
        gateTransform = transform.Find("gate");

        // �v���C���[�{�̂��^�O�ŒT���Ď擾
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // ���t���[���o�ߎ��Ԃ����Z
        passedTimes += Time.deltaTime;

        // �v���C���[�����m�͈͓��ɂ���ꍇ�������˔���
        if (CheckLength(player.transform.position))
        {
            // delayTime�i�ҋ@���ԁj���z������e������
            if (passedTimes > delayTime)
            {
                passedTimes = 0; // �o�ߎ��Ԃ����Z�b�g

                // �e�𔭎˂���ʒu�igate�̈ʒu�j���擾
                Vector2 pos = gateTransform.position;

                // �e�̃v���n�u�𐶐��i���̏�ɐV�����e�I�u�W�F�N�g�����j
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);

                // ���˕������v�Z�iZ���̉�]�p�x����X�EY�����x�N�g�������j
                float angleZ = transform.localEulerAngles.z;
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
                Vector2 dir = new Vector2(x, y).normalized; // �P�ʃx�N�g���ɂ��ĕ��������擾

                // �e�ɕ����I�ȗ́iAddForce�j�������Ĕ�΂�
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                if (rbody != null)
                {
                    // dir�i�����j�~ fireSpeed�i���x�j�Ŕ���
                    rbody.AddForce(dir * fireSpeed, ForceMode2D.Impulse);
                }
            }
        }
    }

    // --- Unity�G�f�B�^��Ŋ��m�͈͂��������邽�߂̊֐��i�I�𒆂����j---
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // �Ԃ�����
        Gizmos.DrawWireSphere(transform.position, length); // �C�䒆�S�ɔ��alength�̉~��`��
    }
}
