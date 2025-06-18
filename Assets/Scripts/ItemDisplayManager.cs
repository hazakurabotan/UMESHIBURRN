using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �A�C�e�����X�g�̕\���ƊǗ���S���i���������͊O���ɂ܂�����j
public class ItemDisplayManager : MonoBehaviour
{
   
    public RectTransform parent;         // �A�C�e���摜����ׂ�p�l��
    public GameObject itemImagePrefab;   // Image�t���v���n�u

    int selectIndex = 0;                 // ���I�����Ă���C���f�b�N�X
    GameObject[] displayedItems;         // ��ʂɕ��񂾃A�C�e��UI�̔z��

    void Start()
    {
        Debug.Log("ItemDisplayManager.Start() �Ăяo���ꂽ");

        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];
            Debug.Log($"itemId[{i}] = {itemId}");
            GameObject go = Instantiate(itemImagePrefab, parent);

            // GameManager����X�v���C�g���擾
            Sprite sprite = (itemId >= 0 && itemId < GameManager.Instance.itemSprites.Length)
                ? GameManager.Instance.itemSprites[itemId]
                : null;

            if (sprite != null)
            {
                go.GetComponent<Image>().sprite = sprite;
                Debug.Log($"itemId[{itemId}] �p sprite = {sprite.name}");
            }
            else
            {
                Debug.LogWarning($"sprite �� null�I itemId = {itemId}");
            }

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
                    // �� ������ itemId �ɑΉ����� Sprite �� GameManager ����擾�I
                    Sprite sprite = (itemId >= 0 && itemId < GameManager.Instance.itemSprites.Length)
    ? GameManager.Instance.itemSprites[itemId]
    : null;

                    ui.TryEquipItem(itemId, sprite, selectIndex, this);
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

    public void RefreshDisplay()
    {
        // �Â��A�C�e��UI�����ׂč폜
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];
            GameObject go = Instantiate(itemImagePrefab, parent);

            Sprite sprite = (itemId >= 0 && itemId < GameManager.Instance.itemSprites.Length)
                ? GameManager.Instance.itemSprites[itemId]
                : null;

            if (sprite != null)
            {
                go.GetComponent<Image>().sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"sprite ��������Ȃ� itemId = {itemId}");
            }

            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
            displayedItems[i] = go;
        }

        selectIndex = 0;
        UpdateHighlight();
    }

}
