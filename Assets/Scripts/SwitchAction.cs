using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �v���C���[���G����ON/OFF���؂�ւ��X�C�b�`�̃X�N���v�g
// ON/OFF�Ō����ڂ�؂�ւ�����A������(MovingBloc)�𓮂�������~�߂��肵�܂�
public class SwitchAction : MonoBehaviour
{
    public GameObject targetMoveBlock;    // �������������iMovingBloc���t���Ă�I�u�W�F�N�g���w��j
    public Sprite imageOn;                // ON���ɕ\������摜
    public Sprite imageOff;               // OFF���ɕ\������摜
    public bool on = false;               // �X�C�b�`�̏�ԁiON=true, OFF=false�j

    // ========== �ŏ��Ɉ�x�����Ă΂�� ==========
    void Start()
    {
        // �X�C�b�`�̏�Ԃŉ摜��؂�ւ�
        if (on)
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;
        }
    }

    // ========== ���t���[���i����͉������Ă��Ȃ��j ==========
    void Update()
    {
        // �󗓂ł�OK�i�g���������ꍇ�Ɏg���j
    }

    // ========== �X�C�b�`�Ƀv���C���[���G�ꂽ���ɌĂ΂�� ==========
    void OnTriggerEnter2D(Collider2D col)
    {
        // �G�ꂽ���肪�uPlayer�v�^�O�Ȃ�
        if (col.gameObject.tag == "Player")
        {
            if (on)
            {
                // ����ON�Ȃ�OFF�ɐ؂�ւ���
                on = false;
                GetComponent<SpriteRenderer>().sprite = imageOff;

                // ��(MovingBloc)���~�߂�
                MovingBloc movingBloc = targetMoveBlock.GetComponent<MovingBloc>();
                movingBloc.Stop();
            }
            else
            {
                // OFF�Ȃ�ON�ɐ؂�ւ���
                on = true;
                GetComponent<SpriteRenderer>().sprite = imageOn;

                // ��(MovingBloc)�𓮂���
                MovingBloc movingBloc = targetMoveBlock.GetComponent<MovingBloc>();
                movingBloc.Move();
            }
        }
    }
}
