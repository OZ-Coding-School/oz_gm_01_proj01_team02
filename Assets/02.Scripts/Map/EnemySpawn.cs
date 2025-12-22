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
    //[SerializeField] private List<Enemy> enemyPrefab;
    [SerializeField] private List<EnemyController> enemyPrefab;
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
    private const int PREFAB_COUNT = 7;

    private void Start()
    {
        count = 0;
        obstacleSpawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
        var gmInit = GameManager.Pool.transform;
        var parent = gmInit.Find("Enemy_Pool");
        if (parent == null)
        {
            parent = new GameObject("Enemy_Pool").transform;
            parent.SetParent(gmInit, false);
        }
        foreach (var prefab in enemyPrefab)
        {
            GameManager.Pool.CreatePool(prefab, PREFAB_COUNT, parent);
        }
        GameManager.Pool.CreatePool(bossPrefab, 1, parent);
        
        StartCoroutine(DelaySpawn(false));
    }

    public void Spawn(bool boss)
    {
        StartCoroutine(DelaySpawn(boss));
    }

    private Vector3 DetectStandardPoint()
    {
        foreach (var point in obstacleSpawnPoints)
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

        canSpawn = (longSpawnPoint.Length != 0 && shortSpawnPoint.Length != 0 && randomSpawnPoint.Length != 0 && pos != null) || boss ? true : false;
        
        if (canSpawn)
        {
            if (!boss)
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
                GetEnemy(pos, EnemyType.Boss, ref count, 1);
            }
        }

    }

    private void GetEnemy(Vector3 pos, EnemyType type, ref int count, int max)
    {
        if (count >= max) return;
        EnemyController enemy = null;
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
        if (enemy == null && boss == null) return;
        if (enemy != null && boss == null) EnemyInit(type, pos, enemy, null);
        if (boss != null && enemy == null) EnemyInit(type, pos, null, boss);

        count++;
    }

    IEnumerator DelaySpawn(bool boss)
    {
        yield return new WaitForSeconds(0.1f);
        GoEnemySpawn(DetectStandardPoint(), boss);
    }

    private void EnemyInit(EnemyType type, Vector3 pos, EnemyController enemy = null, Boss boss = null)
    {
        NavMeshAgent agent = null;
        EnemyCheck check = null;
        Transform target = null;
        GameObject targetObj = null;

        if (type == EnemyType.Boss)
        {
            agent = boss.GetComponent<NavMeshAgent>();
            check = boss.GetComponent<EnemyCheck>();
            target = boss.transform;
            targetObj = boss.gameObject;
        }
        else
        {
            agent = enemy.GetComponent<NavMeshAgent>();
            check = enemy.GetComponent<EnemyCheck>();
            target = enemy.transform;
            targetObj = enemy.gameObject;
        }

        if(agent != null)
        {
            if(agent.enabled) agent.enabled = false;

            NavMeshHit hit;

            
            if(NavMesh.SamplePosition(pos, out hit, 3f, NavMesh.AllAreas))
            {
                target.position = hit.position;
                target.rotation = Quaternion.identity;
            }
            else
            {
                targetObj.SetActive(false);
                return;
            }
            agent.enabled = true;
        }
        if(check != null) check.SetReady();

    }

}
