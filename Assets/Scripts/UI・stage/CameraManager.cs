using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �J�����̒Ǐ]��ړ��������Ǘ�����N���X
public class CameraManager : MonoBehaviour
{
    // === �J�������ړ��ł���͈́iX��Y�̍ŏ��E�ő�l�j ===
    public float leftLimit = -10f;     // �J�������s����ł����̈ʒu
    public float rightLimit = 20f;     // �J�������s����ł��E�̈ʒu
    public float topLimit = 5f;        // �J�������s�����ԏ�̈ʒu
    public float bottomLimit = -5f;    // �J�������s�����ԉ��̈ʒu
    // �� float�l�̖����uf�v�͏ȗ�����ƃG���[�ɂȂ�̂ŖY�ꂸ�ɂ���I

    // �T�u�X�N���[���p�I�u�W�F�N�g�i��F�~�j�}�b�v�≉�o�p��ʁj
    public GameObject subScreen;

    void Update()
    {
        // ���t���[���A�^�O "Player" �̃I�u�W�F�N�g��T��
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // �v���C���[�����������������Ǐ]�������s��
        if (player != null)
        {
            // �v���C���[�̌��݈ʒu�ix, y�j���擾
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z; // �J������Z���͊�{�I�ɌŒ�i2D�Q�[���Ȃ���ɏd�v�j

            // === �J�����̈ړ��͈͂𐧌����� ===
            // Clamp�Ŏw��͈͓���x, y�����߂�i����ȏ�͓����Ȃ��j
            x = Mathf.Clamp(x, leftLimit, rightLimit);
            y = Mathf.Clamp(y, bottomLimit, topLimit);

            // �J�����{�̂̈ʒu���X�V�i�v���C���[�̕��ɒǏ]�j
            transform.position = new Vector3(x, y, z);

            // === �T�u�X�N���[���i�~�j�}�b�v���j���J�����ʒu�ɘA��������� ===
            if (subScreen != null)
            {
                // ���C���J�����̌��݈ʒu���擾
                Vector3 camPos = Camera.main.transform.position;

                // �T�u�X�N���[���̕\���I�t�Z�b�g�ʒu�i��F�E���ɏo���������̒l�j
                float offsetX = 3.5f;
                float offsetY = -2.5f;

                // �T�u�X�N���[���̈ʒu���X�V�iz���W�͕ύX���Ȃ��j
                subScreen.transform.position = new Vector3(
                    camPos.x + offsetX,
                    camPos.y + offsetY,
                    subScreen.transform.position.z
                );
            }
        }
        else
        {
            // �v���C���[��������Ȃ��ꍇ�̌x�����b�Z�[�W
            Debug.LogWarning("Player ��������܂���ł����B�^�O 'Player' ���m�F���Ă��������B");
        }
    }
}
