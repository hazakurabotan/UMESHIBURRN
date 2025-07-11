using UnityEngine;

// ----------------------------------------------
// TimedPlatform
// ��莞�Ԃ��Ƃɏ������茻�ꂽ�肷�鏰�M�~�b�N
// ----------------------------------------------
public class TimedPlatform : MonoBehaviour
{
    public float visibleTime = 2f;   // �����\�������b��
    public float invisibleTime = 1f; // ���������Ă���b��

    private float timer = 0f;        // �^�C�}�[
    private bool isVisible = true;   // ���݂̕\�����
    private SpriteRenderer sr;       // ���̌����ڂ��Ǘ�
    private Collider2D col;          // �����蔻��

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();  // �����ڂ̐���p
        col = GetComponent<Collider2D>();    // ����/���Ȃ��̐���p
        SetVisible(true); // �ŏ��͕\���E�L����Ԃ���X�^�[�g
    }

    void Update()
    {
        timer += Time.deltaTime; // ���t���[���o�ߕb�������Z

        // --- �\������visibleTime�𒴂�������� ---
        if (isVisible && timer >= visibleTime)
        {
            SetVisible(false); // ������
            timer = 0f;        // �^�C�}�[���Z�b�g
        }
        // --- �����Ă���invisibleTime�𒴂�����Ăѕ\�� ---
        else if (!isVisible && timer >= invisibleTime)
        {
            SetVisible(true); // �Ăь����
            timer = 0f;
        }
    }

    // --- �\���E��\����؂�ւ���֐� ---
    void SetVisible(bool show)
    {
        isVisible = show;      // ��ԃt���O�X�V
        sr.enabled = show;     // �X�v���C�g�\��ON/OFF
        if (col != null) col.enabled = show; // �R���C�_�[��ON/OFF
    }
}
