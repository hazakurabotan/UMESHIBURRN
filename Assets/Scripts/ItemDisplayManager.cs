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
        // プレイヤーが取得したアイテム数を取得
        int count = PlayerInventory.obtainedItems.Count;
        displayedItems = new GameObject[count];

        // アイテムごとに画像を並べて生成
        for (int i = 0; i < count; i++)
        {
            int itemId = PlayerInventory.obtainedItems[i];   // アイテムID（0,1,...）

            // Imageプレハブを親パネルの子として生成
            GameObject go = Instantiate(itemImagePrefab, parent);

            // デバッグ：位置やサイズを確認したい場合
            RectTransform rt = go.GetComponent<RectTransform>();
            Debug.Log($"生成したImage: {go.name}, anchoredPos: {rt.anchoredPosition}, size: {rt.sizeDelta}, scale: {rt.localScale}");

            // 画像をアイテムIDに応じて差し替え
            go.GetComponent<Image>().sprite = itemSprites[itemId];

            // 横に並べる（ここでは100ピクセルずつズラす例）
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 + 100 * i, 0);

            displayedItems[i] = go; // 配列に登録
        }

        UpdateHighlight(); // 最初の選択状態を反映
    }

    void Update()
    {
        // 何も表示していない場合は何もしない
        if (displayedItems == null || displayedItems.Length == 0) return;

        // 右キーで次のアイテム、左キーで前のアイテム
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

        // Zキーでアイテム使用（例：ID=1が回復アイテム）
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int itemId = PlayerInventory.obtainedItems[selectIndex];
            if (itemId == 1)
            {
                // プレイヤーを探して回復処理
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.Heal(10); // 体力を10回復
                    Debug.Log("体力回復アイテムを使った！");
                }
            }
            else
            {
                Debug.Log("そのアイテムは使えません！");
            }
        }
    }

    // 選択中アイテムだけ大きく表示（強調）
    void UpdateHighlight()
    {
        for (int i = 0; i < displayedItems.Length; i++)
        {
            displayedItems[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
        }
    }
}
