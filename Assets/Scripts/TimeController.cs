using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Q�[�����̐������Ԃ�o�ߎ��Ԃ��Ǘ�����^�C�}�[�X�N���v�g
public class TimeController : MonoBehaviour
{
    // === �ݒ�p�p�����[�^ ===
    public bool isCountDown = true; // true�Ȃ�J�E���g�_�E���Afalse�Ȃ�J�E���g�A�b�v
    public float gameTime = 0;      // �Q�[���̍ő厞�ԁi�b���Őݒ�j
    public bool isTimeOver = false; // true�Ȃ�^�C�}�[��~�i���Ԑ؂��ڕW�B���j
    public float displayTime = 0;   // UI�\���p�̎c�莞�Ԃ܂��͌o�ߎ���

    float times = 0; // �����Ŏg���o�ߎ��ԃJ�E���g�p�i���t���[�����Z�j

    // ====== �Q�[���J�n����1�񂾂��Ă΂�� ======
    void Start()
    {
        if (isCountDown)
        {
            // �J�E���g�_�E���̏ꍇ�͍ŏ��ɍő厞�Ԃ���X�^�[�g
            displayTime = gameTime;
        }
        // �J�E���g�A�b�v����0���玩���X�^�[�g�Ȃ̂ŉ������Ȃ���OK
    }

    // ====== ���t���[���Ă΂�� ======
    void Update()
    {
        // �^�C�}�[���~�܂��Ă��Ȃ���΁i�܂��I�����Ă��Ȃ���΁j
        if (!isTimeOver)
        {
            times += Time.deltaTime; // 1�t���[���Ԃ�̎��Ԃ����Z�i�b�P�ʁj

            if (isCountDown)
            {
                // === �J�E���g�_�E������ ===
                displayTime = gameTime - times; // �c�莞�Ԃ��v�Z
                // Debug.Log("�J�E���g�_�E����: " + displayTime);

                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f; // 0�����ɂȂ�Ȃ��悤�ɕ␳
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

                // Debug.Log("�o�ߎ���: " + displayTime);
            }
        }
    }
}
