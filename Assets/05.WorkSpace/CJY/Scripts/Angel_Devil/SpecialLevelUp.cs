using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialLevelUp : MonoBehaviour
{
    [Header("angel_devil_spawn setting")]
    [SerializeField] Angel angelPrefab;
    [SerializeField] Devil devilPrefab;
    [SerializeField] private GameObject player;
    private float distanceToPlayer = 15f;
    Transform spawnpoint;
    ObstacleSpawnPoint[] spawnPoints;

    private void OnEnable()
    {
        EnemyCheck.OnEnemyReturnPool += DevilSpawn;
    }

    private void OnDisable()
    {
        EnemyCheck.OnEnemyReturnPool -= DevilSpawn;
    }

    private void Start()
    {
        spawnPoints = FindObjectsOfType<ObstacleSpawnPoint>();
        var gmInit = GameManager.Pool.transform;
        var parent = new GameObject("Angel_Devil_Pool").transform;
        parent.SetParent(gmInit, false);

        GameManager.Pool.CreatePool(angelPrefab, 1, parent);
        GameManager.Pool.CreatePool(devilPrefab, 1, parent);

        Debug.Log(spawnPoints is not null);
    }


    public void ADSpawn(int nowStage)
    {
        foreach (var point in spawnPoints) //find -> update에서 사용 금지
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

        if (nowStage == 4)
        {
            Debug.Log("천사 소환");
            StartCoroutine(DelayAngelSpawn());

        }
        if (nowStage == 2) 
        {
            Debug.Log("악마 소환");
            //Devil devil = GameManager.Pool.GetFromPool(devilPrefab);
            //Debug.Log(devil is not null);
            //devil.transform.SetPositionAndRotation(spawnpoint.position + (Vector3.up * 1.5f), Quaternion.Euler(0, 180, 0));
        }

    }

    private void DevilSpawn(GameObject enemy)
    {
        if (GameManager.clearStage != 10) return;
        ADSpawn(GameManager.clearStage);
    }

    IEnumerator DelayAngelSpawn()
    {
        yield return new WaitForSeconds(0.3f);

        Angel angel = GameManager.Pool.GetFromPool(angelPrefab);
        angel.transform.SetPositionAndRotation(spawnpoint.position + (Vector3.up * 1.5f) , Quaternion.Euler(0,180,0));
    }

}
