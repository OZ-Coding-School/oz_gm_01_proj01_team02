using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Short,
    Long,
    Random
}

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemy Spawn Setting")]
    [SerializeField] private List<Enemy> _enemyPrefab;
    [SerializeField] private List<EnemyController> enemyPrefab;
    [SerializeField] LayerMask[] layerMasks;

    [Header("Enemy Spawn Point Explore Setting")]
    [SerializeField] private GameObject player;
    private float distanceToStandard = 15f;
    ObstacleSpawnPoint[] obstacleSpawnPoints;
    Transform exploredPoint;

    [Header("Spawning Setting")]
    bool canSpawn;
    public int count;
    private const int PREFAB_COUNT = 7;


    private void Start()
    {
        count = 0;
        obstacleSpawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
        var gmInit = GameManager.Pool.transform;
        var parent = gmInit.Find("Enemy_Pool");
        if(parent == null) 
        { 
            parent = new GameObject("Enemy_Pool").transform;
            parent.SetParent(gmInit, false);
        }
        foreach (var prefab in enemyPrefab)
        {
            GameManager.Pool.CreatePool(prefab, PREFAB_COUNT, parent);
        }

        //스폰포인트와 가까이 있는(15f 이내) 모든 EnemySpawnPoint를 
        StartCoroutine(DelaySpawn());
            
    }

    public void Spawn()
    {
        StartCoroutine(DelaySpawn());
    }
    private Vector3 DetectStandardPoint()
    {
        foreach (var point in obstacleSpawnPoints) //find -> update에서 사용 금지
        {
            var current = point.transform;
            if (exploredPoint == null) { exploredPoint = current; continue; }
            else
            {
                if (Vector3.Distance(current.position, player.transform.position) < Vector3.Distance(exploredPoint.position, player.transform.position))
                {
                    exploredPoint = current;
                }
            }
        }
        return exploredPoint.transform.position;
    }

    private void GoEnemySpawn(Vector3 pos)
    {
        Collider[] longSpawnPoint = Physics.OverlapSphere(pos, distanceToStandard, layerMasks[0]);
        Collider[] shortSpawnPoint = Physics.OverlapSphere(pos, distanceToStandard, layerMasks[1]);
        Collider[] randomSpawnPoint = Physics.OverlapSphere(pos, distanceToStandard, layerMasks[2]);
        int maxSize = longSpawnPoint.Length + shortSpawnPoint.Length + randomSpawnPoint.Length;
        
        canSpawn = longSpawnPoint.Length != 0 && shortSpawnPoint.Length != 0 && randomSpawnPoint.Length != 0 ? true: false;
        
        if (canSpawn)
        {
            foreach (var point in longSpawnPoint)
            {
                GetEnemy(point.transform.position, EnemyType.Long, ref count, maxSize);
            }
            foreach (var point in shortSpawnPoint)
            {
                GetEnemy(point.transform.position, EnemyType.Short, ref count, maxSize);
            }
            foreach (var point in randomSpawnPoint)
            {
                GetEnemy(point.transform.position, EnemyType.Random, ref count, maxSize);
            }
        }
    }

    private void GetEnemy(Vector3 pos, EnemyType type, ref int count, int max)
    {
        if (count >= max) return;
        EnemyController enemy = null;

        switch (type)
        {
            case EnemyType.Short:
                enemy = GameManager.Pool.GetFromPool(enemyPrefab[0]);
                break;
            case EnemyType.Long:
                enemy = GameManager.Pool.GetFromPool(enemyPrefab[1]);
                break;
            case EnemyType.Random:
                enemy = Random.value > 0.5f ? GameManager.Pool.GetFromPool(enemyPrefab[0]) : GameManager.Pool.GetFromPool(enemyPrefab[1]);
                break;
        }
        if (enemy == null) return;

        NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
        enemyAgent.enabled = false;
        Vector3 newPos = pos + new Vector3(0, 1.5f, 0);
        enemy.transform.SetPositionAndRotation(newPos, Quaternion.identity);
        enemyAgent.enabled = true;

        if (!enemyAgent.isOnNavMesh) // navmesh 위가 아니면 재배치
        {
            NavMeshHit hit;
            if(NavMesh.SamplePosition(newPos, out hit, 2f, NavMesh.AllAreas))
            {
                enemyAgent.Warp(hit.position);
            }
        }
        EnemyCheck enemyCheck = enemy.GetComponent<EnemyCheck>();
        if (enemyCheck != null) enemyCheck.SetReady();
        count++;

    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(0.1f);
        GoEnemySpawn(DetectStandardPoint());
    }

}
