using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// プレイヤーが持っているアイテム一覧をUIで表示・選択するクラス
public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;        // アイテム画像リスト（例: 0=リンゴ, 1=ポーション…）
    public RectTransform parent;        // 並べる親オブジェクト（UIのPanelなどを指定）
    public GameObject itemImagePrefab;  // アイテム画像用のプレハブ（ImageがついてるPrefab）

    int selectIndex = 0;                // 現在選択中のインデックス
    GameObject[] displayedItems;        // 画面上に生成されたアイテムオブジェクト配列

    void Start()
    {
        // ここでは SetActive(false) しない（GameManagerが管理）
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
        if (!gameObject.activeSelf) return; // パネル非表示中はスキップ
        if (displayedItems == null || displayedItems.Length == 0) return;

        // ←→キーで選択
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

        // Zキーでアイテム使用
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1) // 例：ID1が回復アイテム
            {
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.Heal(10);
                    Debug.Log("体力回復アイテムを使った！");
                }
                // 1. アイテムリストから削除
                PlayerInventory.obtainedItems.RemoveAt(selectIndex);

                // 2. 画面上のImageを消す
                Destroy(displayedItems[selectIndex]);

                // 3. 配列も詰め直す
                var newList = new List<GameObject>(displayedItems);
                newList.RemoveAt(selectIndex);
                displayedItems = newList.ToArray();

                // 4. 選択インデックスの調整
                if (selectIndex >= displayedItems.Length)
                    selectIndex = Mathf.Max(0, displayedItems.Length - 1);

                // 5. 並び直し
                for (int i = 0; i < displayedItems.Length; i++)
                {
                    displayedItems[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
                }
                UpdateHighlight();
            }
            else
            {
                Debug.Log("そのアイテムは使えません！");
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
