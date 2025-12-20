using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    private Player target;
    NavMeshAgent agent;
    Vector3 startPos;
    Obstacle obst;

    private void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(Die());
        obst = FindObjectOfType<Obstacle>();
    }

    private void Start()
    {
        target = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            obst.ReturnPool();
        }
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return;
        agent.SetDestination(target.transform.position);
        
    }
    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        transform.position = startPos;
        agent.enabled = false;
        ReturnPool();
    }
}

