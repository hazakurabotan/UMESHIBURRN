using UnityEngine;
using UnityEngine.UI;

// �u�͂��^�������v�Ȃ�2��I�����̃J�[�\��UI�𐧌䂷��N���X
public class ConfirmSelector : MonoBehaviour
{
    public GameObject yesHighlight;   // �u�͂��v�I�𒆂ɋ����\������UI�i��F�g��F��ς���p�l�����j
    public GameObject noHighlight;    // �u�������v�I�𒆂ɋ����\������UI

    int selectIndex = 0; // �I�𒆂̃C���f�b�N�X�i0�Ȃ�u�͂��v�A1�Ȃ�u�������v�j
    public ShopManager shopManager; // �V���b�v�Ǘ��X�N���v�g�iInspector�Ŋ��蓖�āj

    // UI���A�N�e�B�u�ɂȂ����u�ԂɌĂ΂��i�I����Ԃ��������j
    void OnEnable()
    {
        selectIndex = 0; // ������Ԃ́u�͂��v
        UpdateHighlight(); // �����\�����X�V
    }

    // ���t���[���Ă΂��
    void Update()
    {
        // �����̃I�u�W�F�N�g����\���Ȃ牽�����Ȃ�
        if (!gameObject.activeSelf) return;

        // ���E�L�[�������ꂽ��I������؂�ւ�
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = 1 - selectIndex; // 0��1���g�O���؂�ւ�
            UpdateHighlight(); // �����\�����X�V
        }

        // Z�L�[�������ꂽ�猈��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (selectIndex == 0)
                shopManager.OnConfirmYes(); // �u�͂��v�I����
            else
                shopManager.OnConfirmNo();  // �u�������v�I����
        }
    }

    // �I�𒆂̃{�^�����������\������֐�
    void UpdateHighlight()
    {
        yesHighlight.SetActive(selectIndex == 0); // �u�͂��v�I�𒆂Ȃ狭��
        noHighlight.SetActive(selectIndex == 1);  // �u�������v�I�𒆂Ȃ狭��
    }
}
