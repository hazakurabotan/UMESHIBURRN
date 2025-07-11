using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �^�O����œG�Ȃ�
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(3); // �_���[�W3��^����
            }
            // Destroy(this.gameObject); �� ��������Ȃ���Ίђ�
        }
    }
}
