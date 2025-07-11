using UnityEngine;

// ------------------------------------------------------------
// ExtendPlatform
// �v���C���[���߂Â��Ǝ����ŉ��ɐL�т鑫�����������X�N���v�g
// ------------------------------------------------------------
public class ExtendPlatform : MonoBehaviour
{
    // �߂��ɗ����甽������Ώہi�v���C���[�BInspector�ŃZ�b�g�j
    public Transform player;

    // �ǂ̂��炢�߂Â�����L�т锻��ɂȂ邩�i�����j
    public float triggerDistance = 2.5f;

    // �ǂ̂��炢���ɐL�т邩�i�����j
    public float extendDistance = 2.0f;

    // ���ꂪ�ǂꂭ�炢�̑����ŐL�т邩
    public float extendSpeed = 3.0f;

    // ����̏����ʒu�i�L�юn�߂�O�̈ʒu�j
    private Vector3 originalPos;

    // ���������L�΂������ǂ���
    private bool isExtended = false;

    // �Q�[���J�n���Ɉ�x�������s
    void Start()
    {
        // ���̈ʒu��ۑ��i�������ɐL�΂��j
        originalPos = transform.position;
    }

    // ���t���[���Ă΂��
    void Update()
    {
        // �v���C���[���w�肳��Ă��Ȃ���Ή������Ȃ�
        if (player == null) return;

        // �v���C���[�Ƃ̋������v�Z
        float dist = Vector2.Distance(player.position, transform.position);

        // �܂��L�тĂȂ��āA�v���C���[���߂Â�����
        if (!isExtended && dist < triggerDistance)
        {
            isExtended = true; // �L�ъJ�n
        }

        // �����L�΂��A�j���[�V����
        if (isExtended)
        {
            // �ڕW�ʒu�ioriginalPos����E��extendDistance�Ԃ�ړ��j
            Vector3 target = originalPos + new Vector3(extendDistance, 0, 0);
            // ���݈ʒu����ڕW�ʒu�ցA���t���[���������߂Â���i�X���[�Y�ɐL�΂��j
            transform.position = Vector3.MoveTowards(transform.position, target, extendSpeed * Time.deltaTime);
        }
    }
}
