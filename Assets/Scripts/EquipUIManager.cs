using UnityEngine;
using UnityEngine.UI;

// �����X���b�g�Ƒ����m�F�_�C�A���O�̊Ǘ�
public class EquipUIManager : MonoBehaviour
{
    public Image equipSlotImage;         // �����g�i�����ɃA�C�e���摜���o��j
    public GameObject confirmPanel;      // �m�F�p�l��
    public Image yesButtonImage;         // �u�͂��v�{�^���̉摜
    public Image noButtonImage;          // �u�������v�{�^���̉摜

    public bool IsConfirming() { return isConfirming; }

    public Sprite emptySlotSprite;       // �����Ȃ��̉摜
    // �����������A�C�e���摜�͎��s���Ɏ󂯎��̂ŁA�����ɂ͌Œ肵�Ȃ�

    int select = 0; // 0:Yes 1:No
    bool isConfirming = false;

    // �������A�C�e�����i�ꎞ�I�ɕێ��j
    int pendingItemId = -1;
    Sprite pendingSprite = null;
    int pendingItemIndex = -1;
    ItemDisplayManager pendingManager = null;

    void Start()
    {
        equipSlotImage.sprite = emptySlotSprite;
        Debug.Log($"�����g�摜�Z�b�g: {pendingSprite?.name}");
        confirmPanel.SetActive(false);
        UpdateButtonUI();
    }

    void Update()
    {
        if (!isConfirming) return;

        // �����őI��
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select = 1 - select;
            UpdateButtonUI();
        }
        // Z�Ō���
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (select == 0)
                OnClickYes();
            else
                OnClickNo();
        }
        // X�ŃL�����Z��
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClickNo();
        }
    }

    // �����A�C�e�����I�����ꂽ���ɌĂ΂��B�K�v�ȏ���S�Ď󂯎��
    public void TryEquipItem(int itemId, Sprite itemSprite, int itemIndex, ItemDisplayManager manager)
    {

        Debug.Log($"TryEquipItem�Ăяo��: itemId={itemId}, sprite={itemSprite?.name}, itemIndex={itemIndex}");

        pendingItemId = itemId;
        pendingSprite = itemSprite;
        pendingItemIndex = itemIndex;
        pendingManager = manager;

        confirmPanel.SetActive(true);
        isConfirming = true;
        select = 0;
        UpdateButtonUI();
    }

    // �u�͂��v�F���ۂɑ����g�֕\�������X�g/��ʂ���A�C�e�����폜
    void OnClickYes()
    {
        Debug.Log($"OnClickYes: pendingItemId={pendingItemId}, pendingSprite={pendingSprite?.name}, pendingItemIndex={pendingItemIndex}");

        equipSlotImage.sprite = pendingSprite;
        confirmPanel.SetActive(false);
        isConfirming = false;
        if (pendingManager != null && pendingItemIndex >= 0)
            pendingManager.RemoveItemAt(pendingItemIndex);

        // �ǉ����ʂȂ�
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
            Debug.Log($"���݂̃v���C���[�U����: {player.bulletDamage}");
    }


    // �u�������v�F������������
    void OnClickNo()
    {
        confirmPanel.SetActive(false);
        isConfirming = false;
    }


    void UpdateButtonUI()
    {
        yesButtonImage.color = (select == 0) ? Color.yellow : Color.white;
        noButtonImage.color = (select == 1) ? Color.yellow : Color.white;
        yesButtonImage.transform.localScale = (select == 0) ? Vector3.one * 1.2f : Vector3.one;
        noButtonImage.transform.localScale = (select == 1) ? Vector3.one * 1.2f : Vector3.one;
    }
}
