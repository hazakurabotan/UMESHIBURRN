using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public TypeWriterText twText;
    public ShopSelector selector;
    public GameObject confirmPanel;
    int currentItemIndex;

    // ��b�E�I���̗��ꐧ��i�X�e�[�g�}�V���I�ɂ��Ɗy�I�j
    IEnumerator ShopFlow()
    {
        twText.ShowText("�悤�����A��������Ⴂ");
        yield return WaitZ();

        twText.ShowText("����͂����������Ă����");
        yield return WaitZ();

        selector.StartSelect();
        yield return new WaitUntil(() => selector.selecting == false); // �I�����I���܂őҋ@

        // ���莞�Ɂ����Ă΂��
        // ShowConfirm(selectedIndex)
    }

    public void ShowConfirm(int idx)
    {
        currentItemIndex = idx;
        confirmPanel.SetActive(true); // �u������ł����ł����H�vUI��ON
        // �n�C/�C�C�G�̃{�^���Ƀ��\�b�h���蓖��
    }

    public void OnConfirmYes()
    {
        // ���i����
        selector.items[currentItemIndex].gameObject.SetActive(false);
        confirmPanel.SetActive(false);

        // �Ō�̉�b
        StartCoroutine(EndTalk());
    }

    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
        selector.selecting = true;
    }

    IEnumerator EndTalk()
    {
        twText.ShowText("����ł����H���Ⴀ����΂���");
        yield return WaitZ();
        // �V�[���߂�
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    // Z�҂��R���[�`��
    IEnumerator WaitZ()
    {
        while (!Input.GetKeyDown(KeyCode.Z)) yield return null;
    }
}
