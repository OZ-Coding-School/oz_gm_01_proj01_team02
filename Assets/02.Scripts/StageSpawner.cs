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
    

    private void Start()
    {
        SpawnPoint = FindObjectsOfType<SpawnPoint>();
        fadeIn = FindObjectOfType<FadeIn>(true);
        cg = fadeIn.GetComponent<CanvasGroup>();
        Debug.Log(fadeIn is null);
        Debug.Log(cg is not null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌감지");
            int rand = Random.Range(0, SpawnPoint.Length);
            other.transform.position = SpawnPoint[rand].transform.position;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
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
