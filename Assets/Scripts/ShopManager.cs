using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public TypeWriterText twText;
    public ShopSelector selector;
    public GameObject confirmPanel;
    int currentItemIndex;

    // 会話・選択の流れ制御（ステートマシン的にやると楽！）
    IEnumerator ShopFlow()
    {
        twText.ShowText("ようこそ、いらっしゃい");
        yield return WaitZ();

        twText.ShowText("今回はこれをしいれているよ");
        yield return WaitZ();

        selector.StartSelect();
        yield return new WaitUntil(() => selector.selecting == false); // 選択が終わるまで待機

        // 決定時に↓を呼ばれる
        // ShowConfirm(selectedIndex)
    }

    public void ShowConfirm(int idx)
    {
        currentItemIndex = idx;
        confirmPanel.SetActive(true); // 「こちらでいいですか？」UIをON
        // ハイ/イイエのボタンにメソッド割り当て
    }

    public void OnConfirmYes()
    {
        // 商品消す
        selector.items[currentItemIndex].gameObject.SetActive(false);
        confirmPanel.SetActive(false);

        // 最後の会話
        StartCoroutine(EndTalk());
    }

    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
        selector.selecting = true;
    }

    IEnumerator EndTalk()
    {
        twText.ShowText("それでいい？じゃあがんばって");
        yield return WaitZ();
        // シーン戻る
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    // Z待ちコルーチン
    IEnumerator WaitZ()
    {
        while (!Input.GetKeyDown(KeyCode.Z)) yield return null;
    }
}
