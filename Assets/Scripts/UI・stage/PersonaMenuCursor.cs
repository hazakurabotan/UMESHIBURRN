using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonaMenuCursor : MonoBehaviour
{
    public RectTransform cursor;   // 「選択カーソル」ImageのRectTransform
    public List<RectTransform> menuItems; // メニュー項目ImageのRectTransformリスト

    public float moveSpeed = 0.2f; // カーソル移動のなめらかさ
    public PersonaMenuDetail detailPanelScript;

    private int currentIndex = 0;

    void Start()
    {
        if (menuItems != null && menuItems.Count > 0 && cursor != null)
            cursor.anchoredPosition = menuItems[0].anchoredPosition;
        else
            Debug.LogWarning("PersonaMenuCursor: menuItemsが空です！");
    }

    void Update()
    {
        if (menuItems == null || menuItems.Count == 0 || cursor == null)
            return; // 何もなければスルー

        // 入力取得
        if (Input.GetKeyDown(KeyCode.RightArrow)) Next();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Prev();

        // カーソル移動アニメ
        cursor.anchoredPosition = Vector2.Lerp(
            cursor.anchoredPosition,
            menuItems[currentIndex].anchoredPosition,
            moveSpeed
        );

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (detailPanelScript != null)
                detailPanelScript.ShowDetail(currentIndex); // 現在選択中のindexで詳細表示
        }
    }

    void Next()
    {
        if (menuItems.Count == 0) return;
        currentIndex = (currentIndex + 1) % menuItems.Count;
    }

    void Prev()
    {
        if (menuItems.Count == 0) return;
        currentIndex = (currentIndex - 1 + menuItems.Count) % menuItems.Count;
    }
}
