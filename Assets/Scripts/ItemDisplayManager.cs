using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �v���C���[�������Ă���A�C�e���ꗗ��UI�ŕ\���E�I������N���X
public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;        // �A�C�e���摜���X�g�i��: 0=�����S, 1=�|�[�V�����c�j
    public RectTransform parent;        // ���ׂ�e�I�u�W�F�N�g�iUI��Panel�Ȃǂ��w��j
    public GameObject itemImagePrefab;  // �A�C�e���摜�p�̃v���n�u�iImage�����Ă�Prefab�j

    int selectIndex = 0;                // ���ݑI�𒆂̃C���f�b�N�X
    GameObject[] displayedItems;        // ��ʏ�ɐ������ꂽ�A�C�e���I�u�W�F�N�g�z��

    void Start()
    {
        // �����ł� SetActive(false) ���Ȃ��iGameManager���Ǘ��j
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
        if (!gameObject.activeSelf) return; // �p�l����\�����̓X�L�b�v
        if (displayedItems == null || displayedItems.Length == 0) return;

        // �����L�[�őI��
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

        // Z�L�[�ŃA�C�e���g�p
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1) // ��FID1���񕜃A�C�e��
            {
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.Heal(10);
                    Debug.Log("�̗͉񕜃A�C�e�����g�����I");
                }
                // 1. �A�C�e�����X�g����폜
                PlayerInventory.obtainedItems.RemoveAt(selectIndex);

                // 2. ��ʏ��Image������
                Destroy(displayedItems[selectIndex]);

                // 3. �z����l�ߒ���
                var newList = new List<GameObject>(displayedItems);
                newList.RemoveAt(selectIndex);
                displayedItems = newList.ToArray();

                // 4. �I���C���f�b�N�X�̒���
                if (selectIndex >= displayedItems.Length)
                    selectIndex = Mathf.Max(0, displayedItems.Length - 1);

                // 5. ���ђ���
                for (int i = 0; i < displayedItems.Length; i++)
                {
                    displayedItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
                }
                UpdateHighlight();
            }
            else
            {
                Debug.Log("���̃A�C�e���͎g���܂���I");
            }
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
