using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ----------------------------------------------
// ItemDisplayManager
// �A�C�e�����X�g�iUI�j�̕\���E�I���E�폜��S������X�N���v�g
// �u���������v�͊O���iEquipUIManager�j�ɈϏ�
// ----------------------------------------------
public class ItemDisplayManager : MonoBehaviour
{
    public RectTransform parent;         // �A�C�e���摜����ׂ�e�p�l��
    public GameObject itemImagePrefab;   // 1�A�C�e���ɂ�Image�t����UI�v���n�u

    int selectIndex = 0;                 // ���ݑI�𒆂̃C���f�b�N�X
    GameObject[] displayedItems;         // �\������Ă���A�C�e��UI�̔z��

    void Start()
    {
        Debug.Log("ItemDisplayManager.Start() �Ăяo���ꂽ");

        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        // --- �����A�C�e�����Ԃ�UI�𐶐����ĕ��ׂ� ---
        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];
            Debug.Log($"itemId[{i}] = {itemId}");
            GameObject go = Instantiate(itemImagePrefab, parent);

            // GameManager����Ή�����A�C�e���摜�iSprite�j���擾
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

            // �����ɔz�u
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
            displayedItems[i] = go;
        }
        UpdateHighlight();
    }

    void Update()
    {
        // ��\������A�C�e�������Ȃ牽�����Ȃ�
        if (!gameObject.activeSelf) return;
        if (displayedItems == null || displayedItems.Length == 0) return;

        // �����m�F���͑���s��
        EquipUIManager ui = FindObjectOfType<EquipUIManager>();
        if (ui != null && ui.IsConfirming()) return;

        // �����L�[�őI���C���f�b�N�X�ؑ�
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

        // Z�L�[�ŃA�C�e���g�p�⑕��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1) // ��F�񕜃A�C�e��
            {
                UseHealItem();
            }
            else if (itemId == 2) // ��F�����A�C�e��
            {
                if (ui != null)
                {
                    // �����m�F�p�l����Sprite����n���đ����������˗�
                    Sprite sprite = (itemId >= 0 && itemId < GameManager.Instance.itemSprites.Length)
                        ? GameManager.Instance.itemSprites[itemId]
                        : null;

                    ui.TryEquipItem(itemId, sprite, selectIndex, this);
                }
            }
        }
    }

    // --- �񕜃A�C�e���g�p���� ---
    void UseHealItem()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Heal(10); // �̗�10��
            Debug.Log("�̗͉񕜃A�C�e�����g�����I");
        }
        RemoveItemAt(selectIndex); // �g���������
    }

    // --- �w��C���f�b�N�X�̃A�C�e����UI�ƃ��X�g����폜 ---
    public void RemoveItemAt(int index)
    {
        Debug.Log($"RemoveItemAt�Ă΂ꂽ: index={index}");

        PlayerInventory.obtainedItems.RemoveAt(index); // �f�[�^�ォ��폜
        Destroy(displayedItems[index]);                // �\��UI���폜

        // �z�񂩂���Y���v�f�����O
        var newList = new List<GameObject>(displayedItems);
        newList.RemoveAt(index);
        displayedItems = newList.ToArray();

        // �C���f�b�N�X����
        if (selectIndex >= displayedItems.Length)
            selectIndex = Mathf.Max(0, displayedItems.Length - 1);

        RearrangeItems();   // ���ђ���
        UpdateHighlight();  // �n�C���C�g���X�V
    }

    // --- �A�C�e���������ɕ��ђ��� ---
    // �z����̑S�A�C�e�����A�w��ʒu�ɉ����тŔz�u����֐�
    void RearrangeItems()
    {
        for (int i = 0; i < displayedItems.Length; i++)
        {
            // �擪��x=60�A���̌�100px�����ɂ��炵�Ĕz�u
            displayedItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
        }
    }

    // --- �I�𒆃A�C�e�����g�債�ăn�C���C�g ---
    // ���ݑI�����Ă���A�C�e�������������傫���\������i�n�C���C�g���o�j
    void UpdateHighlight()
    {
        for (int i = 0; i < displayedItems.Length; i++)
        {
            // �I�𒆂�1.2�{�g��A����ȊO�͓��{�ɖ߂�
            displayedItems[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }


    // --- �������X�g���e���ς�����Ƃ��ɍĕ`�悷��֐� ---
    // �A�C�e���ǉ��E�폜�E�����ύX���ȂǂɁuUI���ŐV��Ԃɍ�蒼���v���߂̊֐�
    public void RefreshDisplay()
    {
        // --- �܂��A���\�����̃A�C�e��UI�����ׂď��� ---
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject); // �q�I�u�W�F�N�g�S���폜
        }

        // --- �V���������A�C�e���������z����Ċm�� ---
        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        // --- �e�A�C�e�����Ƃ�UI��V�������� ---
        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];
            GameObject go = Instantiate(itemImagePrefab, parent); // �v���n�u�𕡐����e�ɃZ�b�g

            // �A�C�e��ID�ɑΉ�����X�v���C�g�摜���擾
            Sprite sprite = (itemId >= 0 && itemId < GameManager.Instance.itemSprites.Length)
                ? GameManager.Instance.itemSprites[itemId]
                : null;

            if (sprite != null)
            {
                go.GetComponent<Image>().sprite = sprite; // �摜�����ւ�
            }
            else
            {
                Debug.LogWarning($"sprite ��������Ȃ� itemId = {itemId}");
            }

            // �����ɕ��ׂ�
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
            displayedItems[i] = go;
        }

        selectIndex = 0; // �ĕ`�掞�͐擪�A�C�e����I��
        UpdateHighlight(); // �n�C���C�g���X�V
    }

}
