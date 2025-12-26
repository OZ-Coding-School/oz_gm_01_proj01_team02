using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    //private Player target;
    //NavMeshAgent agent;
    //Vector3 startPos;
    //Obstacle obst;
    //public Vector3 endPos { get; private set; }

    //private void OnEnable()
    //{
    //    target = FindObjectOfType<Player>();
    //    agent = GetComponent<NavMeshAgent>();
    //    startPos = transform.position;
    //    StartCoroutine(Die());
    //    obst = FindObjectOfType<Obstacle>();
    //}

    //private void Update()
    //{
    //    if (this.gameObject.activeSelf)
    //    {
    //    }
    //    if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return;
        
    //}
    ////public void ReturnPool()
    ////{
    ////    if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);

    ////    if (GameManager.Stage.currentStage >= GameManager.Stage.Select("finish"))
    ////    {
    ////        GameManager.ClearChapter();
    ////    }
    ////}

    //IEnumerator Die()
    //{
    //    yield return new WaitForSeconds(2);
    //    transform.position = startPos;
    //    agent.enabled = false;
    //    endPos = transform.position;
    //}
}

