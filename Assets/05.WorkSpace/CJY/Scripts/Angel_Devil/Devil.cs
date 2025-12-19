using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("천사 상호작용");
            // UI쪽 천사 특전 실행 코드
        }
    }
}
