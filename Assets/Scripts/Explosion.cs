using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    void OnEnable()
    {
        // ������\�������^�C�~���O����1�b��Ɏ����Ŕ�\��
        Invoke(nameof(HideSelf), 1f);
    }

    void HideSelf()
    {
        gameObject.SetActive(false);
    }
}
