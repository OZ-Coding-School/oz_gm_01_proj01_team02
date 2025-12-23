using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public TestEnemy enemyPrefab;  // 드래그 앤 드롭으로 프리팹 연결
    public Transform spawnPoint;

    private TestEnemy spawnedEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (spawnedEnemy != null)
        {
            // 이미 스폰된 적이 있다면 재활용 또는 삭제
            Destroy(spawnedEnemy.gameObject);
        }

        spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        spawnedEnemy.gameObject.name = "TestEnemy";
        Debug.Log("TestEnemy Spawned with HP: " + spawnedEnemy.maxHp);
    }
}