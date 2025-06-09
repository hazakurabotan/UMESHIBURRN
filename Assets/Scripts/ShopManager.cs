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

    // ��b�E�I���̗��ꐧ��
    IEnumerator ShopFlow()
    {
        twText.ShowText("�悤�����A��������Ⴂ");
        yield return WaitZ();

        twText.ShowText("����͂����������Ă����");
        yield return WaitZ();

        selector.StartSelect();
        yield return new WaitUntil(() => selector.selecting == false);
        // ���莞 ShowConfirm(selectedIndex)���Ă�
    }

    public void ShowConfirm(int idx)
    {
        currentItemIndex = idx;
        confirmPanel.SetActive(true);
        // �n�C/�C�C�G�̃{�^���Ƀ��\�b�h���蓖��
    }

    public void OnConfirmYes()
    {
        selector.items[currentItemIndex].gameObject.SetActive(false);
        confirmPanel.SetActive(false);

        // ��FShopManager��OnConfirmYes�Ȃǂ�
        PlayerInventory.obtainedItems.Add(currentItemIndex);  // �������I

        Debug.Log("���݂�PlayerInventory: " + string.Join(",", PlayerInventory.obtainedItems));


        StartCoroutine(EndTalk());
    }

    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
        selector.selecting = true;
    }

    IEnumerator EndTalk()
    {
        twText.ShowText("����ł����H���Ⴀ����΂���");
        yield return WaitZ();
        SceneTransitionInfo.cameFromShop = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
    }

    IEnumerator WaitZ()
    {
        while (!Input.GetKeyDown(KeyCode.Z)) yield return null;
    }
}
