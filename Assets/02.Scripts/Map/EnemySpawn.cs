using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public enum EnemyType
{
    Short,
    Long,
    Random,
    Boss
}

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemy Spawn Setting")]
    [SerializeField] private List<Enemy> enemyPrefab;
    [SerializeField] private Boss bossPrefab;
    [SerializeField] LayerMask[] layerMasks;

    [Header("Enemy Spawn Point Explore Setting")]
    [SerializeField] private GameObject player;
    private float distanceToStandard = 15f;
    ObstacleSpawnPoint[] obstacleSpawnPoints;
    Transform exploredPoint;

    [Header("Spawning Setting")]
    bool canSpawn;
    public int count;


    private void Start()
    {
        count = 0;
        obstacleSpawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
        var gmInit = GameManager.Pool.transform;
        var parent = new GameObject("Enemy_Pool").transform;
        parent.SetParent(gmInit, false);

        foreach (var prefab in enemyPrefab)
        {
            GameManager.Pool.CreatePool(prefab, 7, parent);
        }
        GameManager.Pool.CreatePool(bossPrefab, 1, parent);
        //스폰포인트와 가까이 있는(15f 이내) 모든 EnemySpawnPoint를 
        StartCoroutine(DelaySpawn(false));
    }

    public void Spawn(bool boss)
    {
        StartCoroutine(DelaySpawn(boss));
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

    private void GoEnemySpawn(Vector3 pos, bool boss)
    {
        Collider[] longSpawnPoint = Physics.OverlapSphere(pos, distanceToStandard, layerMasks[0]);
        Collider[] shortSpawnPoint = Physics.OverlapSphere(pos, distanceToStandard, layerMasks[1]);
        Collider[] randomSpawnPoint = Physics.OverlapSphere(pos, distanceToStandard, layerMasks[2]);
        int maxSize = longSpawnPoint.Length + shortSpawnPoint.Length + randomSpawnPoint.Length;
        
        canSpawn = longSpawnPoint.Length != 0 && shortSpawnPoint.Length != 0 && randomSpawnPoint.Length != 0 && pos != null? true: false;
        
        if (canSpawn)
        {
            if(!boss)
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
            else
            {
                GetEnemy(pos, EnemyType.Boss, ref count, maxSize);
            }
        }
        
    }

    private void GetEnemy(Vector3 pos, EnemyType type, ref int count, int max)
    {
        if (count >= max) return;
        Enemy enemy = null;
        Boss boss = null;

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
            case EnemyType.Boss:
                boss = GameManager.Pool.GetFromPool(bossPrefab);
                break;
        }
        if(enemy ==null && boss == null) return;
        if(enemy != null && boss == null) EnemyInit(type, pos, enemy, null);
        if(boss != null && enemy == null) EnemyInit(type, pos, null, boss);

        count++;
    }

    IEnumerator DelaySpawn(bool boss)
    {
        yield return new WaitForSeconds(0.1f);
        GoEnemySpawn(DetectStandardPoint(), boss);
    }

    private void EnemyInit(EnemyType type, Vector3 pos, Enemy enemy = null, Boss boss = null)
    {
        if (type == EnemyType.Boss) 
        {
            NavMeshAgent bossAgent = boss.GetComponent<NavMeshAgent>();
            bossAgent.enabled = false;
            Vector3 newPos = pos + new Vector3(0, 1.5f, 0);
            boss.transform.SetPositionAndRotation(newPos, Quaternion.identity);
            bossAgent.enabled = true;

            if (!bossAgent.isOnNavMesh) // navmesh 위가 아니면 재배치
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newPos, out hit, 2f, NavMesh.AllAreas))
                {
                    bossAgent.Warp(hit.position);
                }
            }
            EnemyCheck enemyCheck = boss.GetComponent<EnemyCheck>();
            if (enemyCheck != null) enemyCheck.SetReady();
        }
        else 
        {
            NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
            enemyAgent.enabled = false;
            Vector3 newPos = pos + new Vector3(0, 1.5f, 0);
            enemy.transform.SetPositionAndRotation(newPos, Quaternion.identity);
            enemyAgent.enabled = true;

            if (!enemyAgent.isOnNavMesh) // navmesh 위가 아니면 재배치
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newPos, out hit, 2f, NavMesh.AllAreas))
                {
                    enemyAgent.Warp(hit.position);
                }
            }
            EnemyCheck enemyCheck = enemy.GetComponent<EnemyCheck>();
            if (enemyCheck != null) enemyCheck.SetReady();
        }
    }
}
