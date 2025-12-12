//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySpawn : MonoBehaviour
//{
//    [Header("Enemy Spawn Setting")]
//    [SerializeField] private List<Enemy> enemyPrefab;
//    private Transform[] longRangeEnemySpawnPoint;
//    private Transform[] shortRangeEnemySpawnPoint;
//    private Transform[] RandomRangeEnemySpawnPoint;
//    [SerializeField] LayerMask[] layerMasks;
//    string[] tagNames = { "LongRange", "ShortRange", "RandomRange" };

//    [Header("Enemy Spawn Point Explore Setting")]
//    [SerializeField] private GameObject player;
//    private float distanceToStandard = 15f;
//    ObstacleSpawnPoint[] obstacleSpawnPoints;
//    Transform exploredPoint;


//    private void Start()
//    {
//        obstacleSpawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
//        var gmInit = GameManager.Pool.transform;
//        var parent = new GameObject("Enemy_Pool").transform;
//        parent.SetParent(gmInit, false);

//        foreach (var prefab in enemyPrefab)
//        {
//            GameManager.Pool.CreatePool(prefab, 7, parent);
//        }

//        var child = GameManager.Pool.GetComponentsInChildren<Enemy>(true);
//        foreach (var prefab in child)
//        {
//            Debug.Log(prefab);
//        }

//    }

//    private void Update()
//    {
//        DetectStandardPoint(); // 플레이어와 가장가까운 스폰포인트
//        //스폰포인트와 가까이 있는(15f 이내) 모든 EnemySpawnPoint를 

//    }

//    private Transform DetectStandardPoint()
//    {
//        foreach (var point in obstacleSpawnPoints) //find -> update에서 사용 금지
//        {
//            var current = point.transform;
//            if (exploredPoint == null) { exploredPoint = current; continue; }
//            else
//            {
//                if (Vector3.Distance(current.position, player.transform.position) < Vector3.Distance(exploredPoint.position, player.transform.position))
//                {
//                    exploredPoint = current;
//                }
//            }
//        }
//        return exploredPoint;
//    }
//}
