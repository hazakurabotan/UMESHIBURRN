using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public int punchDamage = 2; // ƒpƒ“ƒ`‚ÌˆĞ—Í

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(punchDamage);
                // 1”­‚Å‰½‰ñ‚à“–‚½‚ç‚È‚¢‚æ‚¤‚É‚µ‚½‚¢‚È‚çA‚±‚±‚Å–³Œø‰»‚à
                // gameObject.SetActive(false); // or Collider‚ğ–³Œø‚É
            }
        }
    }
}
