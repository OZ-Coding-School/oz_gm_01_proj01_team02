using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� �Ǵ� ���� ���������� ���̳Ŀ����� ������ �޸��������
// �������� �Ŵ������� �̺�Ʈ�� �İ� stage ���� ���� ���� ��ȣ�ۿ��� �ʿ���. < �̹������ ����.
// õ�� : Ư�� �������� �ޱ��ڰ� 5�� ������������ ����
// �Ǹ� : ������ ���� ����
// ���ÿ��� �Ϲ���, ��ֹ� ����
// ���ڸ� 5�� ������������ �Ϲ���, ��ֹ� ���� x õ�� ����
// 10�������� ���� �Ϲ���, ��ֹ� ���� x ������ ���� �� �Ǹ� ����

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
        Debug.Log($"���� �������� : {GameManager.Stage.currentStage}");
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
            // �÷��̾� ��ġ �������������� �̵�
            int rand = Random.Range(0, SpawnPoint.Length);
            other.transform.position = SpawnPoint[rand].transform.position;
            DeSpawnObstacle();

            // ȭ�� ���̵��� �ڷ�ƾ
            StartCoroutine(FadeIn());

            // �������� +1 �ϴ� �޼���
            GameManager.Stage.StageIncrease();

            // �ش� ���������� �� �� ��ֹ� ����
            NextStage(GameManager.Stage.currentStage);
            #region õ��/�Ǹ� ��ȯ �����ڵ�
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

            Debug.Log($"���� �������� : {GameManager.Stage.currentStage}");
        }
    }

    private void NextStage(int currentstage)
    {
        if (currentstage == 0) return;
        obstSpawner.notthistimeObstacle = false;
        isBossStage = false;
        Debug.Log(currentstage % 10 == 5 ? $"õ�� ��ȯ. ���� �������� : {currentstage}" : $"õ�� ��ȯ ���������� �ƴ�. ���� �������� : {currentstage}");
        Debug.Log(currentstage % 10 == 0 && currentstage != 0 ? $"���� �� �Ǹ� ��ȯ. ���� �������� : {currentstage}" : $"���� �� �Ǹ� ��ȯ ���������� �ƴ�. ���� �������� : {currentstage}");
        if (currentstage % 10 == GameManager.Stage.Select("angel"))
        {
            Debug.Log("õ�� ��ȯ");
            // õ�簡 ���� �������� => �� x , ��ֹ� x, ��Ż ON
            obstSpawner.notthistimeObstacle = true;
            DeSpawnObstacle();
            specialLevelUp.ADSpawn(GameManager.Stage.currentStage);
            Portal[] portal = FindObjectsOfType<Portal>();
            foreach (var p in portal) p.OpenPortal();
        }
        else if (currentstage % GameManager.Stage.Select("devil") == 0 && currentstage != 0)
        {
            // �Ǹ��� ���� �������� => ���� O, ��ֹ� x, ��Ż OFF
            Debug.Log("���� ����");
            obstSpawner.notthistimeObstacle = true;
            isBossStage = true;
            SpawnEnemy(isBossStage);

        }
        else if (currentstage > 20)
        {
            // �� é�Ͱ� �����.
            Debug.Log("��é�� Ŭ����");
            // �� é�Ͱ� �رݵǾ��ٴ� ������ �̵�.

            GameManager.Stage.InitStageClearCount(); // �������� �ʱ�ȭ
            Debug.Log($"�������� �ʱ�ȭ : {GameManager.Stage.currentStage}");
        }
        else
        {
            Debug.Log("�Ϲ� �� ��ȯ");
            // �Ǹ�, õ�縦 ��Ȱ��ȭ ���ִ� �κ�
            SpawnEnemy(isBossStage);
            DeSpawnAngel();
            DeSpawnDevil();
            // �Ϲ� �������� => �� O, ��ֹ� O, ��Ż OFF

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
