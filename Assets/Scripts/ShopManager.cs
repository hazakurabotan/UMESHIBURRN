using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

// �V���b�v�S�̗̂�����Ǘ�����X�N���v�g
public class ShopManager : MonoBehaviour
{
    public TypeWriterText twText;      // ���b�Z�[�W�\���p�i�^�C�v���C�^�[���o�j
    public ShopSelector selector;      // ���i�I����UI
    public GameObject confirmPanel;    // �n�C/�C�C�G�m�F�p�l��
    int currentItemIndex;              // ���ݑI�𒆂̏��i�ԍ�

    void Start()
    {
        // �V���b�v�̈�A�̗�����R���[�`���ŊJ�n
        StartCoroutine(ShopFlow());
    }

    // ====== �V���b�v�̉�b��I�����̐i�s�Ǘ� ======
    IEnumerator ShopFlow()
    {
        twText.ShowText("�悤�����A��������Ⴂ");
        yield return WaitZ(); // Z�L�[�҂�

        twText.ShowText("����͂����������Ă����");
        yield return WaitZ(); // Z�L�[�҂�

        selector.StartSelect(); // ���i�I���X�^�[�g
        yield return new WaitUntil(() => selector.selecting == false); // �I�����I���܂ő҂�

        // �����莞�� ShowConfirm(selectedIndex) ���Ăԑz��
    }

    // ====== �u����𔃂��܂����H�v�Ȃǂ̊m�F�E�B���h�E�\�� ======
    public void ShowConfirm(int idx)
    {
        currentItemIndex = idx;         // �I�����ꂽ���i�ԍ����L�^
        confirmPanel.SetActive(true);   // �m�F�E�B���h�E��\��
        // �u�n�C/�C�C�G�v�{�^���ɃC�x���g�����蓖�Ă�̂����z�iUI�C�x���g�Őݒ肷��̂�OK�j
    }

    // ====== �u�n�C�v�{�^���������̏��� ======
    public void OnConfirmYes()
    {
        PlayerInventory.obtainedItems.Add(currentItemIndex);

        // ���ȉ��̂悤�ɁuBaseScene�v�����J���悤����i���S��j
        if (SceneManager.GetActiveScene().name.Contains("Stage2"))
        {
            if (!GameManager.Instance.itemDisplayPanel.activeSelf)
                GameManager.Instance.itemDisplayPanel.SetActive(true);
        }

        ItemDisplayManager displayManager = FindObjectOfType<ItemDisplayManager>();
        if (displayManager != null)
        {
            displayManager.RefreshDisplay();
        }
        else
        {
            Debug.Log("Shop�ł� ItemDisplayManager �����݂��Ȃ����ߕ\���X�L�b�v");
        }

        confirmPanel.SetActive(false);

        // �w��������A��b��i�߂�BaseScene�ɖ߂�
        StartCoroutine(EndTalk());
    }

    // ====== �u�C�C�G�v�{�^���������̏��� ======
    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);   // �m�F�E�B���h�E�����
        selector.selecting = true;       // �I����Ԃɖ߂�
    }

    // ====== ��b���I�����Ď��̃V�[���֑J�� ======
    IEnumerator EndTalk()
    {
        twText.ShowText("����ł����H���Ⴀ����΂���");
        yield return WaitZ();
        SceneTransitionInfo.cameFromShop = true; // �V���b�v���痈���t���O�𗧂Ă�
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    // ====== Z�L�[���������܂őҋ@����R���[�`�� ======
    IEnumerator WaitZ()
    {
        while (!Input.GetKeyDown(KeyCode.Z))
            yield return null;
    }
}
