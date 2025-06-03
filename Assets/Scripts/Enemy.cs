using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public bool isGrabbed = false;
    public bool isFlying = false;

    private bool recentlyHit = false;  // ���ǉ�

    public void TakeDamage(int amount)
    {
        // �A���q�b�g�h�~
        if (recentlyHit) return;           // ���łɃq�b�g���Ȃ疳��
        recentlyHit = true;                // ����ȏ�̃q�b�g��h�~
        Invoke(nameof(ResetHit), 0.05f);   // 0.05�b��ɉ����i�K�X�����j

        // �͂܂�Ă�Œ��������G
        if (isGrabbed && !isFlying) return;

        hp -= amount;
        Debug.Log("Enemy�� " + amount + " �_���[�W�I ����HP: " + hp);


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
