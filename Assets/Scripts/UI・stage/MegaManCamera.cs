using UnityEngine;

// ���b�N�}�����J�����F�v���C���[����ʒ[���z������1��ʕ��J�������ړ�����X�N���v�g
public class MegaManCamera : MonoBehaviour
{
    public Transform player;           // �Ǐ]����v���C���[�iInspector�Ŏw�� or �����擾�j
    public float screenWidth = 16f;    // 1��ʕ��̉����iUnity���j�b�g�Ŏw��A��:16�j
    public float screenHeight = 9f;    // 1��ʕ��̍���
    public float yOffset = 0f;         // ��ʑS�̂��㉺�ɃI�t�Z�b�g�������ꍇ�ɒ���

    private Vector2 currentScreenOrigin; // ���݂̉�ʂ̍������W�i���̌��_�j

    void Start()
    {
        // �v���C���[�̎Q�Ƃ�������΃^�O�Ŏ����擾
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // �Q�[���J�n���Ɍ��݂̉�ʋ����v�Z
        UpdateScreenOrigin();
    }

    void Update()
    {
        Vector2 playerPos = player.position;

        // -------- �������̋��ړ��`�F�b�N --------
        // �v���C���[�����[���o��
        if (playerPos.x < currentScreenOrigin.x)
        {
            currentScreenOrigin.x -= screenWidth; // 1��ʍ���
            MoveCamera();
        }
        // �v���C���[���E�[���o��
        else if (playerPos.x > currentScreenOrigin.x + screenWidth)
        {
            currentScreenOrigin.x += screenWidth; // 1��ʉE��
            MoveCamera();
        }
        //
        // if (playerPos.y > currentScreenOrigin.y + screenHeight) {
        //     currentScreenOrigin.y += screenHeight;
        //     MoveCamera();
        // }


        // -------- �c�����̋��ړ��i�K�v�Ȃ�ǉ��j--------
        // �v���C���[�����[���o��
        // if (playerPos.y < currentScreenOrigin.y) {
        //     currentScreenOrigin.y -= screenHeight;
        //     MoveCamera();
        // }
        // �v���C���[����[���o��
        // if (playerPos.y > currentScreenOrigin.y + screenHeight) {
        //     currentScreenOrigin.y += screenHeight;
        //     MoveCamera();
        // }
    }

    // �J�����ʒu�����݂̉�ʋ��̒����Ɉړ�
    void MoveCamera()
    {
        float camX = currentScreenOrigin.x + screenWidth / 2f;
        float camY = currentScreenOrigin.y + screenHeight / 2f + yOffset; // y�����I�t�Z�b�g�����Z
        transform.position = new Vector3(camX, camY, transform.position.z);
    }

    // �v���C���[���������ʋ����v�Z���A��挴�_���X�V
    void UpdateScreenOrigin()
    {
        // ��Fplayer.x=17, screenWidth=16�Ȃ灨��挴�_=16
        float originX = Mathf.Floor(player.position.x / screenWidth) * screenWidth;
        float originY = Mathf.Floor(player.position.y / screenHeight) * screenHeight;
        currentScreenOrigin = new Vector2(originX, originY);

        MoveCamera(); // �����ʒu�ɃJ�����ړ�
    }
}
