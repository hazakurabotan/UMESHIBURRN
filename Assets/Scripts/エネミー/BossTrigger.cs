using UnityEngine;

// �{�X��C�x���g�p�g���K�[�X�N���v�g
// �v���C���[�����̃g���K�[�ɐG���ƁA�C�x���g��b���J�n���ă{�X�s�����ꎞ��~�����܂�
public class BossTrigger : MonoBehaviour
{
    public BossSimpleJump boss;               // �ΏۂƂȂ�{�X�̃X�N���v�g�iInspector�ŃA�T�C���j
    public DialogManager dialogManager;       // ��b�Ǘ��X�N���v�g�iInspector�ŃA�T�C���j
    public DialogLine[] dialogLines;          // �C�x���g�ōĐ�����Z���t�ꗗ�iInspector�œo�^�j

    bool hasTriggered = false;                // ���ɔ����ς݂��ǂ����̃t���O�i2�d�����h�~�j

    // ���̃R���C�_�[�����̃I�u�W�F�N�g�ɓ������Ƃ��ɌĂ΂��
    void OnTriggerEnter2D(Collider2D other)
    {
        // �܂��������Ă��Ȃ��āA���uPlayer�v�^�O�̃I�u�W�F�N�g���G�ꂽ�ꍇ�̂ݔ���
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;              // 2�d�����h�~�̂��߃t���O�𗧂Ă�

            boss.isActive = false;            // �{�X�̍s�����ꎞ��~�i��b�C�x���g���ɖ\��Ȃ��悤�Ɂj

            // �v���C���[����֎~�i��b���͓����Ȃ�����j
            FindObjectOfType<PlayerController>().enabled = false;

            // ��b�C�x���g�J�n�iDialogManager�ɉ�b���e��n���j
            dialogManager.StartDialog(dialogLines);

            Destroy(gameObject);              // ���̃g���K�[�͈�x�g����������i���x���������Ȃ��悤�Ɂj
        }
    }
}
