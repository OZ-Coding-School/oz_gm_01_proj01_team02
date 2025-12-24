using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    bool isBossStage;



    private void Start()
    {
        SpawnPoint = FindObjectsOfType<SpawnPoint>();
        fadeIn = FindObjectOfType<FadeIn>(true);
        cg = fadeIn.GetComponent<CanvasGroup>();
        obstSpawner = FindObjectOfType<ObstacleSpawner>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        portal = FindObjectsOfType<Portal>();
        specialLevelUp = FindObjectOfType<SpecialLevelUp>();
    }

    private void OnEnable()
    {
        StageManager.OnStageIncrease += NextStage;
    }

    private void OnDisable()
    {
        StageManager.OnStageIncrease -= NextStage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int rand = Random.Range(0, SpawnPoint.Length);
            other.transform.position = SpawnPoint[rand].transform.position;
            DeSpawnObstacle();

            StartCoroutine(FadeIn());

            GameManager.Stage.StageIncrease();

            NextStage(GameManager.Stage.currentStage);

        }
    }

    private void NextStage(int currentstage)
    {
        if (currentstage == 0) return;
        isBossStage = false;
        if (currentstage % GameManager.Stage.Select("stage") == GameManager.Stage.Select("angel"))
        {
            DeSpawnObstacle();
            specialLevelUp.ADSpawn(GameManager.Stage.currentStage);
            Portal[] portal = FindObjectsOfType<Portal>();
            foreach (var p in portal) p.OpenPortal();
        }
        else if (currentstage % GameManager.Stage.Select("devil") == 0 && currentstage != 0)
        {
            isBossStage = true;
            SpawnEnemy(isBossStage);

        }
        else
        {
            SpawnEnemy(isBossStage);
            DeSpawnAngel();
            DeSpawnDevil();
        }
    }


    IEnumerator FadeIn()
    {
        foreach (var port in portal)
        {
            port.ClosePortal();
        }
        fadeIn.gameObject.SetActive(true);
        cg.alpha = 1f;
        yield return cg.DOFade(0f, duration)
                   .SetEase(Ease.OutQuad)
                   .SetUpdate(true)
                   .WaitForCompletion();
        fadeIn.gameObject.SetActive(false);
        cg.alpha = 1f;
    }

    private void SpawnEnemy(bool boss)
    {
        enemySpawn.count = 0;
        enemySpawn.Spawn(boss);
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
        if (angel == null) return;
        if (angel.isActiveAndEnabled) angel.ReturnPool();
    }

    private void DeSpawnDevil()
    {
        Devil devil = FindObjectOfType<Devil>();
        if (devil == null) return;
        if (devil.isActiveAndEnabled) devil.ReturnPool();
    }
}
