using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float respawnDelay = 2.0f;   // �ďo���܂ł̕b��

    private GameObject currentEnemy;
    private bool respawning = false;

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        // �����G�������Ă��āA���X�|�[��������Ȃ���΁A���X�|�[���\��
        if (currentEnemy == null && !respawning)
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoint != null)
        {
            currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("�X�|�[���|�C���g���ݒ肳��Ă��܂���I");
        }
    }

    System.Collections.IEnumerator RespawnAfterDelay()
    {
        respawning = true;
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
        respawning = false;
    }
}
