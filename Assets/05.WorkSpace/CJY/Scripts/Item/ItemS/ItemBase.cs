using STH.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    PlayerController player;
    ItemSpawner spawner;
    [SerializeField] float duration = 1.8f; // 아이템이 빨려들어가는 시간

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<ItemSpawner>();
    }
    

    public void Collecting()
    {
        StartCoroutine(CollectionCo());
    }

    IEnumerator CollectionCo()
    {
        Debug.Log("수집 코루틴 동작");
        float current = 0f;

        while( current < duration)
        {
            current += Time.deltaTime;
            float t = current / duration;

            if(player != null)
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position, t);
            }

            yield return null;
        }
        if (Vector3.Distance(transform.position, player.transform.position) < 0.6f) {
            Debug.Log("0.6이하 근접");
            ReturnPool(); }

    }

    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

}
