using UnityEngine;
using UnityEngine.UI;

// �����X���b�g�Ƒ����m�F�_�C�A���O�̊Ǘ�
public class EquipUIManager : MonoBehaviour
{
    public Image equipSlotImage;
    public GameObject confirmPanel;
    public Image yesButtonImage;
    public Image noButtonImage;

    public Sprite emptySlotSprite;

    int select = 0; // 0:Yes 1:No
    bool isConfirming = false;

    int pendingItemId = -1;
    Sprite pendingSprite = null;
    int pendingItemIndex = -1;
    ItemDisplayManager pendingManager = null;

    public bool IsConfirming() { return isConfirming; }

    void Start()
    {
        // �������̉摜�� GameManager ����擾���Ĕ��f
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

        confirmPanel.SetActive(false);
        UpdateButtonUI();
    }

    void Update()
    {
        if (!isConfirming) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select = 1 - select;
            UpdateButtonUI();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (select == 0)
                OnClickYes();
            else
                OnClickNo();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClickNo();
        }
    }

    public void TryEquipItem(int itemId, Sprite itemSprite, int itemIndex, ItemDisplayManager manager)
    {
        if (itemSprite == null)
        {
            Debug.LogWarning("�������悤�Ƃ��Ă��� sprite �� null �ł��IitemId = " + itemId);
            return;
        }

        Debug.Log($"TryEquipItem�Ăяo��: itemId={itemId}, sprite={itemSprite.name}, itemIndex={itemIndex}");

        pendingItemId = itemId;
        pendingSprite = itemSprite;
        pendingItemIndex = itemIndex;
        pendingManager = manager;

        confirmPanel.SetActive(true);
        isConfirming = true;
        select = 0;
        UpdateButtonUI();
    }

    void OnClickYes()
    {
        equipSlotImage.sprite = pendingSprite;
        equipSlotImage.enabled = true;

        GameManager.Instance.equippedItemId = pendingItemId; // �� �d�v�I

        confirmPanel.SetActive(false);
        isConfirming = false;

        if (pendingManager != null && pendingItemIndex >= 0)
            pendingManager.RemoveItemAt(pendingItemIndex);
    }

    void OnClickNo()
    {
        Debug.Log("�C�C�G���������B�p�l������Ēʏ�I����ʂ֖߂�");
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
