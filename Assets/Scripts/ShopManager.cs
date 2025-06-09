using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    public TypeWriterText twText;
    public ShopSelector selector;
    public GameObject confirmPanel;
    int currentItemIndex;

    void Start()
    {
        StartCoroutine(ShopFlow());
    }

    // 会話・選択の流れ制御
    IEnumerator ShopFlow()
    {
        twText.ShowText("ようこそ、いらっしゃい");
        yield return WaitZ();

        twText.ShowText("今回はこれをしいれているよ");
        yield return WaitZ();

        selector.StartSelect();
        yield return new WaitUntil(() => selector.selecting == false);
        // 決定時 ShowConfirm(selectedIndex)を呼ぶ
    }

    public void ShowConfirm(int idx)
    {
        currentItemIndex = idx;
        confirmPanel.SetActive(true);
        // ハイ/イイエのボタンにメソッド割り当て
    }

    public void OnConfirmYes()
    {
        selector.items[currentItemIndex].gameObject.SetActive(false);
        confirmPanel.SetActive(false);

        // 例：ShopManagerやOnConfirmYesなどで
        PlayerInventory.obtainedItems.Add(currentItemIndex);  // ←ここ！

        Debug.Log("現在のPlayerInventory: " + string.Join(",", PlayerInventory.obtainedItems));


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
        SceneTransitionInfo.cameFromShop = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    IEnumerator WaitZ()
    {
        while (!Input.GetKeyDown(KeyCode.Z)) yield return null;
    }
}
