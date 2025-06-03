using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public bool isGrabbed = false;
    public bool isFlying = false;

    public void TakeDamage(int amount)
    {
        // ’Í‚Ü‚ê‚Ä‚éÅ’†‚¾‚¯–³“G
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
