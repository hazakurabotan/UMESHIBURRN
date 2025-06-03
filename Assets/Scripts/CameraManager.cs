using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // === �J�������ړ��ł���͈́iX��Y�j ===
    public float leftLimit = -10f;     // �J�������s����ł���
    public float rightLimit = 20f;     // �J�������s����ł��E
    public float topLimit = 5f;        // ��ԏ�
    public float bottomLimit = -5f;    // ��ԉ��i�� f ��Y�ꂪ���Ȃ̂Œ��ӁI�j

    // �T�u�X�N���[���i��F�~�j�}�b�v�E�J�������o�p�j
    public GameObject subScreen;

    void Update()
    {
        // �^�O�� "Player" �̃I�u�W�F�N�g��T��
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // �v���C���[�����݂���ꍇ�̂ݒǏ]
        if (player != null)
        {
            // �v���C���[�̌��݈ʒu���擾
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z; // �J������Z���͂��̂܂܌Œ�

            // === �J�����̈ړ��͈͂𐧌� ===
            x = Mathf.Clamp(x, leftLimit, rightLimit);
            y = Mathf.Clamp(y, bottomLimit, topLimit);

            // �J�����̈ʒu���v���C���[�ɒǏ]������
            transform.position = new Vector3(x, y, z);

            // === �T�u�X�N���[�����J�����ɘA�����ĒǏ]������ ===
            if (subScreen != null)
            {
                // ���C���J�����̌��݈ʒu���擾
                Vector3 camPos = Camera.main.transform.position;

                // �T�u�X�N���[���̕\���I�t�Z�b�g�ʒu�i��F�E���ɕ\���j
                float offsetX = 3.5f;
                float offsetY = -2.5f;

                // �T�u�X�N���[���̈ʒu���X�V�iZ�͌Œ�j
                subScreen.transform.position = new Vector3(
                    camPos.x + offsetX,
                    camPos.y + offsetY,
                    subScreen.transform.position.z
                );
            }
        }
        else
        {
            Debug.LogWarning("Player ��������܂���ł����B�^�O 'Player' ���m�F���Ă��������B");
        }
    }
}
