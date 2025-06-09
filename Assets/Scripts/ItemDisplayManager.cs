using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;       // �A�C�e���摜�i��F0:��� 1:�� �c�j
    public Transform parent;           // ���ׂ�e�iCanvas���̋�I�u�W�F�N�g�Ȃǁj

    void Start()
    {
        Debug.Log("�擾�ς݃A�C�e��: " + PlayerInventory.obtainedItems.Count);
        int i = 0;
        foreach (int itemId in PlayerInventory.obtainedItems)
        {
            Debug.Log("�擾�ς݃A�C�e��ID: " + itemId);

            // �V����Image�𐶐�
            GameObject go = new GameObject("ItemImage_" + itemId);
            var img = go.AddComponent<Image>();
            img.sprite = itemSprites[itemId]; // ID����Sprite������ł�O��

            // �e���Z�b�g�iUI�Ƃ���Canvas�̉��ɕ��ׂ�j
            go.transform.SetParent(parent, false);

            // �ʒu�����i��F���ɕ��ׂ�j
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);

            i++;
        }
    }
}
