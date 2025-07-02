using UnityEngine;
using UnityEngine.UI;

// ------------------------------------------------------
// EquipUIManager
// �����X���b�g��UI�ƁA�������邩�ǂ����̊m�F�p�l�����Ǘ�����X�N���v�g
// ------------------------------------------------------
public class EquipUIManager : MonoBehaviour
{
    // �����X���b�g�\���p��Image
    public Image equipSlotImage;
    // �u�������܂����H�v�_�C�A���O�p�p�l��
    public GameObject confirmPanel;
    // �u�͂��v�u�������v�{�^����Image
    public Image yesButtonImage;
    public Image noButtonImage;

    // ��X���b�g�\���p��Sprite�i���g�p�A�C�e���j
    public Sprite emptySlotSprite;

    // ���݂ǂ����I�����Ă��邩�i0=Yes, 1=No�j
    int select = 0;
    // �m�F�p�l���\�������ǂ���
    bool isConfirming = false;

    // �����\��A�C�e���̏��i�p�l���ŕۗ����̂��́j
    int pendingItemId = -1;
    Sprite pendingSprite = null;
    int pendingItemIndex = -1;
    ItemDisplayManager pendingManager = null;

    // ���m�F�_�C�A���O���o�Ă��邩�ǂ����O������擾
    public bool IsConfirming() { return isConfirming; }

    // ������
    void Start()
    {
        // �Q�[���J�n���A���ݑ������̃X�v���C�g�摜���擾���ĕ\��
        Sprite sprite = GameManager.Instance.GetEquippedSprite();
        if (sprite != null)
        {
            equipSlotImage.sprite = sprite;
            equipSlotImage.enabled = true;
        }
        else
        {
            equipSlotImage.enabled = false;
        }

        // �m�F�p�l���͍ŏ��͔�\��
        confirmPanel.SetActive(false);
        UpdateButtonUI();
    }

    // ���t���[���Ă΂��
    void Update()
    {
        // �m�F�_�C�A���O�\�����̂ݓ��͂���t
        if (!isConfirming) return;

        // ���E�L�[�Łu�͂��v�u�������v�؂�ւ�
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select = 1 - select; // 0��1, 1��0��؂�ւ�
            UpdateButtonUI();
        }
        // Z�L�[�Ō���i�͂�/�������j
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (select == 0)
                OnClickYes();
            else
                OnClickNo();
        }
        // X�L�[�ŃL�����Z���i�C�C�G�j
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClickNo();
        }
    }

    // �A�C�e���𑕔����悤�Ƃ������ɌĂ΂��
    public void TryEquipItem(int itemId, Sprite itemSprite, int itemIndex, ItemDisplayManager manager)
    {
        // �X�v���C�g���ݒ肳��Ă��Ȃ���Όx�����o���ďI��
        if (itemSprite == null)
        {
            Debug.LogWarning("�������悤�Ƃ��Ă��� sprite �� null �ł��IitemId = " + itemId);
            return;
        }

        Debug.Log($"TryEquipItem�Ăяo��: itemId={itemId}, sprite={itemSprite.name}, itemIndex={itemIndex}");

        // �u�����\��v�f�[�^��ێ�
        pendingItemId = itemId;
        pendingSprite = itemSprite;
        pendingItemIndex = itemIndex;
        pendingManager = manager;

        // �m�F�p�l����\��
        confirmPanel.SetActive(true);
        isConfirming = true;
        select = 0;
        UpdateButtonUI();
    }

    // �u�͂��v�{�^�������������̏���
    void OnClickYes()
    {
        // �X���b�g�摜��V�����A�C�e���摜�ɕύX
        equipSlotImage.sprite = pendingSprite;
        equipSlotImage.enabled = true;

        // GameManager�ɑI���A�C�e��ID��ʒm
        GameManager.Instance.equippedItemId = pendingItemId;

        // �m�F�p�l�������
        confirmPanel.SetActive(false);
        isConfirming = false;

        // ���������A�C�e�����ꗗ��������imanager������΁j
        if (pendingManager != null && pendingItemIndex >= 0)
            pendingManager.RemoveItemAt(pendingItemIndex);
    }

    // �u�������v�{�^�������������̏���
    void OnClickNo()
    {
        Debug.Log("�C�C�G���������B�p�l������Ēʏ�I����ʂ֖߂�");
        confirmPanel.SetActive(false);
        isConfirming = false;
    }

    // �{�^��UI�̐F��g��k���Ȃǐ؂�ւ��\��
    void UpdateButtonUI()
    {
        yesButtonImage.color = (select == 0) ? Color.yellow : Color.white;
        noButtonImage.color = (select == 1) ? Color.yellow : Color.white;
        yesButtonImage.transform.localScale = (select == 0) ? Vector3.one * 1.2f : Vector3.one;
        noButtonImage.transform.localScale = (select == 1) ? Vector3.one * 1.2f : Vector3.one;
    }
}
