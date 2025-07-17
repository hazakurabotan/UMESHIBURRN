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
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            // ��������Hierarchy����DialogManager�S���o��
            Debug.Log("�yBossTrigger�z�g���K�[�������ɑ��݂���DialogManager��");
            foreach (var dm in FindObjectsOfType<DialogManager>())
            {
                Debug.Log("DialogManager: " + dm.gameObject.name);
            }

            var allDialogManagers = FindObjectsOfType<DialogManager>();
            Debug.Log($"�yDialogManager�`�F�b�N�zFindObjectsOfType<DialogManager>().Length = {allDialogManagers.Length}");
            foreach (var dm in allDialogManagers)
            {
                Debug.Log($"DialogManager��: {dm.name} / �e: {dm.transform.parent?.name}");
            }


            boss.isActive = false;
            FindObjectOfType<PlayerController>().enabled = false;

            // �����Ŗ���DialogManager���擾���Ȃ���
            DialogManager manager = FindObjectOfType<DialogManager>();
            if (manager != null)
            {
                manager.StartDialog(dialogLines);
            }
            else
            {
                Debug.LogWarning("DialogManager��������܂���ł����I");
            }

            Destroy(gameObject);
        }
    }
}
