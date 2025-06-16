using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �A�C�e�����X�g�̕\���ƊǗ���S���i���������͊O���ɂ܂�����j
public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;         // �A�C�e���摜�iID���ɑΉ��j
    public RectTransform parent;         // �A�C�e���摜����ׂ�p�l��
    public GameObject itemImagePrefab;   // Image�t���v���n�u

    int selectIndex = 0;                 // ���I�����Ă���C���f�b�N�X
    GameObject[] displayedItems;         // ��ʂɕ��񂾃A�C�e��UI�̔z��

    void Start()
    {
        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];
            GameObject go = Instantiate(itemImagePrefab, parent);
            go.GetComponent<Image>().sprite = itemSprites[itemId];
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
            displayedItems[i] = go;
        }
        UpdateHighlight();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;
        if (displayedItems == null || displayedItems.Length == 0) return;

        // �����m�F���͑���֎~
        EquipUIManager ui = FindObjectOfType<EquipUIManager>();
        if (ui != null && ui.IsConfirming()) return;


        // �����őI��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = (selectIndex + 1) % displayedItems.Length;
            UpdateHighlight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = (selectIndex - 1 + displayedItems.Length) % displayedItems.Length;
            UpdateHighlight();
        }

        // Z�L�[�ŃA�C�e���g�p/����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1) // ��: ��
            {
                UseHealItem();
            }
            else if (itemId == 2) // �����A�C�e��
            {
                if (ui != null)
                {
                    // itemId, itemSprite, selectIndex, this(�Ăяo����)��n��
                    ui.TryEquipItem(itemId, itemSprites[itemId], selectIndex, this);
                }
            }
        }
    }

    // �񕜃A�C�e�������i�g����������j
    void UseHealItem()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Heal(10);
            Debug.Log("�̗͉񕜃A�C�e�����g�����I");
        }
        RemoveItemAt(selectIndex); // �A�C�e�����폜
    }

    // �w��C���f�b�N�X�̃A�C�e�������X�g��UI����폜�i�O��������Ăׂ�悤public�Ɂj
    public void RemoveItemAt(int index)
    {
        Debug.Log($"RemoveItemAt�Ă΂ꂽ: index={index}");

        PlayerInventory.obtainedItems.RemoveAt(index);
        Destroy(displayedItems[index]);
        var newList = new List<GameObject>(displayedItems);
        newList.RemoveAt(index);
        displayedItems = newList.ToArray();

        if (selectIndex >= displayedItems.Length)
            selectIndex = Mathf.Max(0, displayedItems.Length - 1);

        RearrangeItems();
        UpdateHighlight();
    }

    void RearrangeItems()
    {
        for (int i = 0; i < displayedItems.Length; i++)
        {
            displayedItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
        }
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < displayedItems.Length; i++)
        {
            displayedItems[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
