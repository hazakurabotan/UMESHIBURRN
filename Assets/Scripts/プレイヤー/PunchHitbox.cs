using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public int punchDamage = 2; // �p���`�̈З�

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(punchDamage);
                // 1���ŉ����������Ȃ��悤�ɂ������Ȃ�A�����Ŗ�������
                // gameObject.SetActive(false); // or Collider�𖳌���
            }
        }
    }
}
