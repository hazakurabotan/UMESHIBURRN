using UnityEngine;

public class MegaManCamera : MonoBehaviour
{
    public Transform player;
    public float screenWidth = 16f; // 1��ʂԂ�̉����i��F16���j�b�g�j
    public float screenHeight = 9f; // 1��ʂԂ�̍���
    public float yOffset = 0f; // �ǉ��I
    private Vector2 currentScreenOrigin;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        UpdateScreenOrigin();
    }

    void Update()
    {
        Vector2 playerPos = player.position;

        // �v���C���[�����݂̉�ʔ͈͊O�ɏo����J�����ړ�
        if (playerPos.x < currentScreenOrigin.x)
        {
            currentScreenOrigin.x -= screenWidth;
            MoveCamera();
        }
        else if (playerPos.x > currentScreenOrigin.x + screenWidth)
        {
            currentScreenOrigin.x += screenWidth;
            MoveCamera();
        }
        // ���c�X�N���[�����������ꍇ�͓����m����y������

        // �ǉ��F�㉺�����̃p�l���؂�ւ����������ꍇ
        // if (playerPos.y < currentScreenOrigin.y) { ... }
        // if (playerPos.y > currentScreenOrigin.y + screenHeight) { ... }
    }

    void MoveCamera()
    {
        // �J�����̈ʒu���p�l���̍�����̒�����
        float camX = currentScreenOrigin.x + screenWidth / 2f;
        float camY = currentScreenOrigin.y + screenHeight / 2f + yOffset; // �������C��
        transform.position = new Vector3(camX, camY, transform.position.z);
    }

    void UpdateScreenOrigin()
    {
        // �v���C���[������p�l���i��ʋ��j�̍������W
        float originX = Mathf.Floor(player.position.x / screenWidth) * screenWidth;
        float originY = Mathf.Floor(player.position.y / screenHeight) * screenHeight;
        currentScreenOrigin = new Vector2(originX, originY);

        MoveCamera();
    }
}
