using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Player target;
    private float speed = 5;
    NavMeshAgent agent;

    private void Start()
    {
        NavMeshHit hit;
        target = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        Debug.Log(agent.Warp(transform.position));
        if (agent.Warp(transform.position))
        {
            agent.SetDestination(target.transform.position);
        }
    }
    private void Update()
    {
        
    }
}
