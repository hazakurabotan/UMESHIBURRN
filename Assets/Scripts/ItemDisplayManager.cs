using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;      // �A�C�e���摜�i0:�����S 1:�|�[�V���� ...�j
    public RectTransform parent;      // ���ׂ�e�iCanvas����Panel�Ȃǁj
    public GameObject itemImagePrefab;// UI Image��Prefab

    int selectIndex = 0;
    GameObject[] displayedItems;

    void Start()
    {
        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];
            GameObject go = Instantiate(itemImagePrefab, parent);

            // �����Ńf�o�b�O���O�I
            RectTransform rt = go.GetComponent<RectTransform>();
            Debug.Log($"��������Image: {go.name}, anchoredPos: {rt.anchoredPosition}, size: {rt.sizeDelta}, scale: {rt.localScale}");

            go.GetComponent<Image>().sprite = itemSprites[itemId];
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
            displayedItems[i] = go;
        }

        UpdateHighlight();
    }

    void Update()
    {
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

        // Z�L�[�Ŏg�p�i��FID=1���񕜃A�C�e���Ȃ�j
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1)
            {
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.Heal(10); // �񕜏���
                    Debug.Log("�̗͉񕜃A�C�e�����g�����I");
                }
            }
            else
            {
                Debug.Log("���̃A�C�e���͎g���܂���I");
            }
        }
    }

    void UpdateHighlight()
    {
        // �I�𒆃A�C�e�������傫��
        for (int i = 0; i < displayedItems.Length; i++)
        {
            displayedItems[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
