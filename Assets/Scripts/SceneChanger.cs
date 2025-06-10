using UnityEngine;
using UnityEngine.SceneManagement;

// ���̃X�N���v�g�́u���[�v�h�A�v��u�S�[���v�Ȃǂ̃g���K�[�ɃA�^�b�`���܂�
// �v���C���[���G���Ǝ��̃V�[���ֈړ����܂�
public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = "Stage2"; // �J�ڐ�V�[���̖��O�iInspector�Ŏw��j

    // ����Collider2D�����̃g���K�[�ɓ��������ɌĂ΂��
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�iPlayer�^�O�j���G�ꂽ�ꍇ�̂�
        if (other.CompareTag("Player"))
        {
            // �w�肵���V�[���ɐ؂�ւ���
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
