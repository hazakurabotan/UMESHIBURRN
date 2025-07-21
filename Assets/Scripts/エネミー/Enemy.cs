using UnityEngine;

// �G�L�����N�^�[�p�̊�{�X�N���v�g
public class Enemy : MonoBehaviour
{
    public int hp = 3;               // �G�̗̑́iHP�j
    public int maxHp = 3;
    public GameObject[] dropItemPrefabs;
    public bool isGrabbed = false;   // �͂܂�Ă����Ԃ��i���݃A�N�V�����p�t���O�j
    public bool isFlying = false;    // ���ł����Ԃ��i������ꒆ�Ȃǁj
    public EnemyHpBarController hpBar;
    private bool recentlyHit = false;  // �A���q�b�g�h�~�p�t���O


    void Start()
    {
        // �J�n���ɍő�HP���Z�b�g
        if (hpBar != null)
            hpBar.SetHp(hp, maxHp);
    }


    // ======= �_���[�W���� =======
    public void TakeDamage(int amount, string cause = "other")
    {
        // �A���q�b�g�h�~�i�U���̓����蔻�����u�����������j
        if (recentlyHit) return;               // ���łɃq�b�g���蒆�Ȃ牽�����Ȃ�
        recentlyHit = true;                    // �q�b�g�t���OON
        Invoke(nameof(ResetHit), 0.05f);       // 0.05�b��Ƀt���O�����i�����\�j

        // �͂܂�Ă���Œ��͖��G�i�������u���ł���v�ꍇ�̓_���[�W�L���j
        if (isGrabbed && !isFlying) return;

        // HP�����炷
        hp -= amount;
        if (hpBar != null)
            hpBar.SetHp(hp, maxHp);

        // HP��0�ȉ��ɂȂ����玀�S����
        if (hp <= 0)
        {
            Die(cause);
        }
    }




    // ======= �A���q�b�g�����p�֐� =======
    private void ResetHit()
    {
        recentlyHit = false; // �ĂэU�����󂯕t����
    }

    // ======= ���S���� =======
    private void Die(string cause)
    {
        // 1. �h���b�v
        if (dropItemPrefabs != null && dropItemPrefabs.Length > 0)
        {
            if (Random.value < 0.5f)
            {
                int itemType = Random.Range(0, dropItemPrefabs.Length);
                Instantiate(dropItemPrefabs[itemType], transform.position, Quaternion.identity);
            }
        }

        // 2. �L���J�E���g
        if (cause == "gun" && GameManager.Instance != null)
            GameManager.Instance.AddKill();
        // summon�⑼�̎�i�ł̓J�E���g���Ȃ�

        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(2, "gun");
            Destroy(other.gameObject);
        }
        // ...�ق��̏���
    }


}
