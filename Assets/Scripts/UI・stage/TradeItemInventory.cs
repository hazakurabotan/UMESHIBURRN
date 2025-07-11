// �ǂ̃A�C�e�����������Ă邩�ۑ���UI�֔��f����N���X
using UnityEngine;
using UnityEngine.UI;

public class TradeItemInventory : MonoBehaviour
{
    // 3��ނ̃A�C�e������z��ŊǗ�
    public int[] itemCounts = new int[3];

    // �p�l����̌��\��UI�iUnity�̃C���X�y�N�^�[��3�����j
    public Text[] itemCountTexts;

    // �A�C�e���𑝂₷�֐��iitemType��0�`2�j
    public void AddItem(int itemType)
    {
        itemCounts[itemType]++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < itemCountTexts.Length; i++)
        {
            itemCountTexts[i].text = itemCounts[i].ToString();
        }
    }
}
