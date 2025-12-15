using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Player target;
    NavMeshAgent agent;
    Vector3 startPos;
    Portal portal;

    private void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(Die());
    }
    private void Start()
    {
        target = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        portal = GetComponents<Portal>();
        Debug.Log(agent.Warp(transform.position));
    }

    private void Update()
    {
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
        ReturnPool();
    }
}
