using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform player;       // �v���C���[
    public float smoothSpeed = 0.2f; // �Ǐ]�̂Ȃ߂炩��
    public Vector2 minPosition;    // �J�����̍�������
    public Vector2 maxPosition;    // �J�����̉E�㐧��

    void LateUpdate()
    {
        if (player == null) return;

        // �Ǐ]�ʒu���v�Z
        Vector3 targetPos = player.position;
        targetPos.z = transform.position.z;

        // �͈͓��Ɏ��߂�
        targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);

        // �Ȃ߂炩�Ǐ]
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
    }
}
