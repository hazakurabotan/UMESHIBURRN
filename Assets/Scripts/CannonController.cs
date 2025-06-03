using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;     // ���˂���e�i�v���n�u�j
    public float delayTime = 3.0f;   // �e�����܂ł̑ҋ@���ԁi�b�j
    public float fireSpeed = 4.0f;   // �e�̔��ˑ��x
    public float length = 0.0f;      // �v���C���[�����m����͈�

    GameObject player;              // �v���C���[�{��
    Transform gateTransform;        // ���ˌ��i�C�g�̐�Ȃǂɋ�I�u�W�F�N�g��ݒu�j
    float passedTimes = 0;          // �o�ߎ��ԁi�ҋ@�J�E���g�p�j

    // �v���C���[���C��̊��m�͈͓��ɂ��邩�`�F�b�N
    bool CheckLength(Vector2 targetPos)
    {
        float d = Vector2.Distance(transform.position, targetPos); // ��������
        return (length >= d); // �͈͓����ǂ�����Ԃ�
    }

    void Start()
    {
        // ���ˌ����擾�i�q�I�u�W�F�N�g�� "gate" ���K�v�j
        gateTransform = transform.Find("gate");

        // �v���C���[���擾�i�^�O�Ō����j
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // ���ԃJ�E���g
        passedTimes += Time.deltaTime;

        // �v���C���[���͈͓��ɂ���ꍇ�������˔���
        if (CheckLength(player.transform.position))
        {
            // �ҋ@���Ԃ��z������e�𔭎�
            if (passedTimes > delayTime)
            {
                passedTimes = 0; // �J�E���g���Z�b�g

                // ���ˈʒu�𔭎ˌ��ɐݒ�
                Vector2 pos = gateTransform.position;

                // �e�̐���
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);

                // ���˕����̌v�Z�i�I�u�W�F�N�g�̊p�x����x�N�g�����Z�o�j
                float angleZ = transform.localEulerAngles.z;
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
                Vector2 dir = new Vector2(x, y).normalized;

                // �e�ɗ͂������Ĕ���
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                if (rbody != null)
                {
                    rbody.AddForce(dir * fireSpeed, ForceMode2D.Impulse);
                }
            }
        }
    }

    // �G�f�B�^��Ŕ͈͂������i�I�𒆂̂݁j
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
