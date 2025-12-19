using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. ���� ����Ʈ�� �÷��̾� ���� �Ÿ��� ����Ͽ� distanceToPlayer ���� ª���� �� ��ġ�� ��ֹ� ����
//2. ������ ��ֹ��� EndPoint ��ü�� StageSpawner ��ũ��Ʈ�� ���� �÷��̾ �ش� ��ü�� ����� ��� return pool ó��
//3. �÷��̾ �ٸ� ���������� �̵��ϸ� �� �� �ٽ� isSpawned ���θ� �Ǵ��Ͽ� �ش� ��ġ�� ��ֹ� ����

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

    public bool notthistimeObstacle;
    private void Start()
    {
        obstacleSpawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
        var gmInit = GameManager.Pool.transform;

        var parent = gmInit.Find("Obstacle_Pool");
        if (parent == null)
        {
            parent = new GameObject("Obstacle_Pool").transform;
            parent.SetParent(gmInit, false);
        }

        portal = FindObjectsOfType<Portal>();

        ShuffleList();

        if (parent.childCount == obstaclePrefabs.Count) return;

        foreach (var prefab in obstaclePrefabs)
        {
            GameManager.Pool.CreatePool(prefab, 1, parent);
        }
        notthistimeObstacle = false;
    }

    private void Update()
    {
        foreach (var point in obstacleSpawnPoints) //find -> update���� ��� ����
        {
            var current = point.transform;
            if (spawnpoint == null) { spawnpoint = current; continue; }
            else
            {
                if (Vector3.Distance(current.position, player.transform.position) < Vector3.Distance(spawnpoint.position, player.transform.position))
                {
                    spawnpoint = current;
                }
            }
        }
        canSpawn = Vector3.Distance(spawnpoint.position, player.transform.position) < distanceToPlayer ? true : false;

        //if (canSpawn && !alreadySpawned && !notthistimeObstacle) => 장애물 치우는 코드 (악마 소환시 보스몹도 소환이안되서 지움)
        if (canSpawn && !alreadySpawned)
        {
            int select = Random.Range(0, obstaclePrefabs.Count);
            Obstacle obstacle = GameManager.Pool.GetFromPool(obstaclePrefabs[select]);
            if (obstacle == null) return;
            obstacle.transform.SetPositionAndRotation(spawnpoint.position - new Vector3(0, 2.5f, 0), Quaternion.Euler(0, 90f, 0));
            alreadySpawned = true;
        }
    }

    private void ShuffleList()
    {
        System.Random rand = new System.Random();
        for (int i = obstaclePrefabs.Count - 1; i > 0; i--)
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
        // �����ʿ� ���尡��� ���� Ȱ��ȭ �����ֱ�
        foreach (var p in portal)
        {
            if (p == null) continue;
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
