using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private List<EnemyController> meleeEnemyPrefab;
    [SerializeField] private List<EnemyController> rangedEnemyPrefab;
    [SerializeField] private List<EnemyController> bossPrefab;
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
        foreach (var prefab in meleeEnemyPrefab)
        {
            GameManager.Pool.CreatePool(prefab, PREFAB_COUNT, parent);
        }
        foreach (var prefab in rangedEnemyPrefab)
        {
            GameManager.Pool.CreatePool(prefab, PREFAB_COUNT, parent);
        }
        foreach(var prefab in bossPrefab)
        {
            GameManager.Pool.CreatePool(prefab, 1, parent);
        }
        
        StartCoroutine(DelaySpawn(false));
    }

    public void Spawn(bool boss)
    {
        Debug.Log($"Spawn()의 boss값 : {boss}");
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
        Debug.Log($"Spawn()의 boss값 : {boss}");
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
        EnemyController boss = null;

        switch (type)
        {
            case EnemyType.Short:
                int randMelee = Random.Range(0, meleeEnemyPrefab.Count);
                enemy = GameManager.Pool.GetFromPool(meleeEnemyPrefab[randMelee]);
                break;
            case EnemyType.Long:
                int randRange = Random.Range(0, rangedEnemyPrefab.Count);
                enemy = GameManager.Pool.GetFromPool(rangedEnemyPrefab[randRange]);
                break;
            case EnemyType.Random:
                int randMon1 = Random.Range(0, meleeEnemyPrefab.Count);
                int randMon2 = Random.Range(0, rangedEnemyPrefab.Count);
                enemy = Random.value > 0.5f ? GameManager.Pool.GetFromPool(meleeEnemyPrefab[randMon1]) : GameManager.Pool.GetFromPool(rangedEnemyPrefab[randMon2]);
                break;
            case EnemyType.Boss:
                Debug.Log("보스소환");
                int currentChapter = GameManager.Data.playData._chapter;
                boss = GameManager.Pool.GetFromPool(bossPrefab[currentChapter - 1]);
                break;
        }
        if (enemy == null && boss == null) return;
        if (enemy != null && boss == null) EnemyInit(type, pos, enemy, null);
        if (boss != null && enemy == null) EnemyInit(type, pos, null, boss);

        count++;
        Debug.Log(count);
    }

    IEnumerator DelaySpawn(bool boss)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log($"Spawn()의 boss값 : {boss}");
        GoEnemySpawn(DetectStandardPoint(), boss);
    }

    private void EnemyInit(EnemyType type, Vector3 pos, EnemyController enemy = null, EnemyController boss = null)
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
