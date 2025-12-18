using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageSpawner : MonoBehaviour
{
    [Header("FadeIn")]
    FadeIn fadeIn;
    CanvasGroup cg;
    private float duration = 1.5f;

    [Header("Next_Stage")]
    SpawnPoint[] SpawnPoint;
    ObstacleSpawner obstSpawner;
    EnemySpawn enemySpawn;
    Portal[] portal;
    SpecialLevelUp specialLevelUp;


    private void Start()
    {
        SpawnPoint = FindObjectsOfType<SpawnPoint>();
        fadeIn = FindObjectOfType<FadeIn>(true);
        cg = fadeIn.GetComponent<CanvasGroup>();
        obstSpawner = FindObjectOfType<ObstacleSpawner>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        portal = FindObjectsOfType<Portal>();
        specialLevelUp = FindObjectOfType<SpecialLevelUp>();
        Debug.Log(GameManager.clearStage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int rand = Random.Range(0, SpawnPoint.Length);
            other.transform.position = SpawnPoint[rand].transform.position;
            StartCoroutine(FadeIn());

            if (GameManager.clearStage == 9)
            {
                SpawnEnemy();
                GameManager.InitStageClearCount(); // 다른 위치로 옮겨야함.
            }
            else if (GameManager.clearStage == 4)
            {
                //specialLevelUp.ADSpawn(GameManager.clearStage);
                GameManager.StageIncrease();
            }
            else if(GameManager.clearStage == 5)
            {
                //DeSpawnAngel();
                GameManager.StageIncrease();
            }
            else
            {
                SpawnEnemy();
                DeSpawnObstacle();
                GameManager.StageIncrease();
            }

            Debug.Log(GameManager.clearStage);
        }
    }

    IEnumerator FadeIn()
    {
        foreach (var port in portal) 
        {
            port.ClosePortal();
            if (GameManager.clearStage == 4) port.OpenPortal();
        }
        fadeIn.gameObject.SetActive(true);
        cg.alpha = 1f;
        yield return cg.DOFade(0f, duration)
                   .SetEase(Ease.OutQuad)
                   .SetUpdate(true)
                   .WaitForCompletion();
        cg.alpha = 1f;
        fadeIn.gameObject.SetActive(false);
    }

    private void SpawnEnemy()
    {
        enemySpawn.count = 0;
        enemySpawn.Spawn();
    }
    
    private void DeSpawnObstacle()
    {
        foreach (var obst in FindObjectsOfType<Obstacle>())
        {
            if (obst.isActiveAndEnabled) obst.ReturnPool();
        }
        obstSpawner.alreadySpawned = false;
    }

    private void DeSpawnAngel()
    {
        Angel angel = FindObjectOfType<Angel>();
        if(angel.isActiveAndEnabled) angel.ReturnPool();
    }
}
