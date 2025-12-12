using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    private PlayerMove target;
    NavMeshAgent agent;

    private void Start()
    {
        target = FindObjectOfType<PlayerMove>();
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
