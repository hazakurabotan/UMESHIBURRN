using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // === �ݒ�p�p�����[�^ ===
    public bool isCountDown = true; // true�Ȃ�J�E���g�_�E���Afalse�Ȃ�J�E���g�A�b�v
    public float gameTime = 0; // �Q�[���̍ő厞�ԁi�b���Őݒ�j
    public bool isTimeOver = false; // true�Ȃ�^�C�}�[��~�i���Ԑ؂�j
    public float displayTime = 0; // �\���p�̎c�莞�Ԃ܂��͌o�ߎ��ԁiUI�\���p�j

    float times = 0; // �����Ŏg���o�ߎ��ԃJ�E���g�p�iUpdate�ŉ��Z�j

    void Start()
    {
        if (isCountDown)
        {
            // �J�E���g�_�E���̏ꍇ�́A�ŏ��͍ő厞�Ԃ���X�^�[�g
            displayTime = gameTime;
        }
    }

    void Update()
    {
        // ���Ԍv�����~�܂��Ă��Ȃ���Ώ�������
        if (!isTimeOver)
        {
            times += Time.deltaTime; // 1�t���[�����̎��Ԃ����Z�i�b�P�ʁj

            if (isCountDown)
            {
                // === �J�E���g�_�E������ ===
                displayTime = gameTime - times; // �c�莞�Ԃ��v�Z
                // Debug.Log("�J�E���g�_�E����: " + displayTime); // �f�o�b�O�o��

                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f; // ���̎��ԂɂȂ�Ȃ��悤�ɕ␳
                    isTimeOver = true;  // �^�C�}�[�I��
                }
            }
            else
            {
                // === �J�E���g�A�b�v���� ===
                displayTime = times; // �o�ߎ��Ԃ�\���p�Ɋi�[

                if (displayTime >= gameTime)
                {
                    displayTime = gameTime; // �ő厞�Ԉȏ�ɂȂ�Ȃ��悤�ɕ␳
                    isTimeOver = true;      // �^�C�}�[�I��
                }

               //  Debug.Log("TIMES: " + displayTime); // �f�o�b�O�o��
            }
        }
    }
}
