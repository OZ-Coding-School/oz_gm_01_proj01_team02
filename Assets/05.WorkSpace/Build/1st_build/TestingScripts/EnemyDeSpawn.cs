using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeSpawn : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(Die());
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        transform.position = startPos;
        agent.enabled = false;
        ReturnPool();
    }
}
