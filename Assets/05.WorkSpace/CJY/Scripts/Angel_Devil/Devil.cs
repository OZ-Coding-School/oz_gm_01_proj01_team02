using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    DevilUIManager devilPanel;

    private void Awake()
    {
        devilPanel = FindObjectOfType<DevilUIManager>(true);
    }
    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("악마 상호작용");
            // UI쪽 악마 특전 실행 코드
            devilPanel.gameObject.SetActive(true);
        }
    }
}
