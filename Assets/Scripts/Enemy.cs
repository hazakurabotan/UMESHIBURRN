using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public bool isGrabbed = false;
    public bool isFlying = false;

    private bool recentlyHit = false;  // š’Ç‰Á

    public void TakeDamage(int amount)
    {
        // ˜A‘±ƒqƒbƒg–h~
        if (recentlyHit) return;           // ‚·‚Å‚Éƒqƒbƒg’†‚È‚ç–³‹
        recentlyHit = true;                // ‚±‚êˆÈã‚Ìƒqƒbƒg‚ğ–h~
        Invoke(nameof(ResetHit), 0.05f);   // 0.05•bŒã‚É‰ğœi“K‹X’²®j

        // ’Í‚Ü‚ê‚Ä‚éÅ’†‚¾‚¯–³“G
        if (isGrabbed && !isFlying) return;

        hp -= amount;
        Debug.Log("Enemy‚É " + amount + " ƒ_ƒ[ƒWI Œ»İHP: " + hp);


        if (hp <= 0)
        {
            Die();
        }
    }
    private void ResetHit()
    {
        recentlyHit = false;
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}
