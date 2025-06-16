using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// アイテムリストの表示と管理を担当（装備処理は外部にまかせる）
public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;         // アイテム画像（ID順に対応）
    public RectTransform parent;         // アイテム画像を並べるパネル
    public GameObject itemImagePrefab;   // Image付きプレハブ

    int selectIndex = 0;                 // 今選択しているインデックス
    GameObject[] displayedItems;         // 画面に並んだアイテムUIの配列

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

        // 装備確認中は操作禁止
        EquipUIManager ui = FindObjectOfType<EquipUIManager>();
        if (ui != null && ui.IsConfirming()) return;


        // ←→で選択
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

        // Zキーでアイテム使用/装備
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1) // 例: 回復
            {
                UseHealItem();
            }
            else if (itemId == 2) // 装備アイテム
            {
                if (ui != null)
                {
                    // itemId, itemSprite, selectIndex, this(呼び出し元)を渡す
                    ui.TryEquipItem(itemId, itemSprites[itemId], selectIndex, this);
                }
            }
        }
    }

    // 回復アイテム処理（使ったら消す）
    void UseHealItem()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Heal(10);
            Debug.Log("体力回復アイテムを使った！");
        }
        RemoveItemAt(selectIndex); // アイテムを削除
    }

    // 指定インデックスのアイテムをリストとUIから削除（外部からも呼べるようpublicに）
    public void RemoveItemAt(int index)
    {
        Debug.Log($"RemoveItemAt呼ばれた: index={index}");

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
