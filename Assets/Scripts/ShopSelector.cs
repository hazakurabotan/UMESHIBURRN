using UnityEngine;
using UnityEngine.UI;

public class ShopSelector : MonoBehaviour
{
    public Image[] items;        // ���i�摜 (UI�̏ꍇ)
    public Sprite selectFrame;   // �n�C���C�g�摜

    int selectIndex = 0;
    public bool selecting = false; // ��������public�ɁI�iShopManager���瑀��ł���j

    public void StartSelect()
    {
        selecting = true;
        UpdateHighlight();
    }

    void Update()
    {
        // �I�����[�h����Ȃ��Ȃ疳����
        if (!selecting) return;

        // ���m�F�p�l�����A�N�e�B�u�ȏꍇ�͉������삵�Ȃ�
        if (GameObject.Find("ConfirmPanel") != null && GameObject.Find("ConfirmPanel").activeSelf)
            return;

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
            // �m�F�_�C�A���O�\��
            FindObjectOfType<ShopManager>().ShowConfirm(selectIndex);
            selecting = false; // �����őI�𖳌���
        }
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < items.Length; i++)
            items[i].transform.localScale = (i == selectIndex) ? Vector3.one * 1.2f : Vector3.one;
    }
}
