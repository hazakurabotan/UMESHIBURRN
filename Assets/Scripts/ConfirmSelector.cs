using UnityEngine;
using UnityEngine.UI;

public class ConfirmSelector : MonoBehaviour
{
    public GameObject yesHighlight;   // �u�n�C�v�I�����ɘg��F��ς���UI
    public GameObject noHighlight;    // �u�C�C�G�v�I�����ɘg��F��ς���UI

    int selectIndex = 0; // 0:�n�C, 1:�C�C�G
    public ShopManager shopManager; // Inspector�ŃA�T�C��

    void OnEnable()
    {
        selectIndex = 0; // ������
        UpdateHighlight();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = 1 - selectIndex; // 0��1�؂�ւ�
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
