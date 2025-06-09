using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    public Sprite[] itemSprites;       // アイテム画像（例：0:りんご 1:剣 …）
    public Transform parent;           // 並べる親（Canvas内の空オブジェクトなど）

    void Start()
    {
        Debug.Log("取得済みアイテム: " + PlayerInventory.obtainedItems.Count);
        int i = 0;
        foreach (int itemId in PlayerInventory.obtainedItems)
        {
            Debug.Log("取得済みアイテムID: " + itemId);

            // 新しいImageを生成
            GameObject go = new GameObject("ItemImage_" + itemId);
            var img = go.AddComponent<Image>();
            img.sprite = itemSprites[itemId]; // ID順にSpriteが並んでる前提

            // 親をセット（UIとしてCanvasの下に並べる）
            go.transform.SetParent(parent, false);

            // 位置調整（例：横に並べる）
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);

            i++;
        }
    }
}
