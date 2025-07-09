using UnityEngine;

public class TradeItem : MonoBehaviour
{
    public int itemType; // 0,1,2�i�C���X�y�N�^�[�Őݒ� or �������ɃZ�b�g�j

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �C���x���g����T���ăJ�E���gUP
            TradeItemInventory inventory = FindObjectOfType<TradeItemInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemType);
            }

            // �A�C�e�����̂͏���
            Destroy(gameObject);
        }
    }
}
