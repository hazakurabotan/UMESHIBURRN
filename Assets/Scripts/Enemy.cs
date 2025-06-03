using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public bool isGrabbed = false;
    public bool isFlying = false;

    public void TakeDamage(int amount)
    {
        // �͂܂�Ă�Œ��������G
        if (isGrabbed && !isFlying) return;

        hp -= amount;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
