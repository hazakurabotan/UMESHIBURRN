using UnityEngine;
using UnityEngine.UI;

public class ConfirmSelector : MonoBehaviour
{
    public GameObject yesHighlight;   // 「ハイ」選択時に枠や色を変えるUI
    public GameObject noHighlight;    // 「イイエ」選択時に枠や色を変えるUI

    int selectIndex = 0; // 0:ハイ, 1:イイエ
    public ShopManager shopManager; // Inspectorでアサイン

    void OnEnable()
    {
        selectIndex = 0; // 初期化
        UpdateHighlight();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = 1 - selectIndex; // 0⇔1切り替え
            UpdateHighlight();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (selectIndex == 0)
                shopManager.OnConfirmYes();
            else
                shopManager.OnConfirmNo();
        }
    }

    void UpdateHighlight()
    {
        yesHighlight.SetActive(selectIndex == 0);
        noHighlight.SetActive(selectIndex == 1);
    }
}
