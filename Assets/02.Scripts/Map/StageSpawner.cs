using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 현재 또는 다음 스테이지가 몇이냐에따라 동작을 달리해줘야함
// 스테이지 매니저에서 이벤트를 파고 stage 값에 따라 뭔가 상호작용이 필요함. < 이방식으로 변경.
// 천사 : 특정 스테이지 뒷글자가 5인 스테이지마다 등장
// 악마 : 보스전 이후 등장
// 평상시에는 일반적, 장애물 생성
// 뒷자리 5인 스테이지마다 일반적, 장애물 생성 x 천사 생성
// 10스테이지 마다 일반적, 장애물 생성 x 보스전 종료 후 악마 생성

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
    ObstacleSpawner obstacleSpawner;


    private void Start()
    {
        SpawnPoint = FindObjectsOfType<SpawnPoint>();
        fadeIn = FindObjectOfType<FadeIn>(true);
        cg = fadeIn.GetComponent<CanvasGroup>();
        obstSpawner = FindObjectOfType<ObstacleSpawner>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        portal = FindObjectsOfType<Portal>();
        specialLevelUp = FindObjectOfType<SpecialLevelUp>();
        Debug.Log($"현재 스테이지 : {GameManager.Stage.currentStage}");
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
            // 플레이어 위치 다음스테이지로 이동
            int rand = Random.Range(0, SpawnPoint.Length);
            other.transform.position = SpawnPoint[rand].transform.position;
            DeSpawnObstacle();
            
            // 화면 페이드인 코루틴
            StartCoroutine(FadeIn());

            // 스테이지 +1 하는 메서드
            GameManager.Stage.StageIncrease();

            // 해당 스테이지에 적 및 장애물 생성
            NextStage(GameManager.Stage.currentStage);
            #region 천사/악마 소환 이전코드
            //if (GameManager.Stage.currentStage == 10)
            //{
            //    SpawnEnemy(isBossStage);

            //}
            //else if (GameManager.Stage.currentStage == GameManager.Stage.Select("angel"))
            //{
            //    specialLevelUp.ADSpawn(GameManager.Stage.currentStage);
            //    GameManager.Stage.StageIncrease();
            //}
            //else if (GameManager.Stage.currentStage == GameManager.Stage.Select("angel") + 1)
            //{
            //    DeSpawnAngel();
            //    SpawnEnemy(isBossStage);
            //    DeSpawnObstacle();
            //    GameManager.Stage.StageIncrease();
            //}
            //else if (GameManager.Stage.currentStage == GameManager.Stage.Select("devil"))
            //{
            //    SpawnEnemy(isBossStage);
            //    DeSpawnObstacle();
            //    GameManager.Stage.StageIncrease();
            //}
            //else if (GameManager.Stage.currentStage == GameManager.Stage.Select("devil") + 1)
            //{
            //    DeSpawnDevil();
            //    SpawnEnemy(isBossStage);
            //    DeSpawnObstacle();
            //    GameManager.Stage.StageIncrease();
            //}
            //else
            //{
            //    SpawnEnemy(isBossStage);
            //    DeSpawnObstacle();
            //    GameManager.Stage.StageIncrease();
            //}
            #endregion

            Debug.Log($"현재 스테이지 : {GameManager.Stage.currentStage}");
        }
    }

    private void NextStage(int currentstage)
    {
        if (currentstage == 0) return;
        obstSpawner.notthistimeObstacle = false;
        isBossStage = false;
        Debug.Log(currentstage % 10 == 5? $"천사 소환. 현재 스테이지 : {currentstage}" : $"천사 소환 스테이지가 아님. 현재 스테이지 : {currentstage}");
        Debug.Log(currentstage % 10 == 0 && currentstage != 0? $"보스 및 악마 소환. 현재 스테이지 : {currentstage}" : $"보스 및 악마 소환 스테이지가 아님. 현재 스테이지 : {currentstage}");
        if (currentstage % 10 == GameManager.Stage.Select("angel"))
        {
            Debug.Log("천사 소환");
            // 천사가 나올 스테이지 => 적 x , 장애물 x, 포탈 ON
            obstSpawner.notthistimeObstacle = true;
            DeSpawnObstacle();
            specialLevelUp.ADSpawn(GameManager.Stage.currentStage);
            Portal[] portal = FindObjectsOfType<Portal>();
            foreach (var p in portal) p.OpenPortal();
        }
        else if (currentstage % GameManager.Stage.Select("devil") == 0 && currentstage != 0)
        {
            // 악마가 나올 스테이지 => 보스 O, 장애물 x, 포탈 OFF
            Debug.Log("보스 등장");
            obstSpawner.notthistimeObstacle = true;
            isBossStage = true;
            SpawnEnemy(isBossStage);

        }
        else if (currentstage > 20)
        {
            // 한 챕터가 종료됨.
            Debug.Log("한챕터 클리어");
            // 새 챕터가 해금되었다는 씬으로 이동.

            GameManager.Stage.InitStageClearCount(); // 스테이지 초기화
            Debug.Log($"스테이지 초기화 : {GameManager.Stage.currentStage}");
        }
        else
        {
            Debug.Log("일반 적 소환");
            // 악마, 천사를 비활성화 해주는 부분
            SpawnEnemy(isBossStage);
            DeSpawnAngel();
            DeSpawnDevil();
            // 일반 스테이지 => 적 O, 장애물 O, 포탈 OFF

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
        cg.alpha = 1f;
        fadeIn.gameObject.SetActive(false);
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
        if(angel.isActiveAndEnabled) angel.ReturnPool();
    }

    private void DeSpawnDevil()
    {
        Devil devil = FindObjectOfType<Devil>();
        if (devil == null) return;
        if (devil.isActiveAndEnabled) devil.ReturnPool();
    }
}
