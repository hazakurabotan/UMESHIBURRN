using UnityEngine;

public class CrateBombSimple : MonoBehaviour
{
    public GameObject explosionSpriteObj;
    public float delay = 3f;

    private bool isCounting = false;
    private float timer = 0f;

    void Update()
    {
        if (isCounting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (explosionSpriteObj != null)
                {
                    explosionSpriteObj.SetActive(true);
                    explosionSpriteObj.transform.position = transform.position; // ���̈ʒu�ɔ������o��
                }
                Destroy(gameObject); // �����̂͏���
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCounting && other.CompareTag("Player"))
        {
            isCounting = true;
            timer = delay;
        }
    }
}
