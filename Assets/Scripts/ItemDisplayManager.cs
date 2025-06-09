using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;      // アイテム画像（0:リンゴ 1:ポーション ...）
    public RectTransform parent;      // 並べる親（Canvas内のPanelなど）
    public GameObject itemImagePrefab;// UI ImageのPrefab

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

            // ここでデバッグログ！
            RectTransform rt = go.GetComponent<RectTransform>();
            Debug.Log($"生成したImage: {go.name}, anchoredPos: {rt.anchoredPosition}, size: {rt.sizeDelta}, scale: {rt.localScale}");

            go.GetComponent<Image>().sprite = itemSprites[itemId];
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);
            displayedItems[i] = go;
        }

        UpdateHighlight();
    }

    void Update()
    {
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

        // Zキーで使用（例：ID=1が回復アイテムなら）
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1)
            {
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.Heal(10); // 回復処理
                    Debug.Log("体力回復アイテムを使った！");
                }
            }
            else
            {
                Debug.Log("そのアイテムは使えません！");
            }
        }
    }

    void UpdateHighlight()
    {
        // 選択中アイテムだけ大きく
        for (int i = 0; i < displayedItems.Length; i++)
        {
            displayedItems[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
