using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // ======= Shop�V�[���܂��̓p�l�����͎��Ԃ��~�߂� =======
        if (GameManager.Instance != null)
        {
            string sceneName = SceneManager.GetActiveScene().name.ToLower();
            if (sceneName.Contains("shop") || GameManager.Instance.IsItemPanelOpen())
                return;
        }

        // ======= ���Ԃ��~�܂��Ă����ԂȂ珈�����Ȃ� =======
        if (isTimeOver) return;

        // ======= ���ԃJ�E���g�����J�n =======
        times += Time.deltaTime;

        if (isCountDown)
        {
            displayTime = gameTime - times;
            if (displayTime <= 0.0f)
            {
                displayTime = 0.0f;
                isTimeOver = true;
            }
        }
        else
        {
            displayTime = times;
            if (displayTime >= gameTime)
            {
                displayTime = gameTime;
                isTimeOver = true;
            }
        }
    }
}
