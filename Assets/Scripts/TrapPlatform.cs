using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public GameObject spikeObject; // �o���E����������g�Q�ineedle�I�u�W�F�N�g�Ȃǁj
    public float spikeShowTime = 1.0f; // �g�Q���\�������b��
    public float activateDistance = 1.5f; // �v���C���[���߂Â�����

    private bool isActive = false; // �g�Q���\������
    private float timer = 0f;
    private Transform player;

    void Start()
    {
        if (spikeObject != null)
            spikeObject.SetActive(false); // �ŏ��͔�\��

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.position, transform.position);

        // �v���C���[���߂Â�����g�Q�o�����^�C�}�[�N��
        if (!isActive && dist <= activateDistance)
        {
            ActivateSpike();
        }

        // �\�����̓J�E���g
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= spikeShowTime)
            {
                DeactivateSpike();
            }
        }
    }

    void ActivateSpike()
    {
        if (spikeObject != null)
            spikeObject.SetActive(true);
        isActive = true;
        timer = 0f;
    }

    void DeactivateSpike()
    {
        if (spikeObject != null)
            spikeObject.SetActive(false);
        isActive = false;
        timer = 0f;
    }
}
