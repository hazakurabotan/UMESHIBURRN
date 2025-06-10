using UnityEngine;

// �v���C���[�̏o���ʒu���Ǘ�����X�N���v�g
// �ʏ��defaultSpawn�A�V���b�v����߂����Ƃ���shopSpawnPoint�Ƀ��[�v
public class PlayerSpawnManager : MonoBehaviour
{
    public Transform defaultSpawn;      // �ʏ�̏o���ʒu�iInspector�Őݒ�j
    public Transform shopSpawnPoint;    // �V���b�v����߂����Ƃ��̏o���ʒu

    void Start()
    {
        // �uPlayer�v�^�O�̂����I�u�W�F�N�g��T��
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // �V���b�v����߂��Ă����t���O��ON�ŁAshopSpawnPoint���w�肳��Ă���΂�����Ƀ��[�v
            if (SceneTransitionInfo.cameFromShop && shopSpawnPoint != null)
            {
                player.transform.position = shopSpawnPoint.position;
                SceneTransitionInfo.cameFromShop = false; // ��x�g�����烊�Z�b�g�i�߂�����͒ʏ�ɖ߂��j
            }
            // ����ȊO�i�ŏ��̊J�n�Ȃǁj��defaultSpawn�ɔz�u
            else if (defaultSpawn != null)
            {
                player.transform.position = defaultSpawn.position;
            }
        }
    }
}
