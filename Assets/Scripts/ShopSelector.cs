using UnityEngine;
using UnityEngine.UI;

// �V���b�v�̏��i�I��UI���Ǘ�����X�N���v�g
public class ShopSelector : MonoBehaviour
{
    public Image[] items;        // ���i�摜�̔z��iUI Image��Inspector�œo�^�j
    public Sprite selectFrame;   // �n�C���C�g�摜�i�����g�p�Ȃ�폜OK�j

    int selectIndex = 0;         // ���ݑI�𒆂̃C���f�b�N�X
    public bool selecting = false; // �I�����[�h�����ǂ����i�O�������ON/OFF�ł���j

    // ===== �I�����[�h�J�n�i�O������Ă΂��j =====
    public void StartSelect()
    {
        selecting = true;
        UpdateHighlight();
    }

    void Update()
    {
        // �I�����[�h����Ȃ���Ή������Ȃ�
        if (!selecting) return;

        // �m�F�p�l���iConfirmPanel�j���\������Ă���Ԃ͑��삵�Ȃ�
        if (GameObject.Find("ConfirmPanel") != null && GameObject.Find("ConfirmPanel").activeSelf)
            return;

        // �E�L�[�Ŏ��̏��i��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = (selectIndex + 1) % items.Length;
            UpdateHighlight();
        }
        // ���L�[�őO�̏��i��
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = (selectIndex + items.Length - 1) % items.Length;
            UpdateHighlight();
        }
        // Z�L�[�Ō���
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // ShopManager��ShowConfirm���Ăяo���Ċm�F�p�l���\��
            FindObjectOfType<ShopManager>().ShowConfirm(selectIndex);
            selecting = false; // �I�����[�h�I��
        }
    }

    // ===== �I�𒆂̏��i�����傫���i�����\���j =====
    void UpdateHighlight()
    {
        for (int i = 0; i < items.Length; i++)
        {
            // �I�𒆂���1.2�{�A����ȊO�͒ʏ�T�C�Y
            items[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
