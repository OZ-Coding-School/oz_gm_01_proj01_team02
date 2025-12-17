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

    private void Start()
    {
        SpawnPoint = FindObjectsOfType<SpawnPoint>();
        fadeIn = FindObjectOfType<FadeIn>(true);
        cg = fadeIn.GetComponent<CanvasGroup>();
        obstSpawner = FindObjectOfType<ObstacleSpawner>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        portal = FindObjectsOfType<Portal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemySpawn.count = 0;
            enemySpawn.Spawn();
            foreach (var obst in FindObjectsOfType<Obstacle>())
            {
                if(obst.isActiveAndEnabled) obst.ReturnPool();
            }
            obstSpawner.alreadySpawned = false;

            int rand = Random.Range(0, SpawnPoint.Length);
            other.transform.position = SpawnPoint[rand].transform.position;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        foreach (var port in portal) port.ClosePortal();
        fadeIn.gameObject.SetActive(true);
        cg.alpha = 1f;
        yield return cg.DOFade(0f, duration)
                   .SetEase(Ease.OutQuad)
                   .SetUpdate(true)
                   .WaitForCompletion();
        cg.alpha = 1f;
        fadeIn.gameObject.SetActive(false);
    }



}
