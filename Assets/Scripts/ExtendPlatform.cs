using UnityEngine;

public class ExtendPlatform : MonoBehaviour
{
    public Transform player;        // Player�i�C���X�y�N�^�[�ŃA�T�C���j
    public float triggerDistance = 2.5f; // �߂Â����Ɣ��肷�鋗��
    public float extendDistance = 2.0f;  // �ǂꂭ�炢���ɏo����
    public float extendSpeed = 3.0f;     // ����o������

    private Vector3 originalPos;
    private bool isExtended = false;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.position, transform.position);

        if (!isExtended && dist < triggerDistance)
        {
            isExtended = true;
        }

        // ����o���A�j���[�V����
        if (isExtended)
        {
            Vector3 target = originalPos + new Vector3(extendDistance, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, extendSpeed * Time.deltaTime);
        }
    }
}
