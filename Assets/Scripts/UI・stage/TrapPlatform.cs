using UnityEngine;

// ----------------------------------------------
// TrapPlatform
// �v���C���[���߂Â��ƈ�莞�ԃg�Q�ispikeObject�j����яo���M�~�b�N
// ----------------------------------------------
public class TrapPlatform : MonoBehaviour
{
    public GameObject spikeObject;      // �\���E��������g�Q�i�q�I�u�W�F�N�g���Ŋ����j
    public float spikeShowTime = 1.0f;  // �g�Q���o�����Ă��鎞��
    public float activateDistance = 1.5f; // �������鋗���i�߂Â����甭���j

    private bool isActive = false;      // �g�Q�����o�Ă��邩
    private float timer = 0f;           // �o������̌o�ߕb��
    private Transform player;           // �v���C���[��Transform�Q��

    void Start()
    {
        if (spikeObject != null)
            spikeObject.SetActive(false); // �Q�[���J�n���͔�\���ɂ���

        // �v���C���[��Transform�擾
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return; // �v���C���[���Ȃ���Ή������Ȃ�

        float dist = Vector2.Distance(player.position, transform.position); // �����v�Z

        // �v���C���[���w�苗�����ɓ�������g�Q���o��
        if (!isActive && dist <= activateDistance)
        {
            ActivateSpike();
        }

        // �g�Q���o�Ă���Ԃ̓^�C�}�[�v���A�w�莞�Ԍo�߂Ŏ��[
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= spikeShowTime)
            {
                DeactivateSpike();
            }
        }
    }

    // --- �g�Q���o�������� ---
    void ActivateSpike()
    {
        if (spikeObject != null)
            spikeObject.SetActive(true); // �\��ON
        isActive = true;
        timer = 0f;                     // �^�C�}�[���Z�b�g
    }

    // --- �g�Q�����[���� ---
    void DeactivateSpike()
    {
        if (spikeObject != null)
            spikeObject.SetActive(false); // �\��OFF
        isActive = false;
        timer = 0f;
    }
}
