using UnityEngine;
using UnityEngine.UI;

public class ShopSelector : MonoBehaviour
{
    public Image[] items;        // 商品画像 (UIの場合)
    public Sprite selectFrame;   // ハイライト画像

    int selectIndex = 0;
    public bool selecting = false; // ←ここをpublicに！（ShopManagerから操作できる）

    public void StartSelect()
    {
        selecting = true;
        UpdateHighlight();
    }

    void Update()
    {
        if (!selecting) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = (selectIndex + 1) % items.Length;
            UpdateHighlight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = (selectIndex + items.Length - 1) % items.Length;
            UpdateHighlight();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 確認ダイアログ表示
            FindObjectOfType<ShopManager>().ShowConfirm(selectIndex);
            selecting = false;
        }
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < items.Length; i++)
            items[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
    }
}
