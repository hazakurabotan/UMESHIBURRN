using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float respawnDelay = 2.0f;   // 再出現までの秒数

    private GameObject currentEnemy;
    private bool respawning = false;

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        // もし敵が消えていて、リスポーン中じゃなければ、リスポーン予約
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
            Debug.LogWarning("スポーンポイントが設定されていません！");
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
