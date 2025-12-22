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
        
        float current = 0f;
        Vector3 startPos = transform.position;

        while( current < duration)
        {
            current += Time.deltaTime;
            float t = current / duration;

            if(player != null)
            {
                transform.position = Vector3.Lerp(startPos, player.transform.position, t);
            }

            yield return null;
        }

        if(player != null) transform.position = player.transform.position;
        if (Vector3.Distance(transform.position, player.transform.position) < 0.6f)
        {
            ReturnPool(); 
        }

    }

    public virtual void ReturnPool(){ }

}
