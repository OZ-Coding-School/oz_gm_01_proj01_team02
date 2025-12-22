using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Player target;
    NavMeshAgent agent;
    Vector3 startPos;
    public Vector3 endPos {get; private set;}

    private void OnEnable()
    {
        startPos = transform.position;
        StartCoroutine(Die());
    }
    private void Start()
    {
        target = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
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
        agent.enabled = false;
        endPos = transform.position;
        ReturnPool();
    }
}
