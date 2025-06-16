using UnityEngine;
using UnityEngine.UI;

// 装備スロットと装備確認ダイアログの管理
public class EquipUIManager : MonoBehaviour
{
    public Image equipSlotImage;         // 装備枠（ここにアイテム画像が出る）
    public GameObject confirmPanel;      // 確認パネル
    public Image yesButtonImage;         // 「はい」ボタンの画像
    public Image noButtonImage;          // 「いいえ」ボタンの画像

    public bool IsConfirming() { return isConfirming; }

    public Sprite emptySlotSprite;       // 装備なしの画像
    // 装備したいアイテム画像は実行時に受け取るので、ここには固定しない

    int select = 0; // 0:Yes 1:No
    bool isConfirming = false;

    // 装備候補アイテム情報（一時的に保持）
    int pendingItemId = -1;
    Sprite pendingSprite = null;
    int pendingItemIndex = -1;
    ItemDisplayManager pendingManager = null;

    void Start()
    {
        equipSlotImage.sprite = emptySlotSprite;
        Debug.Log($"装備枠画像セット: {pendingSprite?.name}");
        confirmPanel.SetActive(false);
        UpdateButtonUI();
    }

    void Update()
    {
        if (!isConfirming) return;

        // ←→で選択
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            select = 1 - select;
            UpdateButtonUI();
        }
        // Zで決定
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (select == 0)
                OnClickYes();
            else
                OnClickNo();
        }
        // Xでキャンセル
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClickNo();
        }
    }

    // 装備アイテムが選択された時に呼ばれる。必要な情報を全て受け取る
    public void TryEquipItem(int itemId, Sprite itemSprite, int itemIndex, ItemDisplayManager manager)
    {

        Debug.Log($"TryEquipItem呼び出し: itemId={itemId}, sprite={itemSprite?.name}, itemIndex={itemIndex}");

        pendingItemId = itemId;
        pendingSprite = itemSprite;
        pendingItemIndex = itemIndex;
        pendingManager = manager;

        confirmPanel.SetActive(true);
        isConfirming = true;
        select = 0;
        UpdateButtonUI();
    }

    // 「はい」：実際に装備枠へ表示＆リスト/画面からアイテムを削除
    void OnClickYes()
    {
        Debug.Log($"OnClickYes: pendingItemId={pendingItemId}, pendingSprite={pendingSprite?.name}, pendingItemIndex={pendingItemIndex}");

        equipSlotImage.sprite = pendingSprite;
        confirmPanel.SetActive(false);
        isConfirming = false;
        if (pendingManager != null && pendingItemIndex >= 0)
            pendingManager.RemoveItemAt(pendingItemIndex);

        // 追加効果など
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
            Debug.Log($"現在のプレイヤー攻撃力: {player.bulletDamage}");
    }


    // 「いいえ」：何もせず閉じる
    void OnClickNo()
    {
        confirmPanel.SetActive(false);
        isConfirming = false;
    }


    void UpdateButtonUI()
    {
        yesButtonImage.color = (select == 0) ? Color.yellow : Color.white;
        noButtonImage.color = (select == 1) ? Color.yellow : Color.white;
        yesButtonImage.transform.localScale = (select == 0) ? Vector3.one * 1.2f : Vector3.one;
        noButtonImage.transform.localScale = (select == 1) ? Vector3.one * 1.2f : Vector3.one;
    }
}
