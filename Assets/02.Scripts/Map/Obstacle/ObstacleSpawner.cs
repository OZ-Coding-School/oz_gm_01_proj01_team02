using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. 스폰 포인트와 플레이어 간의 거리를 계산하여 distanceToPlayer 보다 짧으면 그 위치에 장애물 스폰
//2. 스폰된 장애물은 EndPoint 객체의 StageSpawner 스크립트를 통해 플레이어가 해당 객체를 통과시 모두 return pool 처리
//3. 플레이어가 다른 스테이지로 이동하면 그 때 다시 isSpawned 여부를 판단하여 해당 위치에 장애물 스폰

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Spawn Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private List<Obstacle> obstaclePrefabs;
    private float distanceToPlayer = 15f;
    bool canSpawn;
    Transform spawnpoint;
    public bool alreadySpawned;
    ObstacleSpawnPoint[] obstacleSpawnPoints;
    Portal[] portal;
    private void Start()
    {
        obstacleSpawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
        var gmInit = GameManager.Pool.transform;
        var parent = new GameObject("Obstacle_Pool").transform;
        parent.SetParent(gmInit, false);
        portal = FindObjectsOfType<Portal>();

        ShuffleList();
        foreach (var prefab in obstaclePrefabs)
        {
            GameManager.Pool.CreatePool(prefab, 1, parent);
        }
    }

    private void Update()
    {
        foreach(var point in obstacleSpawnPoints) //find -> update에서 사용 금지
        {
            var current = point.transform;
            if(spawnpoint==null) { spawnpoint = current; continue; }
            else
            {
                if(Vector3.Distance(current.position, player.transform.position) < Vector3.Distance(spawnpoint.position, player.transform.position))
                {
                    spawnpoint = current;
                }
            }
        }
        canSpawn = Vector3.Distance(spawnpoint.position, player.transform.position) < distanceToPlayer ? true : false;
        
        if (canSpawn && !alreadySpawned)
        {
            int select = Random.Range(0, obstaclePrefabs.Count);
            Obstacle obstacle = GameManager.Pool.GetFromPool(obstaclePrefabs[select]);
            obstacle.transform.SetPositionAndRotation(spawnpoint.position - new Vector3(0,2.5f,0), Quaternion.Euler(0,90f,0));
            alreadySpawned= true;
        }
    }

    private void ShuffleList()
    {
        System.Random rand = new System.Random();
        for (int i = obstaclePrefabs.Count-1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            var temp = obstaclePrefabs[i];
            obstaclePrefabs[i] = obstaclePrefabs[k];
            obstaclePrefabs[k] = temp;
        }
    }
    public Portal GetClosePortal()
    {
        if (!canSpawn) return null;
        Portal closestPortal = null;
        float minDist = float.MaxValue;
        // 스포너와 가장가까운 문만 활성화 시켜주기
        foreach (var p in portal)
        {
            if(p ==null) continue;
            float currentDist = Vector3.Distance(spawnpoint.position, p.transform.position);

            if (currentDist < minDist) 
            {
                minDist = currentDist;
                closestPortal = p;
            }
        }
        return closestPortal;
    }
}
