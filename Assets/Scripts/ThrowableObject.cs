using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public int damage = 1;
    private bool isThrown = false;

    public void ActivateAsProjectile()
    {
        isThrown = true;
        Invoke("Deactivate", 1.0f);
    }

    void Deactivate()
    {
        isThrown = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isThrown) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
            isThrown = false;
        }
    }
}
