using UnityEngine;

// ---------------------------------------------------------
// EnemySpawner
// �G�L�����N�^�[���o�������A���ꂽ���莞�Ԍ�ɍďo��������X�N���v�g
// ---------------------------------------------------------
public class EnemySpawner : MonoBehaviour
{
    // �o��������G�L�����N�^�[�i�v���n�u�j�̎Q��
    public GameObject enemyPrefab;

    // �G���o������ꏊ�iTransform��Inspector�Ŏw��j
    public Transform spawnPoint;

    // �G���|����Ă���Ăяo������܂ł̑҂����ԁi�b�j
    public float respawnDelay = 2.0f;

    // ���ݏo�����Ă���G�̎Q�Ɓi���������ǂ������`�F�b�N����p�j
    private GameObject currentEnemy;

    // �����X�|�[���҂����ǂ����i�d�����X�|�[����h�����߂̃t���O�j
    private bool respawning = false;

    // �Q�[���J�n���Ɉ�x�����Ă΂��
    void Start()
    {
        SpawnEnemy(); // �ŏ��̓G���o��������
    }

    // ���t���[���Ă΂��
    void Update()
    {
        // currentEnemy��null�i�G�����Ȃ��j�ŁA�����X�|�[�����łȂ���΁c
        if (currentEnemy == null && !respawning)
        {
            // �҂����Ԃ̌�œG���o��������\��i�R���[�`���j
            StartCoroutine(RespawnAfterDelay());
        }
    }

    // �G���o��������֐�
    void SpawnEnemy()
    {
        if (spawnPoint != null)
        {
            // enemyPrefab��spawnPoint�̈ʒu�ɐ������A�Q�Ƃ�ۑ�
            currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            // �X�|�[���ꏊ���w�肳��Ă��Ȃ��Ƃ��x�����o��
            Debug.LogWarning("�X�|�[���|�C���g���ݒ肳��Ă��܂���I");
        }
    }

    // ��莞�ԑ҂��Ă���G���o��������R���[�`��
    System.Collections.IEnumerator RespawnAfterDelay()
    {
        respawning = true; // ���X�|�[�����t���O�𗧂Ă�
        yield return new WaitForSeconds(respawnDelay); // �w��b���҂�
        SpawnEnemy(); // �G���o��������
        respawning = false; // �t���O��߂�
    }
}
