using UnityEngine;
using UnityEngine.SceneManagement;

// �V�[����؂�ւ��邽�߂̃V���v���ȃX�N���v�g
// �{�^����C�x���g����Ăяo���Ďg���܂�
public class ChangeScene : MonoBehaviour
{
    // �ړ��������V�[���̖��O��Inspector�Őݒ�
    // ��: "Stage1"��"GameOver"�Ȃ�
    public string sceneName = "Stage1"; // �� �킩��₷���V�[�����Ɩ����I

    // ���̊֐����ĂԂƎw�肵���V�[���ɐ؂�ւ��
    public void Load()
    {
        // SceneManager.LoadScene�ŃV�[�������[�h
        SceneManager.LoadScene(sceneName);
    }
}
