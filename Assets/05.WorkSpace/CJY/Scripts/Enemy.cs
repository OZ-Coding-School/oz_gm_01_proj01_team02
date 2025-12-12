using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Player target;
    NavMeshAgent agent;

    private void Start()
    {
        target = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();

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
