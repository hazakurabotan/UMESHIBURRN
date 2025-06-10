using UnityEngine;
using UnityEngine.SceneManagement;

// �h�A�⃏�[�v�|�C���g�p�g���K�[�X�N���v�g
// �v���C���[���͈͓��Ł��L�[�������Ǝw�肵���V�[���Ɉړ����܂�
public class DoorTrigger : MonoBehaviour
{
    public string sceneToLoad = "Shop"; // �s�������V�[������Inspector�Ŏw��

    private bool isPlayerInTrigger = false; // �v���C���[���͈͓��ɂ��邩�̃t���O

    // ���t���[���Ă΂��
    void Update()
    {
        // �v���C���[���͈͓� ���� ���L�[�������ꂽ�Ƃ���
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �V�[���J�ڂ����s
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // 2D�R���C�_�[�����̃g���K�[�ɓ������u�ԂɌĂ΂��
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �����Ă����I�u�W�F�N�g���uPlayer�v�^�O�̏ꍇ�̂�
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true; // �t���OON
        }
    }

    // 2D�R���C�_�[�����̃g���K�[����o���u�ԂɌĂ΂��
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false; // �t���OOFF
        }
    }
}
